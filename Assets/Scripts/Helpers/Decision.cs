using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New decision", menuName = "Игровые обьекты/Решение")]
public class Decision : ScriptableObject
{
    #if UNITY_EDITOR

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

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Описание решения");
            _decision._decisionDescription = EditorGUILayout.TextArea(_decision._decisionDescription, GUILayout.Height(100));
        }
    }
}

#endif