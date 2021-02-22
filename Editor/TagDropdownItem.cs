using System.Linq;
using Bewildered.Editor;

namespace Bewildered.Tags.Editor
{
    internal class TagDropdownItem : DropdownItem
    {
        public bool IsGroup { get; set; }

        public Tag Tag { get; set; }

        public TagDropdownItem(string name, bool isGroup = false) : base(name)
        {
            IsGroup = isGroup;
        }

        public TagDropdownItem Find(string itemName, bool requireIsGroup = false)
        {
            foreach (TagDropdownItem child in Children)
            {
                if (child.Name == itemName && (!requireIsGroup || child.IsGroup))
                    return child;
            }

            return null;
        }

        public TagDropdownItem CreatePath(string[] groups, int groupIndex)
        {
            bool isGroup = groupIndex < groups.Length - 1;

            var child = Find(groups[groupIndex], isGroup);
            if (child == null)
            {
                child = new TagDropdownItem(groups[groupIndex], isGroup);
                AddChild(child);
            }

            if (!isGroup)
                return child;

            return child.CreatePath(groups, ++groupIndex);
        }

        public void Order()
        {
            Children = Children.OrderBy(item => !((TagDropdownItem)item).IsGroup).ThenBy(item => item.Name).ToList();
            foreach (TagDropdownItem item in Children)
            {
                item.Order();
            }
        }
    }
}
