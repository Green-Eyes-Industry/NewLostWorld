using UnityEngine;
using UnityEditor;

namespace Data.GameEvents
{
    public class PlayerHealthCheck : GameEvent
    {
        /// <summary> Минимальный уровень здоровья </summary>
        public int minHealth;

        /// <summary> Максимальный уровень здоровья </summary>
        public int maxHealth;

        /// <summary> Глава при проверке </summary>
        public GamePart failPart;

        /// <summary> Проверка на соответствие здоровья </summary>
        /// <returns> Вернет False при провале проверки </returns>
        public override bool EventStart() =>
            (MainController.instance.dataController.mainPlayer.playerHealth >= minHealth && MainController.instance.dataController.mainPlayer.playerHealth <= maxHealth);

        public override GamePart FailPart() { return failPart; }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(PlayerHealthCheck))]
    public class PlayerHealthCheckGUInspector : Editor
    {
        private PlayerHealthCheck _playerHealthCheck;

        private void OnEnable() => _playerHealthCheck = (PlayerHealthCheck)target;

        public override void OnInspectorGUI() => ShowEventEditor(_playerHealthCheck);

        public static void ShowEventEditor(PlayerHealthCheck playerHealthCheck)
        {
            GUILayout.Label("Проверка здоровья персонажа персонажа");

            EditorGUILayout.BeginVertical("Box");

            playerHealthCheck.failPart = (GamePart)EditorGUILayout.ObjectField("Глава провала : ", playerHealthCheck.failPart, typeof(GamePart), true);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Минимум", GUILayout.Width(200));
            EditorGUILayout.LabelField("Максимум", GUILayout.Width(200));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            playerHealthCheck.minHealth = EditorGUILayout.IntSlider(playerHealthCheck.minHealth, 0, 100, GUILayout.Width(200));
            playerHealthCheck.maxHealth = EditorGUILayout.IntSlider(playerHealthCheck.maxHealth, 0, 100, GUILayout.Width(200));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }
    }

#endif
}