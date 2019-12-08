using UnityEngine;

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
#endif

/// <summary> Игровое событие </summary>
public class GameEvent : ScriptableObject
{
    public virtual bool EventStart() { return false; }

    #region UNITY_EDITOR

    public bool editorEventFoldout;

    #endregion
}


#if UNITY_EDITOR

namespace GUIInspector
{

    public class GlobalHelperGUI_Inspector : Editor
    {
        private static Vector2 _eventSlider = Vector2.zero;

        /// <summary> Включает дополнительный редактор главы </summary>
        public static void ShowEventEdit(GameEvent gameEvent)
        {
            EditorGUILayout.Space();

            GUILayout.BeginVertical("Button");

            if (gameEvent is CheckDecision)
            {

            }
            else if (gameEvent is CheckPlayerInfl)
            {

            }
            else if (gameEvent is CheckPoint)
            {

            }
            else if (gameEvent is EffectInteract)
            {

            }
            else if (gameEvent is ImportantDecision)
            {

            }
            else if (gameEvent is ItemInfl)
            {

            }
            else if (gameEvent is ItemInteract) ItemInteractGUI_Inspector.ShowEventEditor((ItemInteract)gameEvent);
            else if (gameEvent is LocationFind)
            {

            }
            else if (gameEvent is MemberTime)
            {

            }
            else if (gameEvent is NonPlayerInfl)
            {

            }
            else if (gameEvent is PlayerInfl)
            {

            }

            GUILayout.EndVertical();

            EditorGUILayout.Space();
        }

        /// <summary> Показать список событий </summary>
        public static void ShowPartEventList(List<GameEvent> listEvent)
        {
            GUILayout.BeginScrollView(_eventSlider, "Box");

            if (listEvent.Count > 0)
            {
                try
                {
                    for (int i = 0; i < listEvent.Count; i++)
                    {
                        GUILayout.BeginHorizontal();

                        listEvent[i] = (GameEvent)EditorGUILayout.ObjectField(listEvent[i], typeof(GameEvent), true);

                        if (listEvent[i] != null)
                        {
                            EditorGUILayout.Space();
                            listEvent[i].editorEventFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(listEvent[i].editorEventFoldout, "Подробнее");
                            EditorGUILayout.EndFoldoutHeaderGroup();
                        }

                        if (GUILayout.Button("Удалить", GUILayout.Width(70)))
                        {
                            AssetDatabase.DeleteAsset("Assets/Resources/GameEvents/" + listEvent[i].name + ".asset");
                            listEvent.RemoveAt(i);
                        }

                        GUILayout.EndHorizontal();

                        if (listEvent[i] != null)
                        {
                            if (listEvent[i].editorEventFoldout && listEvent[i] != null) ShowEventEdit(listEvent[i]);
                        }

                        if (i != listEvent.Count - 1) EditorGUILayout.Space();
                    }
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    return;
                }
            }
            else GUILayout.Label("Нет событий");

            GUILayout.EndScrollView();

            if (GUILayout.Button("Добавить событие", GUILayout.Height(30))) listEvent.Add(null);
        }

        /// <summary> Показать эффект </summary>
        public static void ShowEffectFromPart(GameEffect gameEffect)
        {
            if (gameEffect is PositiveEffect) PositiveEffectGUI_Inspector.ShowPositiveEffectGUI((PositiveEffect)gameEffect);
            else if (gameEffect is NegativeEffect) NegativeEffectGUI_Inspector.ShowNegativeEffectGUI((NegativeEffect)gameEffect);
        }
    }
}

#endif