using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChangePart))]
public class ChangePartGUI_Inspector : Editor
{
    private ChangePart _changePart;

    private void OnEnable() => _changePart = (ChangePart)target;

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical("Box");

        _changePart.mainText = EditorGUILayout.TextArea(_changePart.mainText, GUILayout.Height(100));

        GUILayout.BeginHorizontal();
        _changePart.buttonText_1 = EditorGUILayout.TextArea(_changePart.buttonText_1, GUILayout.Height(40));
        _changePart.movePart_1 = (GamePart)EditorGUILayout.ObjectField(_changePart.movePart_1, typeof(GamePart), true, GUILayout.Width(80));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        _changePart.buttonText_2 = EditorGUILayout.TextArea(_changePart.buttonText_2, GUILayout.Height(40));
        _changePart.movePart_2 = (GamePart)EditorGUILayout.ObjectField(_changePart.movePart_2, typeof(GamePart), true, GUILayout.Width(80));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}