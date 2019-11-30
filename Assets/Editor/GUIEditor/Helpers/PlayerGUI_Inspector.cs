using UnityEngine;
using UnityEditor;

namespace GUIInspector
{

    [CustomEditor(typeof(Player))]
    public class PlayerGUI_Inspector : Editor
    {
        private Player _player;

        /// <summary>
        /// Открытие вкладок
        /// </summary>
        private bool
            _playerEffects,
            _playerInventory,
            _playerNotes,
            _playerMap;

        /// <summary>
        /// Прокрутки
        /// </summary>
        private Vector2
            _playerEffectsScroll,
            _playerInventoryScroll,
            _playerNotesScroll,
            _playerMapScroll;

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

            GUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            _playerEffects = EditorGUILayout.BeginFoldoutHeaderGroup(_playerEffects, "Эффекты персонажа");
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (_playerEffects)
            {
                GUILayout.BeginVertical("Button");
                PlayerEffects();
                GUILayout.EndVertical();
            }

            GUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            _playerInventory = EditorGUILayout.BeginFoldoutHeaderGroup(_playerInventory, "Инвентарь персонажа");
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (_playerInventory)
            {
                GUILayout.BeginVertical("Button");
                PlayerInventory();
                GUILayout.EndVertical();
            }

            GUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            _playerNotes = EditorGUILayout.BeginFoldoutHeaderGroup(_playerNotes, "Заметки персонажа");
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (_playerNotes)
            {
                GUILayout.BeginVertical("Button");
                PlayerNotes();
                GUILayout.EndVertical();
            }

            GUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            _playerMap = EditorGUILayout.BeginFoldoutHeaderGroup(_playerMap, "Карта персонажа");
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (_playerMap)
            {
                GUILayout.BeginVertical("Button");
                PlayerMap();
                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();

            if (GUILayout.Button("Сохранить персонажа игрока", GUILayout.Height(20))) EditorUtility.SetDirty(_player);
        }

        /// <summary>
        /// Отображение списка действующих еффектов
        /// </summary>
        private void PlayerEffects()
        {
            _playerEffectsScroll = EditorGUILayout.BeginScrollView(_playerEffectsScroll);

            if (_player.playerEffects.Count > 0)
            {
                for (int i = 0; i < _player.playerEffects.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    _player.playerEffects[i] = (GameEffect)EditorGUILayout.ObjectField(_player.playerEffects[i], typeof(GameEffect), true);
                    if (GUILayout.Button("Удалить", GUILayout.Width(70))) _player.playerEffects.RemoveAt(i);

                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }

            if (GUILayout.Button("Добавить эффект", GUILayout.Height(20))) _player.playerEffects.Add(null);

            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Отображение списка предметов
        /// </summary>
        private void PlayerInventory()
        {
            _playerInventoryScroll = EditorGUILayout.BeginScrollView(_playerInventoryScroll);

            if (_player.playerInventory.Count > 0)
            {
                for (int i = 0; i < _player.playerInventory.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    _player.playerInventory[i] = (GameItem)EditorGUILayout.ObjectField(_player.playerInventory[i], typeof(GameItem), true);
                    if (GUILayout.Button("Удалить", GUILayout.Width(70))) _player.playerInventory.RemoveAt(i);

                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }

            if (GUILayout.Button("Добавить предмет", GUILayout.Height(20))) _player.playerInventory.Add(null);

            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Отображение списка доступных заметок
        /// </summary>
        private void PlayerNotes()
        {
            _playerNotesScroll = EditorGUILayout.BeginScrollView(_playerNotesScroll);

            if (_player.playerNotes.Count > 0)
            {
                for (int i = 0; i < _player.playerNotes.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    _player.playerNotes[i] = (Notes)EditorGUILayout.ObjectField(_player.playerNotes[i], typeof(Notes), true);
                    if (GUILayout.Button("Удалить", GUILayout.Width(70))) _player.playerNotes.RemoveAt(i);

                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }

            if (GUILayout.Button("Добавить заметку", GUILayout.Height(20))) _player.playerNotes.Add(null);

            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Отображение списка доступных локаций на карте
        /// </summary>
        private void PlayerMap()
        {
            _playerMapScroll = EditorGUILayout.BeginScrollView(_playerMapScroll);

            if (_player.playerMap.Count > 0)
            {
                for (int i = 0; i < _player.playerMap.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    _player.playerMap[i] = (MapMark)EditorGUILayout.ObjectField(_player.playerMap[i], typeof(MapMark), true);
                    if (GUILayout.Button("Удалить", GUILayout.Width(70))) _player.playerMap.RemoveAt(i);

                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }

            if (GUILayout.Button("Добавить локацию", GUILayout.Height(20))) _player.playerMap.Add(null);

            EditorGUILayout.EndScrollView();
        }
    }
}