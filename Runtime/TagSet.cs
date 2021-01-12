using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bewildered.Tags
{
    [Serializable]
    public class TagSet : Core.UHashset<Tag>
    {

    }

    //[Serializable]
    //public class TagSet : IEnumerable<Tag>, ICollection<Tag>
    //{
    //    [SerializeField] private List<Tag> _tags = new List<Tag>();

    //    /// <summary>
    //    /// Gets the number of <see cref="Tag"/>s in the <see cref="TagSet"/>.
    //    /// </summary>
    //    public int Count
    //    {
    //        get { return _tags.Count; }
    //    }

    //    bool ICollection<Tag>.IsReadOnly 
    //    {
    //        get; 
    //    }

    //    public Tag this[int index] 
    //    {
    //        get { return _tags[index]; }
    //        set { _tags[index] = value; }
    //    }

    //    /// <summary>
    //    /// Searches for the specified <see cref="Tag"/> and returns the zero-based index of the first occurrence within the <see cref="TagSet"/>.
    //    /// </summary>
    //    /// <param name="tag">The <see cref="Tag"/> to locate in the <see cref="TagSet"/>.</param>
    //    /// <returns>The zero-based index of the first occurrence of <paramref name="tag"/> within the <see cref="TagSet"/>, if found; otherwise, -1.</returns>
    //    public int IndexOf(Tag tag)
    //    {
    //        return _tags.IndexOf(tag);
    //    }

    //    /// <summary>
    //    /// Adds the specified <see cref="Tag"/> to the <see cref="TagSet"/>.
    //    /// </summary>
    //    /// <param name="tag">The <see cref="Tag"/> to add.</param>
    //    /// <returns><c>true</c> if <paramref name="tag"/> is added to the <see cref="TagSet"/>; otherwise <c>false</c> if <paramref name="tag"/> is already present.</returns>
    //    public bool Add(Tag tag)
    //    {
    //        if (!_tags.Contains(tag))
    //        {
    //            _tags.Add(tag);
    //            return true;
    //        }

    //        return false;
    //    }

    //    void ICollection<Tag>.Add(Tag tag)
    //    {
    //        if (!_tags.Contains(tag))
    //        {
    //            _tags.Add(tag);
    //        }
    //    }

    //    public bool Remove(Tag tag)
    //    {
    //        return _tags.Remove(tag);
    //    }

    //    public void RemoveAt(int index)
    //    {
    //        _tags.RemoveAt(index);
    //    }

    //    public void Clear()
    //    {
    //        _tags.Clear();
    //    }

    //    public bool Contains(Tag tag)
    //    {
    //        return _tags.Contains(tag);
    //    }

    //    // Do they contain the exact same descriptors. N
    //    public bool IsIdenticalSetOf(TagSet other)
    //    {
    //        if (Count != other.Count)
    //            return false;

    //        for (int i = 0; i < Count; i++)
    //        {
    //            if (!Contains(other[i]) || !other.Contains(this[i]))
    //                return false;
    //        }

    //        return true;
    //    }

    //    /// <summary>
    //    /// Determines whether the <see cref="TagSet"/> is a superset of the specified 
    //    /// </summary>
    //    /// <param name="other"></param>
    //    /// <returns></returns>
    //    public bool IsSupersetOf(TagSet other)
    //    {
    //        foreach (var descriptor in other)
    //        {
    //            if (!Contains(descriptor))
    //                return false;
    //        }

    //        return true;
    //    }

    //    public bool IsSubsetOf(TagSet other)
    //    {
    //        foreach (var descriptor in this)
    //        {
    //            if (!other.Contains(descriptor))
    //                return false;
    //        }

    //        return true;
    //    }

    //    public void CopyTo(Tag[] array, int arrayIndex)
    //    {
    //        _tags.CopyTo(array, arrayIndex);
    //    }

    //    public IEnumerator<Tag> GetEnumerator()
    //    {
    //        return _tags.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return _tags.GetEnumerator();
    //    }
    //} 
}
