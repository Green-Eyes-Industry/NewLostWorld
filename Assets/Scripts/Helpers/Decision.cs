using UnityEditor;
using UnityEngine;

namespace Helpers
{
    public class Decision : ScriptableObject
    {

#if UNITY_EDITOR

        /// <summary> Название решения </summary>
        public string nameDecision;

        /// <summary> Описание решения </summary>
        public string decisionDescription;

#endif

    }

#if UNITY_EDITOR

    [CustomEditor(typeof(Decision))]
    public class DecisionGUInspector : Editor
    {
        private Decision _decision;

        private void OnEnable() => _decision = (Decision)target;

        public override void OnInspectorGUI() => ShowItemEditor(_decision);

        /// <summary> Отобразить в редакторе </summary>
        public static void ShowItemEditor(Decision decision)
        {
            EditorGUILayout.LabelField("Важное решение");

            EditorGUILayout.BeginVertical("Box");
            decision.nameDecision = EditorGUILayout.TextField("Название", decision.nameDecision);
            EditorGUILayout.LabelField("Описание");
            decision.decisionDescription = EditorGUILayout.TextArea(decision.decisionDescription, GUILayout.Height(100));
            EditorGUILayout.EndVertical();
        }
    }

#endif
}