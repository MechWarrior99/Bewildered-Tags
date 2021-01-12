using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bewildered.Tags
{
    [AddComponentMenu("Systems/Object Tags")]
    public class ObjectTags : MonoBehaviour
    {
        [SerializeField] private TagSet _tags = new TagSet();

        public TagSet Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }
    } 
}
