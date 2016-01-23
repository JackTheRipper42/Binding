using JetBrains.Annotations;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.CooldownButtonTest
{
    [Serializable]
    public struct SerializedValue
    {
        private enum Serialization
        {
            BinaryFormatter,
            UnityJson
        }

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        [SerializeField] private string _name;
        [SerializeField] private string _assemblyQualifiedName;
        [SerializeField] private byte[] _binaryData;
        [SerializeField] private string _jsonData;
        [SerializeField] private Serialization _serialization;
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public SerializedValue([NotNull] string name, [NotNull] Type type, [CanBeNull] object value)
            : this()
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            _name = name;
            _assemblyQualifiedName = type.AssemblyQualifiedName;

            if (type.Namespace == null)
            {
                throw new InvalidOperationException("The namespace is null");
            }

            if (type.Namespace.StartsWith("UnityEngine"))
            {
                if (value == null)
                {
                    _jsonData = string.Empty;
                }
                else
                {
                    _jsonData = JsonUtility.ToJson(value, false);
                }
                _serialization = Serialization.UnityJson;
            }
            else
            {
                var formatter = new BinaryFormatter();
                if (value != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        formatter.Serialize(stream, value);
                        var length = stream.Position;
                        stream.Position = 0;
                        _binaryData = new byte[length];
                        stream.Read(_binaryData, 0, (int) length);
                    }
                }
                else
                {
                    _binaryData = new byte[0];
                }
                _serialization = Serialization.BinaryFormatter;
            }
        }

        public string Name
        {
            get { return _name; }
        }

        public string AssemblyQualifiedName
        {
            get { return _assemblyQualifiedName; }
        }

        public object Deserialize()
        {
            switch (_serialization)
            {
                case Serialization.BinaryFormatter:
                    if (_binaryData.Length == 0)
                    {
                        return null;
                    }

                    var formatter = new BinaryFormatter();
                    using (var stream = new MemoryStream(_binaryData))
                    {
                        return formatter.Deserialize(stream);
                    }
                case Serialization.UnityJson:
                    if (string.IsNullOrEmpty(_jsonData))
                    {
                        return null;
                    }
                    return JsonUtility.FromJson(_jsonData, Type.GetType(_assemblyQualifiedName));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
