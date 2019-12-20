using UnityEngine;

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using NLW.Data;
#endif

namespace NLW.Data
{
    /// <summary> Игровое событие </summary>
    public abstract class GameEvent : ScriptableObject
    {
        /// <summary> Старт события </summary>
        public virtual bool EventStart() { return true; }

        /// <summary> Глава при провале </summary>
        public virtual Parts.GamePart FailPart() { return null; }

        #region UNITY_EDITOR

        public bool editorEventFoldout;

        #endregion
    }
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
            GUI.backgroundColor = Color.white;
            if (gameEvent is CheckDecision) CheckDecisionGUI_Inspector.ShowEventEditor((CheckDecision)gameEvent);
            else if (gameEvent is CheckPlayerInfl) CheckPlayerInflGUI_Inspector.ShowEventEditor((CheckPlayerInfl)gameEvent);
            else if (gameEvent is CheckPoint) CheckPointGUI_Inspector.ShowEventEditor((CheckPoint)gameEvent);
            else if (gameEvent is EffectInteract) EffectInteractGUI_Inspector.ShowEventEditor((EffectInteract)gameEvent);
            else if (gameEvent is ImportantDecision) ImportantDecisionGUI_Inspector.ShowEventEditor((ImportantDecision)gameEvent);
            else if (gameEvent is ItemInfl) ItemInflGUI_Inspector.ShowEventEditor((ItemInfl)gameEvent);
            else if (gameEvent is ItemInteract) ItemInteractGUI_Inspector.ShowEventEditor((ItemInteract)gameEvent);
            else if (gameEvent is LocationFind) LocationFindGUI_Inspector.ShowEventEditor((LocationFind)gameEvent);
            else if (gameEvent is MemberTime) MemberTimeGUI_Inspector.ShowEventEditor((MemberTime)gameEvent);
            else if (gameEvent is NonPlayerInfl) NonPlayerInflGUI_Inspector.ShowEventEditor((NonPlayerInfl)gameEvent);
            else if (gameEvent is PlayerInfl) PlayerInflGUI_Inspector.ShowEventEditor((PlayerInfl)gameEvent);
            else if(gameEvent is RandomPart) RandomPartGUI_Inspector.ShowEventEditor((RandomPart)gameEvent);
        }

        /// <summary> Показать список событий </summary>
        public static void ShowPartEventList(List<GameEvent> listEvent)
        {
            _eventSlider = GUILayout.BeginScrollView(_eventSlider, "Box");

            if (listEvent.Count > 0)
            {
                for (int i = 0; i < listEvent.Count; i++)
                {
                    GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f);

                    GUILayout.BeginHorizontal("Box");

                    listEvent[i] = (GameEvent)EditorGUILayout.ObjectField(listEvent[i], typeof(GameEvent), true);

                    if (listEvent[i] != null)
                    {
                        EditorGUILayout.Space();
                        listEvent[i].editorEventFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(listEvent[i].editorEventFoldout, "Подробнее");
                        EditorGUILayout.EndFoldoutHeaderGroup();
                    }

                    GUI.backgroundColor = Color.red;
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
                }
            }
            else GUILayout.Label("Нет событий");

            GUILayout.EndScrollView();
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