using System.Collections.Generic;
using Data.GameEffects;
using Data.GameEvents;
using UnityEngine;
using UnityEditor;

namespace Data
{
    /// <summary> Игровое событие </summary>
    public abstract class GameEvent : ScriptableObject
    {
        /// <summary> Старт события </summary>
        public virtual bool EventStart() { return true; }

        /// <summary> Глава при провале </summary>
        public virtual GamePart FailPart() { return null; }

        #region UNITY_EDITOR

        public bool editorEventFoldout;

        #endregion
    }

#if UNITY_EDITOR

    public class GlobalHelperGUInspector : Editor
    {
        private static Vector2 _eventSlider = Vector2.zero;

        /// <summary> Включает дополнительный редактор главы </summary>
        private static void ShowEventEdit(GameEvent gameEvent)
        {
            GUI.backgroundColor = Color.white;
            switch (gameEvent)
            {
                case CheckDecision cDecision: CheckDecisionGUInspector.ShowEventEditor(cDecision); break;
                case CheckPlayerInfl cPlayer: CheckPlayerInflGUInspector.ShowEventEditor(cPlayer); break;
                case CheckPoint cPoint: CheckPointGUInspector.ShowEventEditor(cPoint); break;
                case EffectInteract effectInter: EffectInteractGUInspector.ShowEventEditor(effectInter); break;
                case ImportantDecision importantDec: ImportantDecisionGUInspector.ShowEventEditor(importantDec); break;
                case ItemInfl itemInfl: ItemInflGUInspector.ShowEventEditor(itemInfl); break;
                case ItemInteract itemInter: ItemInteractGUInspector.ShowEventEditor(itemInter); break;
                case LocationFind locationF: LocationFindGUInspector.ShowEventEditor(locationF); break;
                case MemberTime memberT: MemberTimeGUInspector.ShowEventEditor(memberT); break;
                case NonPlayerInfl nonPlInfl: NonPlayerInflGUInspector.ShowEventEditor(nonPlInfl); break;
                case PlayerInfl plInfl: PlayerInflGUInspector.ShowEventEditor(plInfl); break;
                case RandomPart randomP: RandomPartGUInspector.ShowEventEditor(randomP); break;
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
                    if (listEvent[i] == null)
                    {
                        listEvent.RemoveAt(i);
                        return;
                    }

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
                case PositiveEffect pEffect: PositiveEffectGUInspector.ShowPositiveEffectGUI(pEffect); break;
                case NegativeEffect nEffect: NegativeEffectGUInspector.ShowNegativeEffectGUI(nEffect); break;
            }
        }
    }

#endif
}