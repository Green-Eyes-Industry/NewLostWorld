using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GUIInspector.StoryEditor
{

    public class BaseStoryEditor : EditorWindow
    {
        #region VARIABLES

        // Меню

        private int _selectorMainMenu;
        private string[] _mainMenuNames = new string[] { "Компоненты", "Сценарий" };

        private int _selectorMenu;
        private string[] _menuNames = new string[] { "Главы", "События", "Предметы", "Персонажи", "Достижения", "Эффекты", "Локации", "Заметки" };

        // Списки данных

        private List<GamePart> _storyParts = new List<GamePart>();
        private List<EventPart> _eventsParts = new List<EventPart>();
        private List<GameItem> _items = new List<GameItem>();
        private List<Character> _characters = new List<Character>();
        private List<Achivemants> _achivemants = new List<Achivemants>();
        private List<GameEffect> _effects = new List<GameEffect>();
        private List<MapMark> _locations = new List<MapMark>();
        private List<Notes> _notes = new List<Notes>();

        // Слайдеры для меню

        private Vector2 _partsMenuScroll = Vector2.zero;
        private Vector2 _eventsMenuScroll = Vector2.zero;
        private Vector2 _itemsMenuScroll = Vector2.zero;
        private Vector2 _charactersMenuScroll = Vector2.zero;
        private Vector2 _achivemantsMenuScroll = Vector2.zero;
        private Vector2 _effectsMenuScroll = Vector2.zero;
        private Vector2 _locationsMenuScroll = Vector2.zero;
        private Vector2 _notesMenuScroll = Vector2.zero;

        private Vector2 _storyMenuScroll = Vector2.zero;

        // Типы данных для обращения

        public enum LoadDataType
        {
            GAME_PART,
            EVENT_PART,
            ITEM,
            CHARACTER,
            ACHIVE,
            EFFECT,
            LOCATION,
            NOTE
        }

        // Дополнительные переменные

        private string _nameNewPart; // Имя новой главы

        private int _partType = 0; // Тип новой главы

        private string[] _partTypeName = new string[]
        {
            "Текстовая",
            "Выбор",
            "Бой",
            "Евент",
            "Слайдшоу",
            "Вставка",
            "Загадка",
            "Финальна"
        };

        #endregion

        #region INIT

        [MenuItem("Story Editor/Editor")]
        [System.Obsolete]
        static void ShowEditor()
        {
            // Создание окна редактора
            BaseStoryEditor storyEditor = GetWindow<BaseStoryEditor>();
            storyEditor.title = "Story Editor";

            storyEditor.ReloadLoadObjects();
        }

        private void OnGUI()
        {
            _selectorMainMenu = GUILayout.SelectionGrid(_selectorMainMenu, _mainMenuNames, _mainMenuNames.Length);

            EditorGUILayout.BeginVertical("TextArea");

            if (_selectorMainMenu == 0)
            {
                _selectorMenu = GUILayout.SelectionGrid(_selectorMenu, _menuNames, _menuNames.Length);

                EditorGUILayout.BeginVertical("Box");

                switch (_selectorMenu)
                {
                    case 0: PartMenu(); break;
                    case 1: EventsMenu(); break;
                    case 2: ItemsMenu(); break;
                    case 3: CharactersMenu(); break;
                    case 4: AchivemantsMenu(); break;
                    case 5: EffectsMenu(); break;
                    case 6: LocationsMenu(); break;
                    case 7: NotesMenu(); break;
                }

                EditorGUILayout.EndVertical();
            }
            else StoryMenu();

            EditorGUILayout.EndVertical();
        }

        #endregion

        #region HELP_EDITOR

        /// <summary>
        /// Меню "Главы"
        /// </summary>
        private void PartMenu()
        {
            GUILayout.Label("Сейчас создано [ " + _storyParts.Count + " ] Глав");

            EditorGUILayout.Space();

            _partsMenuScroll = EditorGUILayout.BeginScrollView(_partsMenuScroll);

            if (_storyParts.Count > 0)
            {
                for (int i = 0; i < _storyParts.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    _storyParts[i] = (GamePart)EditorGUILayout.ObjectField(_storyParts[i], typeof(GamePart), true);
                    if(GUILayout.Button("Удалить", GUILayout.Width(80)))
                    {
                        FileUtil.DeleteFileOrDirectory(AssetDatabase.GetAssetPath(_storyParts[i]));

                        AssetDatabase.Refresh();
                        ReloadLoadObjects(LoadDataType.GAME_PART);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            _partType = EditorGUILayout.Popup(_partType, _partTypeName,GUILayout.Width(100));
            _nameNewPart = EditorGUILayout.TextField(_nameNewPart);
            if(GUILayout.Button("Создать", GUILayout.Width(100))) CreateNewPart(_nameNewPart, _partType);

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Создать главу
        /// </summary>
        /// <param name="namePart">Имя</param>
        /// <param name="partType">Тип</param>
        private void CreateNewPart(string namePart, int partType)
        {
            switch (partType)
            {
                case 0:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(TextPart)), "Assets/Resources/GameParts/" + namePart + ".asset");
                    break;

                case 1:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(ChangePart)), "Assets/Resources/GameParts/" + namePart + ".asset");
                    break;

                case 2:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(BattlePart)), "Assets/Resources/GameParts/" + namePart + ".asset");
                    break;

                case 3:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(EventPart)), "Assets/Resources/GameParts/" + namePart + ".asset");
                    break;

                case 4:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(MoviePart)), "Assets/Resources/GameParts/" + namePart + ".asset");
                    break;

                case 5:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(LeandPart)), "Assets/Resources/GameParts/" + namePart + ".asset");
                    break;

                case 6:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(PazzlePart)), "Assets/Resources/GameParts/" + namePart + ".asset");
                    break;

                case 7:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(FinalPart)), "Assets/Resources/GameParts/" + namePart + ".asset");
                    break;
            }

            ReloadLoadObjects(LoadDataType.GAME_PART);
        }

        /// <summary>
        /// Меню "События"
        /// </summary>
        private void EventsMenu()
        {
            _eventsMenuScroll = EditorGUILayout.BeginScrollView(_eventsMenuScroll);


            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Меню "Предметы"
        /// </summary>
        private void ItemsMenu()
        {
            _itemsMenuScroll = EditorGUILayout.BeginScrollView(_itemsMenuScroll);


            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Меню "Персонажи"
        /// </summary>
        private void CharactersMenu()
        {
            _charactersMenuScroll = EditorGUILayout.BeginScrollView(_charactersMenuScroll);


            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Меню "Достижения"
        /// </summary>
        private void AchivemantsMenu()
        {
            _achivemantsMenuScroll = EditorGUILayout.BeginScrollView(_achivemantsMenuScroll);


            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Меню "Эффекты"
        /// </summary>
        private void EffectsMenu()
        {
            _effectsMenuScroll = EditorGUILayout.BeginScrollView(_effectsMenuScroll);


            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Меню "Локации"
        /// </summary>
        private void LocationsMenu()
        {
            _locationsMenuScroll = EditorGUILayout.BeginScrollView(_locationsMenuScroll);


            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Меню "Заметки"
        /// </summary>
        private void NotesMenu()
        {
            _notesMenuScroll = EditorGUILayout.BeginScrollView(_notesMenuScroll);


            EditorGUILayout.EndScrollView();
        }

        #endregion

        #region STORY_EDITOR

        /// <summary>
        /// Меню "Сюжет"
        /// </summary>
        private void StoryMenu()
        {
            _storyMenuScroll = EditorGUILayout.BeginScrollView(_storyMenuScroll);

            

            EditorGUILayout.EndScrollView();
        }

        #endregion

        #region HELP_METHODS

        /// <summary>
        /// Перезагрузить файлы по выбранному типу
        /// </summary>
        public void ReloadLoadObjects(LoadDataType typeData)
        {
            string resPath = ""; // Путь в директории Resources
            Object[] loadedObj;

            switch (typeData)
            {
                case LoadDataType.GAME_PART: resPath = "GameParts"; break;
                case LoadDataType.EVENT_PART: resPath = "GameEvents"; break;
                case LoadDataType.ITEM: resPath = "GameItems"; break;
                case LoadDataType.CHARACTER: resPath = "Players"; break;
                case LoadDataType.ACHIVE: resPath = "Achivemants"; break;
                case LoadDataType.EFFECT: resPath = "PlayerEffects"; break;
                case LoadDataType.LOCATION: resPath = "Locations"; break;
                case LoadDataType.NOTE: resPath = "Notes"; break;
            }

            loadedObj = Resources.LoadAll(resPath); // Загрузка файлов из выбранной директории

            // Преобразование и перегрузка

            switch (typeData)
            {
                case LoadDataType.GAME_PART:

                    _storyParts.Clear();

                    for (int i = 0; i < loadedObj.Length; i++)
                    {
                        if (loadedObj[i] is GamePart) _storyParts.Add((GamePart)loadedObj[i]);
                    }

                    break;

                case LoadDataType.EVENT_PART:

                    _eventsParts.Clear();

                    for (int i = 0; i < loadedObj.Length; i++)
                    {
                        if (loadedObj[i] is EventPart) _eventsParts.Add((EventPart)loadedObj[i]);
                    }

                    break;

                case LoadDataType.ITEM:

                    _items.Clear();

                    for (int i = 0; i < loadedObj.Length; i++)
                    {
                        if (loadedObj[i] is GameItem) _items.Add((GameItem)loadedObj[i]);
                    }

                    break;

                case LoadDataType.CHARACTER:

                    _characters.Clear();

                    for (int i = 0; i < loadedObj.Length; i++)
                    {
                        if (loadedObj[i] is Character) _characters.Add((Character)loadedObj[i]);
                    }

                    break;

                case LoadDataType.ACHIVE:

                    _achivemants.Clear();

                    for (int i = 0; i < loadedObj.Length; i++)
                    {
                        if (loadedObj[i] is Achivemants) _achivemants.Add((Achivemants)loadedObj[i]);
                    }

                    break;

                case LoadDataType.EFFECT:

                    _effects.Clear();

                    for (int i = 0; i < loadedObj.Length; i++)
                    {
                        if (loadedObj[i] is GameEffect) _effects.Add((GameEffect)loadedObj[i]);
                    }

                    break;

                case LoadDataType.LOCATION:

                    _locations.Clear();

                    for (int i = 0; i < loadedObj.Length; i++)
                    {
                        if (loadedObj[i] is MapMark) _locations.Add((MapMark)loadedObj[i]);
                    }

                    break;

                case LoadDataType.NOTE:

                    _notes.Clear();

                    for (int i = 0; i < loadedObj.Length; i++)
                    {
                        if (loadedObj[i] is Notes) _notes.Add((Notes)loadedObj[i]);
                    }

                    break;
            }
        }

        /// <summary>
        /// Перегрузить все файлы
        /// </summary>
        public void ReloadLoadObjects()
        {
            ReloadLoadObjects(LoadDataType.GAME_PART);
            ReloadLoadObjects(LoadDataType.EVENT_PART);
            ReloadLoadObjects(LoadDataType.ITEM);
            ReloadLoadObjects(LoadDataType.CHARACTER);
            ReloadLoadObjects(LoadDataType.ACHIVE);
            ReloadLoadObjects(LoadDataType.EFFECT);
            ReloadLoadObjects(LoadDataType.LOCATION);
            ReloadLoadObjects(LoadDataType.NOTE);
        }

        #endregion
    }
}