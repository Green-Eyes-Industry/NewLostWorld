using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Data.GameEvents
{
    public class PlayerMindCheck : GameEvent
    {
        /// <summary> Минимальный уровень здоровья </summary>
        public int minMind;

        /// <summary> Максимальный уровень здоровья </summary>
        public int maxMind;

        /// <summary> Глава при проверке </summary>
        public GamePart failPart;

        /// <summary> Проверка на соответствие здоровья </summary>
        /// <returns> Вернет False при провале проверки </returns>
        public override bool EventStart()
        {
            return (MainController.instance.dataController.mainPlayer.playerHealth >= minMind &&
                MainController.instance.dataController.mainPlayer.playerHealth <= maxMind);
        }

        public override GamePart FailPart() { return failPart; }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(PlayerMindCheck))]
    public class PlayerMindCheckGUInspector : Editor
    {
        private PlayerMindCheck _playerMindCheck;

        private void OnEnable() => _playerMindCheck = (PlayerMindCheck)target;

        public override void OnInspectorGUI() => ShowEventEditor(_playerMindCheck);

        public static void ShowEventEditor(PlayerMindCheck playerMindCheck)
        {
            GUILayout.Label("Проверка рассудка персонажа персонажа");

            EditorGUILayout.BeginVertical("Box");

            playerMindCheck.failPart = (GamePart)EditorGUILayout.ObjectField("Глава провала : ", playerMindCheck.failPart, typeof(GamePart), true);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Минимум", GUILayout.Width(200));
            EditorGUILayout.LabelField("Максимум", GUILayout.Width(200));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            playerMindCheck.minMind = EditorGUILayout.IntSlider(playerMindCheck.minMind, 0, 100, GUILayout.Width(200));
            playerMindCheck.maxMind = EditorGUILayout.IntSlider(playerMindCheck.maxMind, 0, 100, GUILayout.Width(200));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }
    }

#endif
}