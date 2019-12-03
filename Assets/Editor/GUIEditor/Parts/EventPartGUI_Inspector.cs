using UnityEngine;
using UnityEditor;

namespace GUIInspector
{

    [CustomEditor(typeof(EventPart))]
    public class EventPartGUI_Inspector : Editor
    {
        private EventPart _eventPart;
        private Vector2 _partsSlider = Vector2.zero;

        private void OnEnable() => _eventPart = (EventPart)target;

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Параметры");

            _eventPart.failPart = (GamePart)EditorGUILayout.ObjectField("Глава при провале :", _eventPart.failPart, typeof(GamePart), true);
            _eventPart.finalPart = (GamePart)EditorGUILayout.ObjectField("Глава при победе :", _eventPart.finalPart, typeof(GamePart), true);
            EditorGUILayout.Space();

            _eventPart.timeToEvent = EditorGUILayout.IntSlider("Время на выполнение: ", _eventPart.timeToEvent, 0, 120);
            EditorGUILayout.Space();

            GUILayout.BeginScrollView(_partsSlider, "Box");

            if (_eventPart.eventParts.Count > 0)
            {
                for (int i = 0; i < _eventPart.eventParts.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    _eventPart.eventParts[i] = (GamePart)EditorGUILayout.ObjectField(_eventPart.eventParts[i], typeof(GamePart), true);
                    if (GUILayout.Button("Удалить", GUILayout.Width(70))) _eventPart.eventParts.RemoveAt(i);

                    GUILayout.EndHorizontal();
                }
            }
            else GUILayout.Label("Нет глав");

            GUILayout.EndScrollView();

            if (GUILayout.Button("Добавить главу", GUILayout.Height(30))) _eventPart.eventParts.Add(null);
        }
    }
}