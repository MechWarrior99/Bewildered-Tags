using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Bewildered.Tags.Editor
{
    [CustomEditor(typeof(ObjectTags))]
    public class ObjectDefinitionInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_tags"));
        }
    } 
}
