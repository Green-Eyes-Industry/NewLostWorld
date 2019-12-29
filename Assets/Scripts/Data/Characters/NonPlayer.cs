﻿using UnityEditor;
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
    public class NonPlayerGInspector : Editor
    {
        private NonPlayer _nonPlayer;

        private void OnEnable() => _nonPlayer = (NonPlayer)target;

        public override void OnInspectorGUI() => ShowNonPlayerGUI(_nonPlayer);

        /// <summary> Показать мено редактора не игрового персонажа </summary>
        private static void ShowNonPlayerGUI(NonPlayer nonPlayer)
        {
            EditorGUILayout.LabelField("Не игровой персонаж");

            EditorGUILayout.BeginVertical("Box");

            nonPlayer.npName = EditorGUILayout.TextField("Имя персонажа", nonPlayer.npName);
            nonPlayer.npToPlayerRatio = EditorGUILayout.IntSlider(nonPlayer.npToPlayerRatio, -10, 10);

            EditorGUILayout.EndVertical();
        }
    }

#endif
}