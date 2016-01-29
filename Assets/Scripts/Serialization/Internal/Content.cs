using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Serialization.Internal
{
    [Serializable]
    public class Content
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        [SerializeField] private string _name;
        [SerializeField] private SerializedValue _value;
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public Content([NotNull] string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public SerializedValue Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
