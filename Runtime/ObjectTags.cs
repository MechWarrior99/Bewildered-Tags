using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bewildered.Tags
{
    [AddComponentMenu("Systems/Object Tags")]
    public class ObjectTags : MonoBehaviour, ISet<Tag>, ICollection<Tag>, IEnumerable<Tag>
    {
        [SerializeField] private TagSet _tags = new TagSet();

        public int Count => _tags.Count;

        bool ICollection<Tag>.IsReadOnly
        {
            get { return false; }
        }
        
        public bool Add(Tag item)
        {
            return _tags.Add(item);
        }

        public void Clear()
        {
            _tags.Clear();
        }

        public bool Contains(Tag item)
        {
            return _tags.Contains(item);
        }

        public void CopyTo(Tag[] array, int arrayIndex)
        {
           _tags.CopyTo(array, arrayIndex);
        }

        public void ExceptWith(IEnumerable<Tag> other)
        {
            _tags.ExceptWith(other);
        }

        public IEnumerator<Tag> GetEnumerator()
        {
            return _tags.GetEnumerator();
        }

        public void IntersectWith(IEnumerable<Tag> other)
        {
            _tags.IntersectWith(other);
        }

        public bool IsProperSubsetOf(IEnumerable<Tag> other)
        {
            return _tags.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<Tag> other)
        {
            return _tags.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<Tag> other)
        {
            return _tags.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<Tag> other)
        {
            return _tags.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<Tag> other)
        {
            return _tags.Overlaps(other);
        }

        public bool Remove(Tag item)
        {
            return _tags.Remove(item);
        }

        public bool SetEquals(IEnumerable<Tag> other)
        {
            return _tags.SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<Tag> other)
        {
            _tags.SymmetricExceptWith(other);
        }

        public void UnionWith(IEnumerable<Tag> other)
        {
            _tags.UnionWith(other);
        }

        void ICollection<Tag>.Add(Tag item)
        {
            _tags.Add(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_tags).GetEnumerator();
        }
    } 
}
