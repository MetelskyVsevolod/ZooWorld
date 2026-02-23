using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Animals.Core.Editor
{
    [CustomPropertyDrawer(typeof(MovementStrategyBase), true)]
    public class MovementStrategyPickerDrawer : PropertyDrawer
    {
        private static List<Type> _strategyTypes;

        private static List<Type> StrategyTypes
        {
            get
            {
                if (_strategyTypes != null)
                {
                    return _strategyTypes;
                }

                _strategyTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a =>
                    {
                        try
                        {
                            return a.GetTypes();
                        }
                        catch (ReflectionTypeLoadException e)
                        {
                            return e.Types.Where(t => t != null);
                        }
                    })
                    .Where(t => t.IsSubclassOf(typeof(MovementStrategyBase)) && !t.IsAbstract)
                    .OrderBy(t => t.Name)
                    .ToList();

                return _strategyTypes;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight;

            if (property.managedReferenceValue != null)
            {
                foreach (var child in GetVisibleChildren(property))
                {
                    height += EditorGUI.GetPropertyHeight(child, true) + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var dropdownRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var currentType = property.managedReferenceValue?.GetType();
            var displayName = currentType != null ? ObjectNames.NicifyVariableName(currentType.Name) : "— None —";

            if (EditorGUI.DropdownButton(dropdownRect, new GUIContent(label.text + ":  " + displayName), FocusType.Keyboard))
            {
                ShowDropdown(dropdownRect, property);
            }

            if (property.managedReferenceValue != null)
            {
                var y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.indentLevel++;

                foreach (var child in GetVisibleChildren(property))
                {
                    var h = EditorGUI.GetPropertyHeight(child, true);
                    EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), child, true);
                    y += h + EditorGUIUtility.standardVerticalSpacing;
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        private void ShowDropdown(Rect rect, SerializedProperty property)
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("None"), property.managedReferenceValue == null, () =>
            {
                property.managedReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
            });

            menu.AddSeparator(string.Empty);

            foreach (var type in StrategyTypes)
            {
                var t = type;
                var active = property.managedReferenceValue?.GetType() == t;
                menu.AddItem(new GUIContent(ObjectNames.NicifyVariableName(t.Name)), active, () =>
                {
                    property.managedReferenceValue = Activator.CreateInstance(t);
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            menu.DropDown(rect);
        }

        private static IEnumerable<SerializedProperty> GetVisibleChildren(SerializedProperty parent)
        {
            var copy = parent.Copy();
            var end = parent.GetEndProperty();
            var entering = true;

            while (copy.NextVisible(entering) && !SerializedProperty.EqualContents(copy, end))
            {
                entering = false;
                yield return copy.Copy();
            }
        }
    }
}