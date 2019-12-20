using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Data;
#endif

namespace NLW.Data
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
            MainController.Instance.dataController.SaveNonPlayerRatio(nonPlayer);
            return false;
        }
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(NonPlayerInfl))]
    public class NonPlayerInflGUI_Inspector : Editor
    {
        private NonPlayerInfl _nonPlayerInfl;

        private void OnEnable() => _nonPlayerInfl = (NonPlayerInfl)target;

        public override void OnInspectorGUI() => ShowEventEditor(_nonPlayerInfl);

        public static void ShowEventEditor(NonPlayerInfl nonPlayerInfl)
        {
            GUILayout.Label("Влияние на НПС");

            EditorGUILayout.BeginVertical("Box");

            // Код

            EditorGUILayout.EndVertical();
        }
    }
}

#endif