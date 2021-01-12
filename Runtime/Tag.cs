using System;
using UnityEngine;

namespace Bewildered.Tags
{
    /// <summary>
    /// 
    /// </summary>
    public class Tag : ScriptableObject, IComparable<Tag>
    {
        [SerializeField] private string _path = "";

        /// <summary>
        /// The grouping path of the <see cref="Tag"/>.
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// The name of the <see cref="Tag"/>.
        /// </summary>
        public string Title
        {
            get { return _path.Substring(_path.LastIndexOf("/") + 1); }
        }

        public int CompareTo(Tag compareDescriptor)
        {
            // A null value means that this object is greater
            if (compareDescriptor == null)
                return 1;
            else
                return Path.CompareTo(compareDescriptor.Path);
        }
    } 
}
