using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Decision : ScriptableObject
{
#if UNITY_EDITOR

    /// <summary> Название решения </summary>
    public string _nameDecision;

    /// <summary> Описание решения </summary>
    public string _decisionDescription;

    #endif
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(Decision))]
    public class DecisionGUI_Inspector : Editor
    {
        private Decision _decision;

        private void OnEnable() => _decision = (Decision)target;

        public override void OnInspectorGUI() => ShowItemEditor(_decision);

        /// <summary> Отобразить в редакторе </summary>
        public static void ShowItemEditor(Decision decision)
        {
            EditorGUILayout.LabelField("Важное решение");

            EditorGUILayout.BeginVertical("Box");
            decision._nameDecision = EditorGUILayout.TextField("Название", decision._nameDecision);
            EditorGUILayout.LabelField("Описание");
            decision._decisionDescription = EditorGUILayout.TextArea(decision._decisionDescription, GUILayout.Height(100));
            EditorGUILayout.EndVertical();
        }
    }
}

#endif