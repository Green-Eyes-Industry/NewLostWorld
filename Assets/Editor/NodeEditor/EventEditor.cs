using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GUIInspector.NodeEditor
{
    public class EventEditor : EditorWindow
    {
        public static EventEditor eventEditor;
        public static Event eventThis;

        /// <summary> Данные графа Эвента </summary>
        public static EventPart eventGraph;

        public enum InputEnum
        {
            ADD_NEW_EVENT_SUB_PART
        }

        public static void ShowWindow()
        {
            eventEditor.UserEventInput(eventThis);

            eventEditor.DrawEventWindows();
            GUI.backgroundColor = Color.white;
            eventEditor.DrawEventConnectors();
        }

        /// <summary> Отрисовка окон в редакторе Евентов </summary>
        private void DrawEventWindows()
        {
            if (eventGraph.eventParts == null) eventGraph.eventParts = new List<SubEventPart>();

            BeginWindows();

            if(eventGraph.eventParts.Count > 0)
            {
                for (int i = 0; i < eventGraph.eventParts.Count; i++)
                {
                    if (eventGraph.eventParts[i] == null)
                    {
                        eventGraph.eventParts.RemoveAt(i);
                        break;
                    }

                    eventGraph.eventParts[i].windowRect = GUI.Window(
                             i,
                             eventGraph.eventParts[i].windowRect,
                             DrawSubEventWindow,
                             eventGraph.eventParts[i].name, BehaviorEditor.storyData.graphSkin.GetStyle("Window"));
                }
            }

            EndWindows();
        }

        /// <summary> Отрисовка отдельной ноды </summary>
        private void DrawSubEventWindow(int id)
        {
            GUI.DragWindow();
        }

        /// <summary> Отрисовка соединений в редакторе событий </summary>
        private void DrawEventConnectors()
        {

        }

        #region USER_INPUT

        /// <summary> Ввод пользователя </summary>
        private void UserEventInput(Event e)
        {
            if (e.button == 1) { if (e.type == EventType.MouseDown) AddEventToPart(e); }
        }


        /// <summary> Добавить событие к главе </summary>
        private void AddEventToPart(Event e)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Создать главу"), false, AddNewEventSubPart, InputEnum.ADD_NEW_EVENT_SUB_PART);
            menu.ShowAsContext();
            e.Use();
        }

        private void AddNewEventSubPart(object o)
        {
            if(!AssetDatabase.IsValidFolder("Assets/Resources/GameParts/" + eventGraph.name))
            {
                AssetDatabase.CreateFolder("Assets/Resources/GameParts", eventGraph.name);
            }
            
            string nameNewEvent = Resources.LoadAll("GameParts/" + eventGraph.name).Length + "_" + eventGraph.name;

            AssetDatabase.CreateAsset(CreateInstance(typeof(SubEventPart)),
                "Assets/Resources/GameParts/" + eventGraph.name + "/" + nameNewEvent + ".asset");

            SubEventPart subPart = (SubEventPart)Resources.Load("GameParts/" + eventGraph.name + "/" + nameNewEvent, typeof(SubEventPart));

            subPart.windowRect = new Rect(
                BehaviorEditor._mousePosition.x,
                BehaviorEditor._mousePosition.y,
                BehaviorEditor.storyData.baseNodeLgWidth,
                BehaviorEditor.storyData.baseNodeLgHeight);

            eventGraph.eventParts.Add(subPart);
        }

        #endregion
    }
}