using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Bewildered.Editor;

namespace Bewildered.Tags.Editor
{
    public static class TagEditorUtility 
    {

        /// <summary>
        /// The asset path for the <see cref="TagsManager"/> instance.
        /// </summary>
        // If ever changed, the end asset must be within the Resources folder.
        public const string AssetPath = "Assets/Resources/" + nameof(TagsManager) + ".asset";

        /// <summary>
        /// Create a new <see cref="Tag"/> with a provided title as long as a <see cref="Tag"/> with the same name does not already exist.
        /// </summary>
        /// <param name="title">The title/name of the <see cref="Tag"/> to create.</param>
        /// <returns><c>false</c> if a <see cref="Tag"/> with the same <see cref="Tag.Path"/> already exists.</returns>
        public static bool CreateTag(string title)
        {
            //if (!AssetDatabaseUtility.IsValidPath(title))
            //    return false;

            SerializedObject tagManager = new SerializedObject(TagsManager.Instance);

            SerializedProperty descriptorsProperty = tagManager.FindProperty("_tags");

            // Check if a tag with the same name already exists. If so return false.
            for (int i = 0; i < descriptorsProperty.arraySize; i++)
            {
                Tag descriptor = descriptorsProperty.GetArrayElementAtIndex(i).objectReferenceValue as Tag;

                if (string.Equals(descriptor.Path, title))
                    return false;
            }

            Undo.IncrementCurrentGroup();

            // Create a new tag.
            Tag newDescriptor = ScriptableObject.CreateInstance<Tag>();
            newDescriptor.hideFlags = HideFlags.HideInHierarchy;
            newDescriptor.name = title.Split('/').Last();
            newDescriptor.Path = title;
            // Save the new desscriptor as a child of the DescriptorManager in the project.
            AssetDatabase.AddObjectToAsset(newDescriptor, AssetPath);
            AssetDatabase.ImportAsset(AssetPath);

            // Add the newly created tag to the list of tags in the TagManager so it can be accessed through it.
            descriptorsProperty.InsertArrayElementAtIndex(descriptorsProperty.arraySize);
            descriptorsProperty.GetArrayElementAtIndex(descriptorsProperty.arraySize - 1).objectReferenceValue = newDescriptor;
            descriptorsProperty.serializedObject.ApplyModifiedProperties();
            TagsManager.Tags.Sort();

            Undo.RegisterCreatedObjectUndo(newDescriptor, "Tag created");
            Undo.IncrementCurrentGroup();

            return true;
        }

        public static void RemoveTag(Tag descriptor)
        {
            int index = TagsManager.Tags.IndexOf(descriptor);
            RemoveTag(index);
        }

        /// <summary>
        /// Remove and destroy a <see cref="Tag"/> from the <see cref="TagsManager"/> at a specific index.
        /// </summary>
        /// <param name="index">The index of the <see cref="Tag"/> to destroy.</param>
        public static void RemoveTag(int index)
        {
            SerializedObject descriptorManager = new SerializedObject(TagsManager.Instance);
            SerializedProperty descriptorsProperty = descriptorManager.FindProperty("_tags");

            RemoveTag(descriptorsProperty, index);
        }

        public static void RemoveTag(SerializedProperty descriptorsProperty, int index)
        {
            Undo.SetCurrentGroupName("Remove and destroy tag.");
            int undoGroupIndex = Undo.GetCurrentGroup();
            Object obj = descriptorsProperty.GetArrayElementAtIndex(index).objectReferenceValue;

            // Using DeleteArrayElementAtIndex on a UnityEngine.Object field will only set it to null if it is not already instead of deleting the item.
            // So the item must be set to null first inorder for it to actually delete the item entry.
            descriptorsProperty.GetArrayElementAtIndex(index).objectReferenceValue = null;
            descriptorsProperty.DeleteArrayElementAtIndex(index);

            Undo.DestroyObjectImmediate(obj);

            Undo.CollapseUndoOperations(undoGroupIndex);
        }

        public static void SyncListWithAssets()
        {
            SerializedObject descriptorManager = new SerializedObject(TagsManager.Instance);
            SerializedProperty descriptorsProperty = descriptorManager.FindProperty("_tags");

            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(AssetPath);
            for (int i = 0; i < assets.Length; i++)
            {
                if (!TagsManager.Tags.Contains(assets[i]))
                {
                    descriptorsProperty.AddAndGetArrayElement().objectReferenceValue = assets[i];
                }
            }

            descriptorManager.ApplyModifiedProperties();
        }

        /// <summary>
        /// Creates a <see cref="TagManager"/> asset if one does not already exist.
        /// </summary>
        [InitializeOnLoadMethod]
        public static void CreateTagManagerAsNeeded()
        {
            if (!TagsManager.HasInstance())
            {
                TagsManager tagManagerInstance = ScriptableObject.CreateInstance<TagsManager>();

                AssetDatabaseUtility.CreateFoldersAsNeeded(AssetPath);

                AssetDatabase.CreateAsset(tagManagerInstance, AssetPath);
                AssetDatabase.ImportAsset(AssetPath);
                AssetDatabase.SaveAssets();
                Debug.Log("Created new TagsManager asset at path: " + AssetPath, tagManagerInstance);
            }
        }
    } 
}
