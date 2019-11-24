using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChangePart))]
public class ChangePartGUI_Inspector : Editor
{
    private ChangePart _changePart;
    private Vector2 _eventSlider = Vector2.zero;

    private void OnEnable() => _changePart = (ChangePart)target;

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Текстовые поля");

        GUILayout.BeginVertical("Box");

        _changePart.mainText = EditorGUILayout.TextArea(_changePart.mainText, GUILayout.Height(100));
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        _changePart.buttonText_1 = EditorGUILayout.TextArea(_changePart.buttonText_1, GUILayout.Height(40));
        _changePart.movePart_1 = (GamePart)EditorGUILayout.ObjectField(_changePart.movePart_1, typeof(GamePart), true, GUILayout.Width(80));
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        _changePart.buttonText_2 = EditorGUILayout.TextArea(_changePart.buttonText_2, GUILayout.Height(40));
        _changePart.movePart_2 = (GamePart)EditorGUILayout.ObjectField(_changePart.movePart_2, typeof(GamePart), true, GUILayout.Width(80));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        GUILayout.Label("Параметры");

        GUILayout.BeginScrollView(_eventSlider, "Box");
        if (_changePart.mainEvents.Count > 0)
        {
            for (int i = 0; i < _changePart.mainEvents.Count; i++)
            {
                GUILayout.BeginHorizontal();

                _changePart.mainEvents[i] = (GameEvent)EditorGUILayout.ObjectField(_changePart.mainEvents[i], typeof(GameEvent), true);
                if (GUILayout.Button("Удалить", GUILayout.Width(70))) _changePart.mainEvents.RemoveAt(i);

                GUILayout.EndHorizontal();
            }
        }
        else GUILayout.Label("Нет событий");

        GUILayout.EndScrollView();

        if (GUILayout.Button("Добавить событие", GUILayout.Height(30))) _changePart.mainEvents.Add(null);

        if (GUILayout.Button("Сохранить", GUILayout.Height(30))) _changePart.SetDirty();
    }
}