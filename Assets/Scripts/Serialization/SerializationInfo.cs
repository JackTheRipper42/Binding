using Assets.Scripts.Serialization.Internal;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Serialization
{
    [Serializable]
    public class SerializationInfo
    {
        private const int InitializeSize = 4;

        private readonly IFormatter _formatter;

        [SerializeField] private Content[] _values;
        [SerializeField] private int _count;

        public SerializationInfo()
            :this(new Formatter())
        {            
        }

        public SerializationInfo([NotNull] IFormatter formatter)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException("formatter");
            }

            _formatter = formatter;
            _values = new Content[InitializeSize];
            _count = 0;
        }

        public T GetValue<T>(string name)
        {
            var serializedContent = GetContent(name);
            if (serializedContent == null)
            {
                throw new KeyNotFoundException(string.Format("The key '{0}' is not found.", name));
            }

            var value = _formatter.Deserialize<T>(serializedContent.Value);
            return value;
        }

        public bool TryGetValue(string name, out object value)
        {
            var serializedContent = GetContent(name);
            if (serializedContent == null)
            {
                value = null;
                return false;
            }
            value = _formatter.Deserialize(serializedContent.Value);
            return true;
        }

        public void SetValue<T>([NotNull] string name, [CanBeNull] T value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            var serializedValue = _formatter.Serialize(value);
            StoreValue(name, serializedValue);
        }

        public void SetValue([NotNull] string name, [NotNull] Type type, [CanBeNull] object value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var serializedValue = _formatter.Serialize(type, value);
            StoreValue(name, serializedValue);
        }

        private void StoreValue(string name, SerializedValue serializedValue)
        {
            var serializedContent = GetContent(name);
            if (serializedContent == null)
            {
                serializedContent = new Content(name);
                AddContent(serializedContent);
            }
            serializedContent.Value = serializedValue;
        }

        private void AddContent(Content content)
        {
            if (_count >= _values.Length)
            {
                Array.Resize(ref _values, _values.Length*2);
            }
            _values[_count++] = content;
        }

        private Content GetContent(string name)
        {
            for (int i = 0; i < _count; i++)
            {
                var content = _values[i];
                if (content.Name == name)
                {
                    return content;
                }
            }
            return null;
        }
    }
}
