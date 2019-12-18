﻿using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
        if (currentPart == movePart_3) return true;
        else return false;
    }
}

#if UNITY_EDITOR

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

            EditorGUILayout.BeginHorizontal("Box");
            if (_eventPart.movePart_1 != null)
            {
                EditorGUILayout.LabelField("Победа : " + _eventPart.movePart_1.name);
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Отключить", GUILayout.Width(80))) _eventPart.movePart_1 = null;
                GUI.backgroundColor = Color.white;
            }
            else EditorGUILayout.LabelField("Победа : " + "Нет подключения");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("Box");
            if (_eventPart.movePart_3 != null)
            {
                EditorGUILayout.LabelField("Провал : " + _eventPart.movePart_3.name);
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Отключить", GUILayout.Width(80))) _eventPart.movePart_3 = null;
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
}

#endif