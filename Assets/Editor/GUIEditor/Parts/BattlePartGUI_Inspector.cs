using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BattlePart))]
public class BattlePartGUI_Inspector : Editor
{
    private BattlePart _battlePart;
    private Vector2 _eventSlider;

    private void OnEnable() => _battlePart = (BattlePart)target;

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Текстовые поля");

        GUILayout.BeginVertical("Box");

        _battlePart.mainText = EditorGUILayout.TextArea(_battlePart.mainText, GUILayout.Height(100));
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        _battlePart.buttonText_1 = EditorGUILayout.TextArea(_battlePart.buttonText_1, GUILayout.Height(40));
        _battlePart.movePart_1 = (GamePart)EditorGUILayout.ObjectField(_battlePart.movePart_1, typeof(GamePart), true, GUILayout.Width(80));
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        _battlePart.buttonText_2 = EditorGUILayout.TextArea(_battlePart.buttonText_2, GUILayout.Height(40));
        _battlePart.movePart_2 = (GamePart)EditorGUILayout.ObjectField(_battlePart.movePart_2, typeof(GamePart), true, GUILayout.Width(80));
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        _battlePart.buttonText_3 = EditorGUILayout.TextArea(_battlePart.buttonText_3, GUILayout.Height(40));
        _battlePart.movePart_3 = (GamePart)EditorGUILayout.ObjectField(_battlePart.movePart_3, typeof(GamePart), true, GUILayout.Width(80));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        GUILayout.Label("Параметры");

        GUILayout.BeginScrollView(_eventSlider,"Box");
        if(_battlePart.mainEvents.Count > 0)
        {
            for (int i = 0; i < _battlePart.mainEvents.Count; i++)
            {
                GUILayout.BeginHorizontal();

                _battlePart.mainEvents[i] = (GameEvent)EditorGUILayout.ObjectField(_battlePart.mainEvents[i], typeof(GameEvent), true);
                if (GUILayout.Button("Удалить",GUILayout.Width(70))) _battlePart.mainEvents.RemoveAt(i);

                GUILayout.EndHorizontal();
            }
        }
        else GUILayout.Label("Нет событий");

        GUILayout.EndScrollView();

        if (GUILayout.Button("Добавить событие", GUILayout.Height(30))) _battlePart.mainEvents.Add(null);

        if (GUILayout.Button("Сохранить", GUILayout.Height(30))) _battlePart.SetDirty();
    }
}