using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GUIInspector.NodeEditor;

namespace GUIInspector.NodeEditor
{
    [CreateAssetMenu(fileName = "New story data", menuName = "Сюжет")]
    public class StoryData : ScriptableObject
    {
        /// <summary> Описание сюжета </summary>
        public string storyDescript;

        /// <summary> Главы сюжета </summary>
        public List<GamePart> nodesData;

        /// <summary> Стиль отображения графа </summary>
        public GUISkin graphSkin;
    }
}

namespace GUIInspector
{
    [CustomEditor(typeof(StoryData))]
    public class StoryDataGUI_Inspector : Editor
    {
        private StoryData _storyData;

        private void OnEnable() => _storyData = (StoryData)target;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("Box");
            _storyData.graphSkin = (GUISkin)EditorGUILayout.ObjectField("Стиль графа : ", _storyData.graphSkin, typeof(GUISkin), true);
            EditorGUILayout.Space();
            _storyData.storyDescript = EditorGUILayout.TextArea(_storyData.storyDescript, GUILayout.Height(40));
            if (_storyData.nodesData != null) EditorGUILayout.LabelField("В сюжете учавствует : " + _storyData.nodesData.Count + " Глав");
            else EditorGUILayout.Space();

            for (int i = 0; i < _storyData.nodesData.Count; i++)
            {
                GUI.backgroundColor = Color.white;
                EditorGUILayout.BeginHorizontal("TextArea");
                EditorGUILayout.LabelField(_storyData.nodesData[i].name);
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Удалить", GUILayout.Width(70)))
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_storyData.nodesData[i]));
                    _storyData.nodesData.RemoveAt(i);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }
    }
}