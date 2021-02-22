using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Bewildered.Editor;

namespace Bewildered.Tags.Editor
{
    public class TagDropdown : AdvancedDropdownView
    {
        public SerializedProperty TagsArrayProperty { get; set; }
        public SerializedProperty TagProperty { get; set; }

        /// <summary>
        /// <c>true</c> if on select it sets the value of <see cref="TagProperty"/>; otherwise <c>false</c> if it adds an item to <see cref="TagsArrayProperty"/>.
        /// </summary>
        public bool IsChangeProperty { get; set; } = false;

        static class Styles
        {
            public static GUIContent ItemCollapisedContent;
            public static GUIContent TagIconContent;

            static Styles()
            {
                ItemCollapisedContent = EditorGUIUtility.IconContent("Folder Icon");
                TagIconContent = EditorGUIUtility.IconContent("FilterByLabel");
            }
        }

        protected override DropdownItem BuildRoot()
        {
            TagDropdownItem root = new TagDropdownItem("Tags");

            foreach (var descriptor in TagsManager.Tags)
            {
                if (descriptor != null)
                {
                    if (TagsArrayProperty != null)
                    {
                        if (TagsArrayProperty.ExistsInArray(p => p.objectReferenceValue == descriptor))
                            continue;
                    }

                    if (TagProperty != null)
                    {
                        if (TagProperty.objectReferenceValue == descriptor)
                            continue;
                    }

                    var leafDescriptorItem = root.CreatePath(descriptor.Path.Split('/'), 0);
                    leafDescriptorItem.Tag = descriptor;
                }
            }
            root.Order();

            return root;
        }

        protected override void BindItem(VisualElement element, int index)
        {
            Label label = element.Q<Label>();
            label.text = ParentItem.Children[index].Name;

            Image icon = element.Q<Image>("icon");

            if (ParentItem.Children[index].HasChildren)
            {
                icon.image = Styles.ItemCollapisedContent.image;
            }
            else
            {
                icon.image = Styles.TagIconContent.image;
            }
        }

        protected override void ItemSelected(DropdownItem item)
        {
            if (TagsArrayProperty != null && !IsChangeProperty)
            {
                TagsArrayProperty.AddAndGetArrayElement().objectReferenceValue = ((TagDropdownItem)item).Tag;
                TagsArrayProperty.serializedObject.ApplyModifiedProperties();
            }

            if (TagProperty != null && IsChangeProperty)
            {
                TagProperty.objectReferenceValue = ((TagDropdownItem)item).Tag;
                TagProperty.serializedObject.ApplyModifiedProperties();
            }
        }
    } 
}
