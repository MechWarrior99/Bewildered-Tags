using UnityEngine;
using UnityEditor;
using Bewildered.Core.Editor;

namespace Bewildered.Tags.Editor
{
    [CustomPropertyDrawer(typeof(TagSet))]
    internal class TagSetPropertyDrawer : PropertyDrawer
    {
        private ReorderablePropertyList _list;
        private SerializedProperty _property;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_list != null)
                _list.Draw(position);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (_list == null)
            {
                _property = property;
                _list = new ReorderablePropertyList(property.FindPropertyRelative("_hashsetList"), true, false, false, false);
                _list.DrawElementGUI += ElementGUI;
                _list.OnAddDropdown += OnAddDropdown;
            }

            if (_list != null)
                return _list.GetHeight();
            else
                return EditorGUIUtility.singleLineHeight;
        }

        private void OnAddDropdown(Rect rect, ReorderablePropertyList list)
        {
            TagDropdown dropdown = new TagDropdown()
            {
                TagsArrayProperty = _property.FindPropertyRelative("_hashsetList"),
                IsChangeProperty = false
            };
            dropdown.Reload();
            dropdown.Show(GUIUtility.GUIToScreenRect(rect));
        }

        private void ElementGUI(Rect rect, SerializedProperty elementProperty)
        {
            rect.height = EditorGUI.GetPropertyHeight(elementProperty, GUIContent.none, true);
            rect.y += 1;

            GUIContent dropdownLabel = new GUIContent();
            Tag descriptor = elementProperty.objectReferenceValue as Tag;
            if (descriptor != null)
                dropdownLabel.text = descriptor.Title;

            using (new EditorGUI.PropertyScope(rect, GUIContent.none, elementProperty))
            {
                if (EditorGUI.DropdownButton(rect, dropdownLabel, FocusType.Keyboard))
                {
                    TagDropdown dropdown = new TagDropdown()
                    {
                        TagProperty = elementProperty,
                        TagsArrayProperty = _property.FindPropertyRelative("_hashsetList"),
                        IsChangeProperty = true
                    };
                    dropdown.Reload();
                    dropdown.Show(GUIUtility.GUIToScreenRect(rect));
                }
            }
        }
    } 
}
