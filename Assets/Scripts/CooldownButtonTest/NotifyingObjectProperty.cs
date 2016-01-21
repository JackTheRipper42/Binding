using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Assets.Scripts.CooldownButtonTest
{
    public class NotifyingObjectProperty
    {
        private readonly Type _type;
        private readonly MethodInfo _getter;
        private readonly MethodInfo _setter;
        private readonly object _notifyingObject;
        private readonly string _displayName;
        private readonly string _description;
        private readonly PropertyKind _propertyKind;

        public NotifyingObjectProperty(PropertyInfo property, object obj)
        {
            _type = property.PropertyType.GetGenericArguments()[0];
            _notifyingObject = property.GetValue(obj, null);
            _getter = _notifyingObject.GetType().GetMethod("GetValue");
            _setter = _notifyingObject.GetType().GetMethod("SetValue");

            var displayNameAttribute = property.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                .Cast<DisplayNameAttribute>()
                .SingleOrDefault();

            _displayName = displayNameAttribute != null
                ? displayNameAttribute.DisplayName
                : property.Name;

            var descriptionAttribute = property.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .SingleOrDefault();

            _description = descriptionAttribute != null
                ? descriptionAttribute.Description
                : string.Empty;

            var propertyKindAttribut = property.GetCustomAttributes(typeof(PropertyKindAttribute), false)
                .Cast<PropertyKindAttribute>()
                .SingleOrDefault();

            _propertyKind = propertyKindAttribut != null
                ? propertyKindAttribut.PropertyKind
                : PropertyKind.Undefined;
        }

        public Type Type
        {
            get { return _type; }
        }

        public string DisplayName
        {
            get { return _displayName; }
        }

        public string description
        {
            get { return _description; }
        }

        public PropertyKind PropertyKind
        {
            get { return _propertyKind; }
        }

        public void SetValue<T>(T value)
        {
            _setter.Invoke(_notifyingObject, new object[] {value});
        }

        public T GetValue<T>()
        {
            return (T) _getter.Invoke(_notifyingObject, null);
        }
    }
}
