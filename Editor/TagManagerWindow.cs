using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Bewildered.Tags.Editor
{
    public class TagManagerWindow : EditorWindow
    {
        private SerializedProperty _tagsProperty;
        private TagTreeView _treeView;
        private GUIContent _showFullTreeContent;
        [SerializeField] private TreeViewState _treeState;
        
        [MenuItem("Tools/Tag Manager")]
        private static void Open()
        {
            TagManagerWindow window = GetWindow<TagManagerWindow>();
            window.titleContent = new GUIContent("Tag Manager", EditorGUIUtility.IconContent("d_FilterByLabel").image);
            window.Show();
        }

        private void OnEnable()
        {
            if (_treeState == null)
                _treeState = new TreeViewState();

            TagEditorUtility.CreateTagManagerAsNeeded();
            TagEditorUtility.SyncListWithAssets();
            _tagsProperty = new SerializedObject(TagsManager.Instance).FindProperty("_tags");
            _treeView = new TagTreeView(_tagsProperty, _treeState);
            _treeView.Reload();

            _showFullTreeContent = EditorGUIUtility.IconContent("UnityEditor.SceneHierarchyWindow");
        }

        private void OnGUI()
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                string focusedControlName = GUI.GetNameOfFocusedControl();
                if (focusedControlName == "TagsField")
                {
                    TagEditorUtility.CreateTag(_treeView.searchString);
                    _tagsProperty = new SerializedObject(TagsManager.Instance).FindProperty("_tags");
                    _treeView._tagsProperty = _tagsProperty;
                    _treeView.searchString = "";
                    _treeView.Reload();
                    Repaint();
                }
            }

            using (new GUILayout.HorizontalScope("Toolbar"))
            {
                GUI.SetNextControlName("TagsField");
                _treeView.searchString = EditorGUILayout.TextField("", _treeView.searchString, "ToolbarTextField", GUILayout.ExpandWidth(true));

                if (GUILayout.Button(_showFullTreeContent.image, "toolbarbutton", GUILayout.ExpandWidth(false)))
                {
                    _treeView.showAsList = !_treeView.showAsList;
                    _treeView.Reload();
                }
            }

            Rect treeRect = GUILayoutUtility.GetRect(Screen.width, Screen.height - EditorGUIUtility.singleLineHeight);

            _treeView.OnGUI(treeRect);

            Repaint();
        }
    } 
}
