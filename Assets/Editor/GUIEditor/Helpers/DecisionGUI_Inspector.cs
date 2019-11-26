using UnityEngine;
using UnityEditor;

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