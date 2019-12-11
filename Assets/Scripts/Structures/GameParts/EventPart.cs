using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New part", menuName = "Игровые обьекты/Новая глава/Глава Эвента", order = 4)]
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

            _eventPart.movePart_1 = (GamePart)EditorGUILayout.ObjectField("Глава при победе :", _eventPart.movePart_1, typeof(GamePart), true);
            _eventPart.movePart_3 = (GamePart)EditorGUILayout.ObjectField("Глава при провале :", _eventPart.movePart_3, typeof(GamePart), true);
            EditorGUILayout.Space();

            _eventPart.timeToEvent = EditorGUILayout.IntSlider("Время на выполнение: ", _eventPart.timeToEvent, 0, 120);
            EditorGUILayout.Space();

            GUILayout.BeginScrollView(_partsSlider, "Box");

            if (_eventPart.eventParts != null)
            {
                if (_eventPart.eventParts.Count > 0)
                {
                    for (int i = 0; i < _eventPart.eventParts.Count; i++)
                    {
                        GUILayout.BeginHorizontal();

                        _eventPart.eventParts[i] = (SubEventPart)EditorGUILayout.ObjectField(_eventPart.eventParts[i], typeof(SubEventPart), true);
                        if (GUILayout.Button("Удалить", GUILayout.Width(70))) _eventPart.eventParts.RemoveAt(i);

                        GUILayout.EndHorizontal();
                    }
                }
                else GUILayout.Label("Нет глав");
            }
            else _eventPart.eventParts = new List<SubEventPart>();

            GUILayout.EndScrollView();

            if (GUILayout.Button("Добавить главу", GUILayout.Height(30))) _eventPart.eventParts.Add(null);
        }
    }
}

#endif