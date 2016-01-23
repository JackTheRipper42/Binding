using Assets.Scripts.Binding;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.CooldownButtonTest
{
    public class ResourceManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private Color _buttonEnabledColor;
        [SerializeField] private Color _buttonDisabledColor;
        [SerializeField] private Color _buttonEnabledTextColor;
        [SerializeField] private Color _buttonDisabledTextColor;
        [SerializeField] private Color _buttonEnabledProgressBarColor;
        [SerializeField] private Color _buttonDisabledProgressBarColor;
        [SerializeField] private CategoryFoldout[] _serializedCategoryFoldouts; 

        private readonly NotifyingObject<Color> _buttonEnabledColorProperty = new NotifyingObject<Color>();
        private readonly NotifyingObject<Color> _buttonDisabledColorProperty = new NotifyingObject<Color>();
        private readonly NotifyingObject<Color> _buttonEnabledTextColorProperty = new NotifyingObject<Color>();
        private readonly NotifyingObject<Color> _buttonDisabledTextColorProperty = new NotifyingObject<Color>();
        private readonly NotifyingObject<Color> _buttonEnabledProgressBarColorProperty = new NotifyingObject<Color>();
        private readonly NotifyingObject<Color> _buttonDisabledProgressBarColorProperty = new NotifyingObject<Color>();

        private List<CategoryProperties> _categoryProperties;
        private Dictionary<string, bool> _categoryFoldouts;

        public ResourceManager()
        {
            FindProperties();
        }

        internal List<CategoryProperties> CategoryProperties
        {
            get { return _categoryProperties; }
        }

        internal Dictionary<string, bool> CategoryFoldouts
        {
            get { return _categoryFoldouts; }
        }

        [DisplayName("button enabled")]
        [Category("button colors")]
        public NotifyingObject<Color> ButtonEnabledColorProperty
        {
            get { return _buttonEnabledColorProperty; }
        }

        [DisplayName("button disabled")]
        [Category("button colors")]
        public NotifyingObject<Color> ButtonDisabledColorProperty
        {
            get { return _buttonDisabledColorProperty; }
        }

        [DisplayName("text enabled")]
        [Category("button colors")]
        public NotifyingObject<Color> ButtonEnabledTextColorProperty
        {
            get { return _buttonEnabledTextColorProperty; }
        }

        [DisplayName("text disabled")]
        [Category("button colors")]
        public NotifyingObject<Color> ButtonDisabledTextColorProperty
        {
            get { return _buttonDisabledTextColorProperty; }
        }

        [DisplayName("progress bar enabled")]
        [Category("button colors")]
        public NotifyingObject<Color> ButtonEnabledProgressBarColorProperty
        {
            get { return _buttonEnabledProgressBarColorProperty; }
        }

        [DisplayName("progress bar disabled")]
        [Category("button colors")]
        public NotifyingObject<Color> ButtonDisabledProgressBarColorProperty
        {
            get { return _buttonDisabledProgressBarColorProperty; }
        }

        public void OnBeforeSerialize()
        {
            _buttonEnabledColor = ButtonEnabledColorProperty.GetValue();
            _buttonDisabledColor = ButtonDisabledColorProperty.GetValue();
            _buttonEnabledTextColor = ButtonEnabledTextColorProperty.GetValue();
            _buttonDisabledTextColor = ButtonDisabledTextColorProperty.GetValue();
            _buttonEnabledProgressBarColor = ButtonEnabledProgressBarColorProperty.GetValue();
            _buttonDisabledProgressBarColor = ButtonDisabledProgressBarColorProperty.GetValue();

            _serializedCategoryFoldouts = _categoryFoldouts.Select(
                categoryFoldout => new CategoryFoldout(categoryFoldout.Key, categoryFoldout.Value))
                .ToArray();
        }

        public void OnAfterDeserialize()
        {
            ButtonEnabledColorProperty.SetValue(_buttonEnabledColor);
            ButtonDisabledColorProperty.SetValue(_buttonDisabledColor);
            ButtonEnabledTextColorProperty.SetValue(_buttonEnabledTextColor);
            ButtonDisabledTextColorProperty.SetValue(_buttonDisabledTextColor);
            ButtonEnabledProgressBarColorProperty.SetValue(_buttonEnabledProgressBarColor);
            ButtonDisabledProgressBarColorProperty.SetValue(_buttonDisabledProgressBarColor);

            foreach (var categoryFoldout in _serializedCategoryFoldouts)
            {
                if (_categoryFoldouts.ContainsKey(categoryFoldout.Name))
                {
                    _categoryFoldouts[categoryFoldout.Name] = categoryFoldout.Foldout;
                }
            }
        }

        private void FindProperties()
        {
            _categoryProperties = typeof(ResourceManager).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(IsNotifyingObject)
                .Select(property => new NotifyingObjectProperty(property, this))
                .Where(IsSupported)
                .OrderBy(property => property.DisplayName)
                .GroupBy(property => property.Category)
                .Select(group => new CategoryProperties(group.Key, group.ToArray()))
                .OrderBy(group => group.Category)
                .ToList();

            _categoryFoldouts = _categoryProperties.ToDictionary(group => group.Category, category => false);
        }

        private static bool IsNotifyingObject(PropertyInfo property)
        {
            var getter = property.GetGetMethod();
            if (getter == null || !getter.IsPublic)
            {
                return false;
            }

            var type = property.PropertyType;
            if (!type.IsGenericType)
            {
                return false;
            }

            var genericTypeDefinition = type.GetGenericTypeDefinition();

            return genericTypeDefinition == typeof(NotifyingObject<>);
        }

        private static bool IsSupported(NotifyingObjectProperty property)
        {
            var propertyType = property.Type;
            switch (property.PropertyKind)
            {
                case PropertyKind.Undefined:
                    return propertyType == typeof(Color) ||
                           propertyType == typeof(float) ||
                           propertyType == typeof(string) ||
                           propertyType == typeof(int) ||
                           propertyType == typeof(long) ||
                           propertyType == typeof(double) ||
                           propertyType == typeof(bool) ||
                           propertyType == typeof(Vector2) ||
                           propertyType == typeof(Vector3) ||
                           propertyType == typeof(Bounds) ||
                           propertyType == typeof(Rect) ||
                           propertyType.IsEnum ||
                           typeof(Object).IsAssignableFrom(propertyType);
                case PropertyKind.Tag:
                    return propertyType == typeof(string);
                case PropertyKind.Layer:
                    return propertyType == typeof(int);
                case PropertyKind.Passwort:
                    return propertyType == typeof(string);
                default:
                    return false;
            }
        }
    }
}
