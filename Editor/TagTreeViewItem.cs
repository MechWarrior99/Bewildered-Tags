using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Bewildered.Tags.Editor
{
    internal class TagTreeViewItem : TreeViewItem
    {
        public SerializedProperty Property { get; set; }

        public bool IsGroup { get; set; } = true;

        public int Index { get; set; }

        public string Path { get; set; }

        /// <inheritdoc/>
        public TagTreeViewItem(int id) : base(id)
        {

        }

        /// <inheritdoc/>
        public TagTreeViewItem(int id, int depth) : base(id, depth)
        {

        }

        public TagTreeViewItem(int id, string name, bool isGroup) : base(id)
        {
            displayName = name;
            IsGroup = isGroup;
        }

        public TagTreeViewItem Find(string itemName, bool requireIsGroup = false, int descriptorIndex = -1)
        {
            if (children == null || string.IsNullOrEmpty(itemName))
                return null;

            foreach (TagTreeViewItem child in children)
            {
                if (child.displayName == itemName && (!requireIsGroup || child.IsGroup) && (descriptorIndex < 0 || child.Index == descriptorIndex))
                    return child;
            }

            return null;
        }

        public TagTreeViewItem CreatePath(string[] groups, int descriptorIndex, int groupIndex, ref int id)
        {
            bool isGroup = groupIndex < groups.Length - 1;

            var child = Find(groups[groupIndex], isGroup, isGroup ? -1 : descriptorIndex);
            if (child == null)
            {
                child = new TagTreeViewItem(id, groups[groupIndex], isGroup);
                child.Path = string.Join("/", groups.Take(groupIndex + 1));
                if (!isGroup)
                    child.Index = descriptorIndex;
                id++;
                AddChild(child);
            }

            if (!isGroup)
                return child;

            return child.CreatePath(groups, descriptorIndex, ++groupIndex, ref id);
        }

        public List<TagTreeViewItem> GetLeafItems()
        {
            List<TagTreeViewItem> items = new List<TagTreeViewItem>();
            if (!IsGroup)
            {
                items.Add(this);
                return items;
            }

            foreach (TagTreeViewItem child in children)
            {
                items.AddRange(child.GetLeafItems());
            }

            return items;
        }

        /// <summary>
        /// Orders all children recursivly by whether it is a group, then alphabetically by name.
        /// </summary>
        public void Order()
        {
            if (!hasChildren)
                return;

            children = children.OrderBy(item => !((TagTreeViewItem)item).IsGroup).ThenBy(item => item.displayName).ToList();
            foreach (TagTreeViewItem child in children)
            {
                child.Order();
            }
        }
    }
}
