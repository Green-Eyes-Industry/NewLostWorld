using System.Collections.Generic;
using Helpers;
using UnityEditor;
using UnityEngine;

namespace Data.Characters
{
    [CreateAssetMenu(fileName = "New player", menuName = "Игровые обьекты/Новый персонаж/Игрок", order = 0)]
    public class Player : Character
    {
        /// <summary> Здоровье игрока </summary>
        public int playerHealth;

        /// <summary> Рассудок игрока </summary>
        public int playerMind;

        /// <summary> Список эффектов на игроке </summary>
        public List<GameEffect> playerEffects;

        /// <summary> Инвентарь игрока </summary>
        public List<GameItem> playerInventory;

        /// <summary> Заметки игрока </summary>
        public List<Note> playerNotes;

        /// <summary> Открытиые локации </summary>
        public List<MapMark> playerMap;

        /// <summary> Важные решения </summary>
        public List<Decision> playerDecisions;

        /// <summary> Встреченные персонажи </summary>
        public List<NonPlayer> playerMeet;
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(Player))]
    public class PlayerGInspector : Editor
    {
        private Player _player;

        private int _menuNum;

        private readonly string[] _menuNames = new string[]
        {
            "Инвентарь",
            "Эффекты",
            "Карта",
            "Заметки",
            "Решения",
            "Встречи"
        };

        /// <summary> Прокрутки </summary>
        private Vector2 _scroll;

        private void OnEnable() => _player = (Player)target;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Характеристики игрока");

            GUILayout.BeginVertical("Box");

            _player.playerHealth = EditorGUILayout.IntSlider("Здоровье : ", _player.playerHealth, 0, 100);
            _player.playerMind = EditorGUILayout.IntSlider("Рассудок : ", _player.playerMind, 0, 100);

            GUILayout.EndVertical();

            EditorGUILayout.LabelField("Дополнительные параметры");

            GUILayout.BeginVertical("Box");

            _menuNum = GUILayout.SelectionGrid(_menuNum, _menuNames, _menuNames.Length);

            switch (_menuNum)
            {
                case 0: ShowListItems(_player.playerInventory); break;
                case 1: ShowListItems(_player.playerEffects); break;
                case 2: ShowListItems(_player.playerMap); break;
                case 3: ShowListItems(_player.playerNotes); break;
                case 4: ShowListItems(_player.playerDecisions); break;
                case 5: ShowListItems(_player.playerMeet); break;
            }

            GUILayout.EndVertical();
        }

        /// <summary> Отобразить список </summary>
        private void ShowListItems<T>(List<T> items)
        {
            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            if (items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] == null)
                    {
                        items.RemoveAt(i);
                        break;
                    }

                    GUILayout.BeginHorizontal("Box");

                    EditorGUILayout.LabelField(items[i].ToString());
                    if (GUILayout.Button("Удалить", GUILayout.Width(70))) items.RemoveAt(i);

                    GUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();
        }
    }

#endif
}