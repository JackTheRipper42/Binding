using System;

namespace Assets.Scripts.CooldownButtonTest
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PropertyTypeAttribute : Attribute
    {
        private readonly PropertyType _propertyType;

        public PropertyTypeAttribute(PropertyType propertyType)
        {
            _propertyType = propertyType;
        }

        public PropertyType PropertyType
        {
            get { return _propertyType; }
        }
    }
}