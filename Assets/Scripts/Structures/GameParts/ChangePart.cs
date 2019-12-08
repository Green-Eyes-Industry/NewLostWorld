using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New part", menuName = "Игровые обьекты/Новая глава/Глава выбора", order = 1)]
public class ChangePart : GamePart
{
    /// <summary> Текст первой кнопки </summary>
    public string buttonText_1;

    /// <summary> Текст второй кнопки </summary>
    public string buttonText_2;
}

#if UNITY_EDITOR

namespace GUIInspector
{

    [CustomEditor(typeof(ChangePart))]
    public class ChangePartGUI_Inspector : Editor
    {
        private ChangePart _changePart;

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

            if (_changePart.mainEvents != null) GlobalHelperGUI_Inspector.ShowPartEventList(_changePart.mainEvents);
            else _changePart.mainEvents = new System.Collections.Generic.List<GameEvent>();
        }
    }
}

#endif