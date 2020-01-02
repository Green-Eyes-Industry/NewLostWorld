using Data.Characters;
using UnityEditor;
using UnityEngine;

namespace Data.GameEvents
{
    [CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Влияние на НПС")]
    public class NonPlayerInfl : GameEvent
    {
        /// <summary> На какого персонажа </summary>
        public NonPlayer nonPlayer;

        /// <summary> Влияние </summary>
        public int value;

        /// <summary> Старт события </summary>
        public override bool EventStart()
        {
            nonPlayer.npToPlayerRatio += value;

            if (!MainController.instance.dataController.mainPlayer.playerMeet.Contains(nonPlayer))
            {
                MainController.instance.dataController.mainPlayer.playerMeet.Add(nonPlayer);
            }
            return false;
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(NonPlayerInfl))]
    public class NonPlayerInflGInspector : Editor
    {
        private NonPlayerInfl _nonPlayerInfl;

        private void OnEnable() => _nonPlayerInfl = (NonPlayerInfl)target;

        public override void OnInspectorGUI() => ShowEventEditor(_nonPlayerInfl);

        public static void ShowEventEditor(NonPlayerInfl nonPlayerInfl)
        {
            GUILayout.Label("Влияние на НПС");

            EditorGUILayout.BeginVertical("Box");

            // TODO : Код

            EditorGUILayout.EndVertical();
        }
    }

#endif
}