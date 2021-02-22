using UnityEngine;
using UnityEditor;

namespace Bewildered.Tags.Editor
{
    [CustomPropertyDrawer(typeof(Tag))]
    internal class TagPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool showLabel = !string.IsNullOrEmpty(label.text);

            GUIContent dropdownLabel = new GUIContent();
            Tag descriptor = property.objectReferenceValue as Tag;
            if (descriptor != null)
                dropdownLabel.text = descriptor.Title;

            Rect labelRect = new Rect(position);
            labelRect.width = EditorGUIUtility.labelWidth;

            Rect fieldRect = new Rect(position);
            if (showLabel)
            {
                fieldRect.x += labelRect.width;
                fieldRect.width -= labelRect.width;
            }

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                if (showLabel)
                    EditorGUI.PrefixLabel(labelRect, label);

                if (EditorGUI.DropdownButton(fieldRect, dropdownLabel, FocusType.Keyboard))
                {
                    TagDropdown dropdown = new TagDropdown()
                    {
                        TagProperty = property
                    };
                    dropdown.IsChangeProperty = true;
                    dropdown.Reload();
                    dropdown.Show(GUIUtility.GUIToScreenRect(fieldRect));
                }
            }
        }
    } 
}
