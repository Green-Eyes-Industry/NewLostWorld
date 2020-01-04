using UnityEditor;
using UnityEngine;

namespace Data.Characters
{
    [CreateAssetMenu(fileName = "New npc", menuName = "Игровые обьекты/Новый персонаж/НПС", order = 1)]
    public class NonPlayer : Character
    {
        /// <summary> Имя НПС </summary>
        public string npName;

        /// <summary> Отношение НПС к игроку </summary>
        public int npToPlayerRatio;
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(NonPlayer))]
    public class NonPlayerGUInspector : Editor
    {
        private NonPlayer _nonPlayer;

        private void OnEnable() => _nonPlayer = (NonPlayer)target;

        public override void OnInspectorGUI() => ShowNonPlayerGUI(_nonPlayer);

        /// <summary> Показать мено редактора не игрового персонажа </summary>
        public static void ShowNonPlayerGUI(NonPlayer nonPlayer)
        {
            EditorGUILayout.LabelField("Не игровой персонаж");

            EditorGUILayout.BeginVertical("Box");

            nonPlayer.npName = EditorGUILayout.TextField("Имя персонажа :", nonPlayer.npName);

            if (nonPlayer.npToPlayerRatio < 0) GUI.backgroundColor = Color.red;
            else if (nonPlayer.npToPlayerRatio > 0) GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.white;

            nonPlayer.npToPlayerRatio = EditorGUILayout.IntSlider("Отношение к вам :",nonPlayer.npToPlayerRatio, -10, 10);

            EditorGUILayout.EndVertical();
        }
    }

#endif
}