using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextPart))]
public class TextPartGUI_Inspector : Editor
{
    private TextPart _textPart;

    private void OnEnable() => _textPart = (TextPart)target;

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Текстовые поля");

        GUILayout.BeginVertical("Box");

        _textPart.mainText = EditorGUILayout.TextArea(_textPart.mainText, GUILayout.Height(100));
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        _textPart.buttonText_1 = EditorGUILayout.TextArea(_textPart.buttonText_1, GUILayout.Height(40));
        _textPart.movePart_1 = (GamePart)EditorGUILayout.ObjectField(_textPart.movePart_1, typeof(GamePart), true, GUILayout.Width(80));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        GUILayout.Label("Параметры");

        if (_textPart.mainEvents != null) GlobalHelperGUI_Inspector.ShowPartEventList(_textPart.mainEvents);
        else _textPart.mainEvents = new System.Collections.Generic.List<GameEvent>();

        if (GUILayout.Button("Сохранить", GUILayout.Height(30))) EditorUtility.SetDirty(_textPart);
    }
}