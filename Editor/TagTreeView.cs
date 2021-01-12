using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Bewildered.Tags.Editor
{
    internal class TagTreeView : TreeView
    {
        private int _lastId = 1;
        public SerializedProperty _tagsProperty;
        private List<TagTreeViewItem> _allLeafItems = new List<TagTreeViewItem>();

        public bool showAsList = false;

        static class Styles
        {
            public static GUIStyle EmptyButtonStyle;
            public static GUIContent EditFullpathContent;
            public static GUIContent ItemCollapisedContent;
            public static GUIContent ItemExpandedContent;
            public static GUIContent TagIconContent;

            static Styles()
            {
                EmptyButtonStyle = new GUIStyle("TV Line");
                EmptyButtonStyle.alignment = TextAnchor.MiddleCenter;

                EditFullpathContent = EditorGUIUtility.IconContent("editicon.sml");
                ItemCollapisedContent = EditorGUIUtility.IconContent("Folder Icon");
                ItemExpandedContent = EditorGUIUtility.IconContent("d_OpenedFolder Icon");
                TagIconContent = EditorGUIUtility.IconContent("FilterByLabel");
            }
        }

        public TagTreeView(SerializedProperty tagsProperty, TreeViewState state) : base(state)
        {
            _tagsProperty = tagsProperty;
            rowHeight = 21;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            _allLeafItems.Clear();
            _lastId = 1;

            TagTreeViewItem root = new TagTreeViewItem(0)
            {
                depth = -1,
                displayName = ""
            };
            root.children = new List<TreeViewItem>();
            
            for (int i = 0; i < _tagsProperty.arraySize; i++)
            {
                SerializedProperty tagProperty = _tagsProperty.GetArrayElementAtIndex(i);

                if (tagProperty.objectReferenceValue == null)
                {
                    _tagsProperty.DeleteArrayElementAtIndex(i);
                    _tagsProperty.serializedObject.ApplyModifiedProperties();
                    i--;
                    continue;
                }

                SerializedObject serializedTag = new SerializedObject(tagProperty.objectReferenceValue);

                string path = serializedTag.FindProperty("_path").stringValue;
                string[] splitPath = path.Split('/');

                if (!showAsList)
                {
                    TagTreeViewItem item = root.CreatePath(splitPath, i, 0, ref _lastId);
                    item.Property = tagProperty;
                    _allLeafItems.Add(item);
                }
                else
                {
                    TagTreeViewItem item = new TagTreeViewItem(_lastId, splitPath.Last(), false)
                    {
                        Path = path,
                        Index = i
                    };

                    _lastId++;
                    root.AddChild(item);
                    item.Property = tagProperty;
                    _allLeafItems.Add(item);
                }
               
            }

            root.Order();
            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            TagTreeViewItem item = (TagTreeViewItem)args.item;
            float editPathButtonWidth = 25.0f;
            Rect editPathButtonRect = new Rect()
            {
                x = args.rowRect.x + args.rowRect.width - editPathButtonWidth,
                y = args.rowRect.y,
                width = editPathButtonWidth,
                height = args.rowRect.height
            };
            
            if (GUI.Button(editPathButtonRect, Styles.EditFullpathContent.image, Styles.EmptyButtonStyle))
            {
                args.item.displayName = item.Path;
                BeginRename(args.item);
            }

            Rect labelRect = new Rect(args.rowRect);
            labelRect.x += 15;
            if (!hasSearch)
                labelRect.x += args.item.depth * depthIndentWidth;
            Texture icon = item.IsGroup ? IsExpanded(args.item.id) ? Styles.ItemExpandedContent.image : Styles.ItemCollapisedContent.image : Styles.TagIconContent.image;
            EditorGUI.LabelField(labelRect, new GUIContent(args.item.displayName, icon));
        }

        protected override bool DoesItemMatchSearch(TreeViewItem item, string search)
        {
            TagTreeViewItem tagItem = item as TagTreeViewItem;
            if (tagItem.IsGroup)
                return false;

            tagItem.displayName = tagItem.Path;

            return base.DoesItemMatchSearch(item, search);
        }

        protected override void SearchChanged(string newSearch)
        {
            if (string.IsNullOrEmpty(newSearch))
            {
                foreach (var item in _allLeafItems)
                {
                    item.displayName = item.Path.Split('/').Last();
                }
            }
        }

        protected override void KeyEvent()
        {
            base.KeyEvent();

            Event evt = Event.current;

            if (evt.type == EventType.KeyDown && evt.keyCode == KeyCode.Delete)
            {
                 IList<int> selectedIds = GetSelection();
                if (selectedIds.Count > 0)
                {
                    TagTreeViewItem selectedItem = (TagTreeViewItem)FindItem(selectedIds[0], rootItem);
                    if (!selectedItem.IsGroup)
                    {
                        TagEditorUtility.RemoveTag(_tagsProperty, selectedItem.Index);
                    }
                    else
                    {
                        // Get and then delete all children items of the group.
                        var items = selectedItem.GetLeafItems();
                        items.OrderByDescending(i => i.Index);
                        foreach (var item in items)
                        {
                            TagEditorUtility.RemoveTag(_tagsProperty, item.Index);
                        } 
                    }

                    _tagsProperty.serializedObject.ApplyModifiedProperties();
                    Reload();
                }
            }
        }

        protected override void DoubleClickedItem(int id)
        {
            BeginRename(FindItem(id, rootItem));
        }

        protected override bool CanRename(TreeViewItem item)
        {
            return true;
        }

        protected override void RenameEnded(RenameEndedArgs args)
        {
            if (string.IsNullOrEmpty(args.newName))
                return;

            TagTreeViewItem tagItem = (TagTreeViewItem)FindItem(args.itemID, rootItem);

            if (!tagItem.IsGroup)
            {
                RenameLeafItem(tagItem, args.originalName, args.newName);
            }
            else
            {
                List<TagTreeViewItem> items = tagItem.GetLeafItems();
                foreach (var item in items)
                {
                    RenameLeafItem(item, args.originalName, args.newName);
                }
            }

            if (args.newName.Contains("/") && args.newName != args.originalName)
            {
                Reload();
                return;
            }

            tagItem.displayName = args.newName;
        }

        private void RenameLeafItem(TagTreeViewItem item, string originalName, string newName)
        {
            Tag tag = (Tag)item.Property.objectReferenceValue;
            SerializedObject serializedTag = new SerializedObject(tag);

            string path = serializedTag.FindProperty("_path").stringValue;
            path = string.IsNullOrEmpty(originalName) ? newName : path.Replace(originalName, newName);
            serializedTag.FindProperty("_path").stringValue = path;
            serializedTag.ApplyModifiedProperties();
        }
    } 
}
