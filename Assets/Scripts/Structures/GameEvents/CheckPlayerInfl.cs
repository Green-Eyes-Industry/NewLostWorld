using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Data;
#endif

namespace NLW.Data
{
    [CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Проверка влияния персонажа")]
    public class CheckPlayerInfl : GameEvent
    {
        /// <summary> Персонаж </summary>
        public NonPlayer nonPlayer;

        /// <summary> Влияние </summary>
        public int value;

        /// <summary> Глава при провале </summary>
        public Parts.GamePart _failPart;

        /// <summary> Проверка соответствия влияния </summary>
        /// <returns> Вернет False при провале проверки </returns>
        public override bool EventStart()
        {
            MainController.Instance.dataController.LoadNonPlayerRatio(nonPlayer);
            if (nonPlayer.npToPlayerRatio >= value) return true;
            else return false;
        }

        /// <summary> Вернуть главу провала </summary>
        public override Parts.GamePart FailPart() { return _failPart; }
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(CheckPlayerInfl))]
    public class CheckPlayerInflGUI_Inspector : Editor
    {
        private CheckPlayerInfl _checkPlayerInfl;

        private void OnEnable() => _checkPlayerInfl = (CheckPlayerInfl)target;

        public override void OnInspectorGUI() => ShowEventEditor(_checkPlayerInfl);

        public static void ShowEventEditor(CheckPlayerInfl checkPlayerInfl)
        {
            GUILayout.Label("Проверка влияния персонажа на НПС");

            EditorGUILayout.BeginVertical("Box");

            // Код

            EditorGUILayout.EndVertical();
        }
    }
}

#endif