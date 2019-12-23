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
    public class GlobalHelperGInspector : Editor
    {
        private static Vector2 _eventSlider = Vector2.zero;

        /// <summary> Включает дополнительный редактор главы </summary>
        private static void ShowEventEdit(GameEvent gameEvent)
        {
            GUI.backgroundColor = Color.white;
            switch (gameEvent)
            {
                case CheckDecision cDecision: CheckDecisionGInspector.ShowEventEditor(cDecision); break;
                case CheckPlayerInfl cPlayer: CheckPlayerInflGInspector.ShowEventEditor(cPlayer); break;
                case CheckPoint cPoint: CheckPointGInspector.ShowEventEditor(cPoint); break;
                case EffectInteract effectInter: EffectInteractGInspector.ShowEventEditor(effectInter); break;
                case ImportantDecision importantDec: ImportantDecisionGInspector.ShowEventEditor(importantDec); break;
                case ItemInfl itemInfl: ItemInflGInspector.ShowEventEditor(itemInfl); break;
                case ItemInteract itemInter: ItemInteractGInspector.ShowEventEditor(itemInter); break;
                case LocationFind locationF: LocationFindGInspector.ShowEventEditor(locationF); break;
                case MemberTime memberT: MemberTimeGInspector.ShowEventEditor(memberT); break;
                case NonPlayerInfl nonPlInfl: NonPlayerInflGInspector.ShowEventEditor(nonPlInfl); break;
                case PlayerInfl plInfl: PlayerInflGInspector.ShowEventEditor(plInfl); break;
                case RandomPart randomP: RandomPartGInspector.ShowEventEditor(randomP); break;
            }
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

                    if (listEvent[i] == null) continue;
                    if (listEvent[i].editorEventFoldout && listEvent[i] != null) ShowEventEdit(listEvent[i]);
                }
            }
            else GUILayout.Label("Нет событий");

            GUILayout.EndScrollView();
        }

        /// <summary> Показать эффект </summary>
        public static void ShowEffectFromPart(GameEffect gameEffect)
        {
            switch (gameEffect)
            {
                case PositiveEffect pEffect: PositiveEffectGInspector.ShowPositiveEffectGUI(pEffect); break;
                case NegativeEffect nEffect: NegativeEffectGInspector.ShowNegativeEffectGUI(nEffect); break;
            }
        }
    }
}

#endif