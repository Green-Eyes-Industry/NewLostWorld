using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Data.GameParts
{
    public class EventPart : GamePart
    {
        /// <summary> Время на эвент </summary>
        public int timeToEvent;

        /// <summary> Список глав эвента </summary>
        public List<SubEventPart> eventParts;

        /// <summary> Проверка главы </summary>
        /// <param name="currentPart"> Текущая глава </param>
        /// <returns> True если вы добрались к последней главе </returns>
        public bool CheckEvent(GamePart currentPart)
        {
            return currentPart == movePart[2];
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(EventPart))]
    public class EventPartGUInspector : Editor
    {
        private EventPart _eventPart;
        private Vector2 _partsSlider = Vector2.zero;

        private void OnEnable() => _eventPart = (EventPart)target;

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Параметры");

            EditorGUILayout.BeginHorizontal("Box");
            if (_eventPart.movePart[0] != null)
            {
                EditorGUILayout.LabelField("Победа : " + _eventPart.movePart[0].name);
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Отключить", GUILayout.Width(80))) _eventPart.movePart[0] = null;
                GUI.backgroundColor = Color.white;
            }
            else EditorGUILayout.LabelField("Победа : " + "Нет подключения");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("Box");
            if (_eventPart.movePart[2] != null)
            {
                EditorGUILayout.LabelField("Провал : " + _eventPart.movePart[2].name);
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Отключить", GUILayout.Width(80))) _eventPart.movePart[2] = null;
                GUI.backgroundColor = Color.white;
            }
            else EditorGUILayout.LabelField("Провал : " + "Нет подключения");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            _eventPart.timeToEvent = EditorGUILayout.IntSlider("Время (сек): ", _eventPart.timeToEvent, 20, 360);
            EditorGUILayout.Space();

            GUILayout.BeginScrollView(_partsSlider, "Box");

            if (_eventPart.eventParts != null)
            {
                if (_eventPart.eventParts.Count > 0)
                {
                    for (int i = 0; i < _eventPart.eventParts.Count; i++)
                    {
                        GUILayout.BeginHorizontal();

                        if (_eventPart.eventParts[i].isFail) GUI.backgroundColor = Color.red;
                        else if (_eventPart.eventParts[i].isFinal) GUI.backgroundColor = Color.green;
                        else if (i == 0) GUI.backgroundColor = Color.yellow;
                        else GUI.backgroundColor = Color.white;

                        EditorGUILayout.BeginHorizontal("Button");
                        if (_eventPart.eventParts[i].comment == null)
                            EditorGUILayout.LabelField(_eventPart.eventParts[i].name);
                        else EditorGUILayout.LabelField(_eventPart.eventParts[i].comment);
                        EditorGUILayout.EndHorizontal();

                        GUILayout.EndHorizontal();
                    }
                }
                else GUILayout.Label("Нет глав");
            }
            else _eventPart.eventParts = new List<SubEventPart>();

            GUILayout.EndScrollView();
        }
    }

#endif
}