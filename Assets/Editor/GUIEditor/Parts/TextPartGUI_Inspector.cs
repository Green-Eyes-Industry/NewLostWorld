using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextPart))]
public class TextPartGUI_Inspector : Editor
{
    private TextPart _textPart;
    private Vector2 _eventSlider = Vector2.zero;

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

        GUILayout.BeginScrollView(_eventSlider, "Box");
        if (_textPart.mainEvents.Count > 0)
        {
            for (int i = 0; i < _textPart.mainEvents.Count; i++)
            {
                GUILayout.BeginHorizontal();

                _textPart.mainEvents[i] = (GameEvent)EditorGUILayout.ObjectField(_textPart.mainEvents[i], typeof(GameEvent), true);
                if (GUILayout.Button("Удалить", GUILayout.Width(70))) _textPart.mainEvents.RemoveAt(i);

                GUILayout.EndHorizontal();
            }
        }
        else GUILayout.Label("Нет событий");

        GUILayout.EndScrollView();

        if (GUILayout.Button("Добавить событие", GUILayout.Height(30))) _textPart.mainEvents.Add(null);

        if (GUILayout.Button("Сохранить", GUILayout.Height(30))) _textPart.SetDirty();
    }
}