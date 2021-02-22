using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Bewildered.Tags.Editor
{
    [CustomEditor(typeof(ObjectTags))]
    public class ObjectTagsInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_tags"));
        }
    } 
}
