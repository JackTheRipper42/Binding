using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CooldownButtonTest
{
    [CustomEditor(typeof(ResourceManager))]
    public class ResourceManagerEditor : Editor
    {
        private const float FieldWidth = 400f;
        private const float LabelWidth = 120f;

        private readonly GUILayoutOption _layelWidth = GUILayout.Width(LabelWidth);
        private readonly GUILayoutOption _fieldMaxWidth = GUILayout.MaxWidth(FieldWidth);

        private ResourceManager _resourceManager;
        private PropertyInfo[] _properties;

        public override void OnInspectorGUI()
        {
            foreach (var property in _properties)
            {
                var propertyType = property.PropertyType;
                var propertyAttribute = property.GetCustomAttributes(typeof(PropertyTypeAttribute), false)
                    .Cast<PropertyTypeAttribute>()
                    .SingleOrDefault();
                var displayNameAttribute = property.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                    .Cast<DisplayNameAttribute>()
                    .SingleOrDefault();

                var name = displayNameAttribute != null
                    ? displayNameAttribute.DisplayName
                    : property.Name;

                if (propertyAttribute != null)
                {
                    switch (propertyAttribute.PropertyType)
                    {
                        case PropertyType.Tag:
                            AddProperty(
                                name,
                                property,
                                () => EditorGUILayout.TagField(GetValue<string>(property), _fieldMaxWidth));
                            break;
                        case PropertyType.Layer:
                            AddProperty(
                                name,
                                property,
                                () => EditorGUILayout.LayerField(GetValue<int>(property), _fieldMaxWidth));
                            break;
                        case PropertyType.Passwort:
                            AddProperty(
                                name,
                                property,
                                () => EditorGUILayout.PasswordField(GetValue<string>(property), _fieldMaxWidth));
                            break;
                        default:
                            throw new NotSupportedException(string.Format(
                                "The type '{0}' is not supported.",
                                propertyAttribute.PropertyType));
                    }
                }
                else if (propertyType == typeof(Color))
                {
                    AddProperty(
                        name,
                        property,
                        () => EditorGUILayout.ColorField(GetValue<Color>(property), _fieldMaxWidth));
                }
                else if (propertyType == typeof(float))
                {
                    AddProperty(
                        name,
                        property,
                        () => EditorGUILayout.DelayedFloatField(GetValue<float>(property), _fieldMaxWidth));
                }
                else if (propertyType == typeof(string))
                {
                    AddProperty(
                        name,
                        property,
                        () => EditorGUILayout.DelayedTextField(GetValue<string>(property), _fieldMaxWidth));
                }
                else if (propertyType == typeof(int))
                {
                    AddProperty(
                        name,
                        property,
                        () => EditorGUILayout.DelayedIntField(GetValue<int>(property), _fieldMaxWidth));
                }
                else if (propertyType == typeof(long))
                {
                    AddProperty(
                        name,
                        property,
                        () => EditorGUILayout.LongField(GetValue<long>(property), _fieldMaxWidth));
                }
                else if (propertyType == typeof(double))
                {
                    AddProperty(
                        name,
                        property,
                        () => EditorGUILayout.DoubleField(GetValue<double>(property), _fieldMaxWidth));
                }
                else if (propertyType == typeof(bool))
                {
                    AddProperty(
                        name,
                        property,
                        () => EditorGUILayout.Toggle(GUIContent.none, GetValue<bool>(property), _fieldMaxWidth));
                }
                else if (propertyType == typeof(Vector2))
                {
                    AddProperty(
                        name,
                        property,
                        () => EditorGUILayout.Vector2Field(GUIContent.none, GetValue<Vector2>(property), _fieldMaxWidth));
                }
                else if (propertyType == typeof(Vector3))
                {
                    AddProperty(
                        name,
                        property,
                        () => EditorGUILayout.Vector3Field(GUIContent.none, GetValue<Vector3>(property), _fieldMaxWidth));
                }
                else if (propertyType == typeof(Bounds))
                {
                    AddProperty(
                        name,
                        property,
                        () => EditorGUILayout.BoundsField(GetValue<Bounds>(property), _fieldMaxWidth));
                }
                else if (propertyType == typeof(Rect))
                {
                    AddProperty(
                        name,
                        property,
                        () => EditorGUILayout.RectField(GetValue<Rect>(property), _fieldMaxWidth));
                }
                else if (propertyType.IsEnum)
                {
                    AddProperty(
                        name,
                        property,
                        () => EditorGUILayout.EnumMaskField(GetValue<Enum>(property), _fieldMaxWidth));
                }
                else if (typeof(UnityEngine.Object).IsAssignableFrom(propertyType))
                {
                    AddProperty(
                        name,
                        property,
                        () => EditorGUILayout.ObjectField(
                            GetValue<UnityEngine.Object>(property),
                            propertyType,
                            true,
                            _fieldMaxWidth));
                }
            }
        }

        public void OnEnable()
        {
            _resourceManager = (ResourceManager) target;
            _properties =
                typeof(ResourceManager).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(SelectProperty)
                    .ToArray();
        }

        private static bool SelectProperty(PropertyInfo property)
        {
            var getter = property.GetGetMethod();
            var setter = property.GetSetMethod();
            if (getter == null || setter == null)
            {
                return false;
            }
            return getter.IsPublic && setter.IsPublic;
        }

        private void AddProperty<T>(string name, PropertyInfo property, Func<T> fieldFunc)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(property.Name, _layelWidth);
            var value = fieldFunc();
            SetValue(property, value);
            GUILayout.EndHorizontal();
        }

        private void SetValue<T>(PropertyInfo property, T value)
        {
            property.SetValue(_resourceManager, value, null);
        }

        private T GetValue<T>(PropertyInfo property)
        {
            return (T) property.GetValue(_resourceManager, null);
        }
    }
}
