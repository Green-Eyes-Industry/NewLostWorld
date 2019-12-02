using UnityEngine;
using UnityEditor;

namespace GUIInspector
{

    [CustomEditor(typeof(Player))]
    public class PlayerGUI_Inspector : Editor
    {
        private Player _player;

        private int _menuNum;

        private readonly string[] _menuNames = new string[]
        {
            "Инвентарь",
            "Эффекты",
            "Карта",
            "Заметки"
        };

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

            _menuNum = GUILayout.SelectionGrid(_menuNum, _menuNames, _menuNames.Length);

            GUILayout.BeginVertical("Box");

            switch (_menuNum)
            {
                case 0: PlayerInventory(); break;
                case 1: PlayerEffects(); break;
                case 2: PlayerMap(); break;
                case 3: PlayerNotes(); break;
            }

            GUILayout.EndVertical();
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
                }
            }

            if (GUILayout.Button("Добавить локацию", GUILayout.Height(20))) _player.playerMap.Add(null);

            EditorGUILayout.EndScrollView();
        }
    }
}