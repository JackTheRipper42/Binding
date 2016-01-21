using Assets.Scripts.Binding;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CooldownButtonTest
{
    [CustomEditor(typeof(ResourceManager))]
    public class ResourceManagerEditor : Editor
    {
        private NotifyingObjectProperty[] _properties;

        public override void OnInspectorGUI()
        {
            foreach (var property in _properties)
            {
                var propertyType = property.Type;

                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent(property.DisplayName, property.description));
                GUILayout.FlexibleSpace();

                switch (property.PropertyKind)
                {
                    case PropertyKind.Undefined:
                        if (propertyType == typeof(Color))
                        {
                            property.SetValue(EditorGUILayout.ColorField(property.GetValue<Color>()));
                        }
                        else if (propertyType == typeof(float))
                        {
                            property.SetValue(
                                EditorGUILayout.DelayedFloatField(
                                    property.GetValue<float>()));
                        }
                        else if (propertyType == typeof(string))
                        {
                            property.SetValue(
                                EditorGUILayout.DelayedTextField(
                                    property.GetValue<string>()));
                        }
                        else if (propertyType == typeof(int))
                        {
                            property.SetValue(EditorGUILayout.DelayedIntField(property.GetValue<int>()));
                        }
                        else if (propertyType == typeof(long))
                        {
                            property.SetValue(EditorGUILayout.LongField(property.GetValue<long>()));
                        }
                        else if (propertyType == typeof(double))
                        {
                            property.SetValue(EditorGUILayout.DoubleField(property.GetValue<double>()));
                        }
                        else if (propertyType == typeof(bool))
                        {
                            property.SetValue(EditorGUILayout.Toggle(
                                GUIContent.none,
                                property.GetValue<bool>()));
                        }
                        else if (propertyType == typeof(Vector2))
                        {
                            property.SetValue(EditorGUILayout.Vector2Field(GUIContent.none, property.GetValue<Vector2>()));
                        }
                        else if (propertyType == typeof(Vector3))
                        {
                            property.SetValue(EditorGUILayout.Vector3Field(
                                GUIContent.none,
                                property.GetValue<Vector3>()));
                        }
                        else if (propertyType == typeof(Bounds))
                        {
                            property.SetValue(EditorGUILayout.BoundsField(property.GetValue<Bounds>()));
                        }
                        else if (propertyType == typeof(Rect))
                        {
                            property.SetValue(EditorGUILayout.RectField(property.GetValue<Rect>()));
                        }
                        else if (propertyType.IsEnum)
                        {
                            property.SetValue(EditorGUILayout.EnumMaskField(property.GetValue<Enum>()));
                        }
                        else if (typeof(UnityEngine.Object).IsAssignableFrom(propertyType))
                        {
                            property.SetValue(EditorGUILayout.ObjectField(
                                property.GetValue<UnityEngine.Object>(),
                                propertyType,
                                true));
                        }
                        break;
                    case PropertyKind.Tag:
                        property.SetValue(EditorGUILayout.TagField(property.GetValue<string>()));
                        break;
                    case PropertyKind.Layer:
                        property.SetValue(EditorGUILayout.LayerField(property.GetValue<int>()));
                        break;
                    case PropertyKind.Passwort:
                        property.SetValue(EditorGUILayout.PasswordField(property.GetValue<string>()));
                        break;
                    default:
                        throw new NotImplementedException();
                }

                GUILayout.EndHorizontal();
            }
        }

        public void OnEnable()
        {
            var resourceManager = (ResourceManager) target;
            _properties =
                typeof(ResourceManager).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(IsNotifyingObject)
                    .Select(property => SelectNotifyingObjectProperty(property, resourceManager))
                    .Where(IsSupported)
                    .ToArray();
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
                           typeof(UnityEngine.Object).IsAssignableFrom(propertyType);
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

        private static NotifyingObjectProperty SelectNotifyingObjectProperty(PropertyInfo property, object obj)
        {
            return new NotifyingObjectProperty(property, obj);
        }
    }
}
