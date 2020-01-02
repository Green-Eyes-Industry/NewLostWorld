using Data.Characters;
using UnityEditor;
using UnityEngine;

namespace Data.GameEvents
{
    public class CheckPlayerInfl : GameEvent
    {
        /// <summary> Non Player </summary>
        public NonPlayer nonPlayer;

        /// <summary> Inflation </summary>
        public int value;

        /// <summary> Fail part </summary>
        public GamePart failPart;

        /// <summary> Check inflation </summary>
        /// <returns> Fail part </returns>
        public override bool EventStart()
        {
            return nonPlayer.npToPlayerRatio >= value;
        }

        /// <summary> Return false part </summary>
        public override GamePart FailPart() { return failPart; }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(CheckPlayerInfl))]
    public class CheckPlayerInflGInspector : Editor
    {
        private CheckPlayerInfl _checkPlayerInfl;

        private void OnEnable() => _checkPlayerInfl = (CheckPlayerInfl)target;

        public override void OnInspectorGUI() => ShowEventEditor(_checkPlayerInfl);

        public static void ShowEventEditor(CheckPlayerInfl checkPlayerInfl)
        {
            GUILayout.Label("Проверка влияния персонажа на НПС");

            EditorGUILayout.BeginVertical("Box");

            // TODO : Код

            EditorGUILayout.EndVertical();
        }
    }

#endif
}