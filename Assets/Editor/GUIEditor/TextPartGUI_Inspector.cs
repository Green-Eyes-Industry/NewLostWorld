using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextPart))]
public class TextPartGUI_Inspector : Editor
{
    private TextPart _textPart;

    private void OnEnable() => _textPart = (TextPart)target;

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical("Box");

        _textPart.mainText = EditorGUILayout.TextArea(_textPart.mainText, GUILayout.Height(100));

        GUILayout.BeginHorizontal();
        _textPart.buttonText_1 = EditorGUILayout.TextArea(_textPart.buttonText_1, GUILayout.Height(40));
        _textPart.movePart_1 = (GamePart)EditorGUILayout.ObjectField(_textPart.movePart_1, typeof(GamePart), true, GUILayout.Width(80));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}
