﻿using System.Collections.Generic;
using UnityEngine;
using Bewildered.Core;

namespace Bewildered.Tags
{
    public class TagsManager : SingletonScriptableObject<TagsManager>
    {
        [SerializeField] private List<Tag> _tags = new List<Tag>();

        public static List<Tag> Tags
        {
            get { return Instance._tags; }
        }
    } 
}
