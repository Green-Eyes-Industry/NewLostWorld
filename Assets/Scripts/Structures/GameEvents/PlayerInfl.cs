using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Data;
#endif

namespace NLW.Data
{
    public class PlayerInfl : GameEvent
    {
        /// <summary> Влияние на здоровье </summary>
        public int healthInfl;

        /// <summary> Влияние на рассудок </summary>
        public int mindInfl;

        /// <summary> Влияние </summary>
        public override bool EventStart()
        {
            Player mPlayer = MainController.instance.dataController.mainPlayer;

            mPlayer.playerHealth += healthInfl;
            mPlayer.playerMind += mindInfl;
            return true;
        }
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(PlayerInfl))]
    public class PlayerInflGInspector : Editor
    {
        private PlayerInfl _playerInfl;

        private void OnEnable() => _playerInfl = (PlayerInfl)target;

        public override void OnInspectorGUI() => ShowEventEditor(_playerInfl);

        public static void ShowEventEditor(PlayerInfl playerInfl)
        {
            GUILayout.Label("Влияние на характеристики игрока");

            EditorGUILayout.BeginVertical("Box");

            if (playerInfl.healthInfl == 0) GUI.backgroundColor = Color.white;
            else if(playerInfl.healthInfl > 0) GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.red;

            playerInfl.healthInfl = EditorGUILayout.IntSlider("Здоровье", playerInfl.healthInfl, -100, 100);

            if (playerInfl.mindInfl == 0) GUI.backgroundColor = Color.white;
            else if (playerInfl.mindInfl > 0) GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.red;

            playerInfl.mindInfl = EditorGUILayout.IntSlider("Рассудок", playerInfl.mindInfl, -100, 100);

            EditorGUILayout.EndVertical();
        }
    }
}

#endif