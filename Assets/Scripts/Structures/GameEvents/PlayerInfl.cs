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
        public int _healthInfl;

        /// <summary> Влияние на рассудок </summary>
        public int _mindInfl;

        /// <summary> Влияние </summary>
        public override bool EventStart()
        {
            Player mPlayer = MainController.Instance.dataController.mainPlayer;

            mPlayer.playerHealth += _healthInfl;
            mPlayer.playerMind += _mindInfl;
            return true;
        }
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(PlayerInfl))]
    public class PlayerInflGUI_Inspector : Editor
    {
        private PlayerInfl _playerInfl;

        private void OnEnable() => _playerInfl = (PlayerInfl)target;

        public override void OnInspectorGUI() => ShowEventEditor(_playerInfl);

        public static void ShowEventEditor(PlayerInfl _playerInfl)
        {
            GUILayout.Label("Влияние на характеристики игрока");

            EditorGUILayout.BeginVertical("Box");

            if (_playerInfl._healthInfl == 0) GUI.backgroundColor = Color.white;
            else if(_playerInfl._healthInfl > 0) GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.red;

            _playerInfl._healthInfl = EditorGUILayout.IntSlider("Здоровье", _playerInfl._healthInfl, -100, 100);

            if (_playerInfl._mindInfl == 0) GUI.backgroundColor = Color.white;
            else if (_playerInfl._mindInfl > 0) GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.red;

            _playerInfl._mindInfl = EditorGUILayout.IntSlider("Рассудок", _playerInfl._mindInfl, -100, 100);

            EditorGUILayout.EndVertical();
        }
    }
}

#endif