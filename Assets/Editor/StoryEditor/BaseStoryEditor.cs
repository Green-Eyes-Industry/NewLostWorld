using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GUIInspector.StoryEditor
{
    public class BaseStoryEditor : EditorWindow
    {
        #region VARIABLES

        #region DATA

        private GamePart _selectedPart; // Текущая глава

        private List<GamePart> _storyParts = new List<GamePart>();
        private List<GameEvent> _eventsParts = new List<GameEvent>();
        private List<GameItem> _items = new List<GameItem>();
        private List<Character> _characters = new List<Character>();
        private List<Achivemants> _achivemants = new List<Achivemants>();
        private List<GameEffect> _effects = new List<GameEffect>();
        private List<MapMark> _locations = new List<MapMark>();
        private List<Notes> _notes = new List<Notes>();

        private Color _accentColor = new Color(0.75f, 0.75f, 0.75f);
        private Color _baseColor = Color.white;

        private bool _isCheckStop = false;
        private bool _isConnectPartEnable = false;

        #endregion

        #region GLOBAL

        private Vector2 _storyMenuScroll = Vector2.zero;

        private int _selectorMainMenu;
        private readonly string[] _mainMenuNames = new string[]
        {
            "Компоненты",
            "Статистика"
        };

        private int _selectorMenu;
        private readonly string[] _menuNames = new string[] {
            "Главы",
            "События",
            "Предметы",
            "Персонажи",
            "Достижения",
            "Эффекты",
            "Локации",
            "Заметки"
        };

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

        #endregion

        #region PART_MENU

        private Vector2 _partsMenuScroll = Vector2.zero;
        private string _nameNewPart; // Имя новой главы
        private int _partType = 0; // Тип новой главы
        private readonly string[] _partTypeName = new string[]
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

        #region EVENT_MENU

        private Vector2 _eventsMenuScroll = Vector2.zero;
        private string _nameNewEvent; // Имя нового события
        private int _eventType = 0; // Тип нового события
        private readonly string[] _eventTypeName = new string[]
        {
            "Проверка влияния персонажа",
            "Контрольная точка",
            "Взаимодействие с эффектом",
            "Важное решение",
            "Влияние предмета",
            "Взаимодействие с предметом",
            "Влияние на НПС",
            "Влияние на игрока"
        };

        #endregion

        #region ITEM_MENU

        private Vector2 _itemsMenuScroll = Vector2.zero;
        private string _nameNewItem; // Имя нового предмета
        private int _itemType = 0; // Тип нового предмета
        private readonly string[] _itemTypeName = new string[]
        {
            "Активный",
            "Пасивный"
        };

        #endregion

        #region CHARACTER_MENU

        private Vector2 _charactersMenuScroll = Vector2.zero;
        private string _nameNewCharacter; // Имя нового персонажа
        private int _characterType = 0; // Тип нового персонажа
        private readonly string[] _characterTypeName = new string[]
        {
            "Игрок",
            "НПС"
        };

        #endregion

        #region ACHIVEMANTS

        private Vector2 _achivemantsMenuScroll = Vector2.zero;
        private string _nameNewAchive; // Имя нового достижения

        #endregion

        #region EFFECT_MENU

        private Vector2 _effectsMenuScroll = Vector2.zero;
        private string _nameNewEffect; // Имя нового эффекта
        private int _effectType = 0; // Тип нового эффекта
        private readonly string[] _effectTypeName = new string[]
        {
            "Положительный",
            "Отрицательный"
        };

        #endregion

        #region LOCATIONS_MENU

        private Vector2 _locationsMenuScroll = Vector2.zero;
        private string _nameNewLocation; // Имя новой локации

        #endregion

        #region NOTES_MENU

        private Vector2 _notesMenuScroll = Vector2.zero;
        private string _nameNewNote; // Имя новой заметки

        #endregion

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
            EditorGUILayout.BeginHorizontal();
            
            _selectorMainMenu = GUILayout.SelectionGrid(_selectorMainMenu, _mainMenuNames, _mainMenuNames.Length);
            if (GUILayout.Button("Сохранить",GUILayout.Width(150))) SaveAllDataFiles();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("TextArea");

            if (_selectorMainMenu == 0)
            {
                _selectorMenu = GUILayout.SelectionGrid(_selectorMenu, _menuNames, _menuNames.Length);
                GUI.backgroundColor = _accentColor;
                EditorGUILayout.BeginVertical("Box");
                GUI.backgroundColor = _baseColor;

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
            else
            {
                _storyMenuScroll = EditorGUILayout.BeginScrollView(_storyMenuScroll);

                GUILayout.Label("Всего создано : [ " + GlobalCount() + " ] Компонентов");
                EditorGUILayout.Space();

                ShowFinalStory(_storyParts.Count, "Глав");
                ShowFinalStory(_eventsParts.Count, "Событий");
                ShowFinalStory(_items.Count, "Предметов");
                ShowFinalStory(_characters.Count, "Персонажей");
                ShowFinalStory(_achivemants.Count, "Достижений");
                ShowFinalStory(_effects.Count, "Еффектов");
                ShowFinalStory(_locations.Count, "Локаций");
                ShowFinalStory(_notes.Count, "Заметок");

                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.EndVertical();
        }

        #endregion

        #region HELP_EDITOR

        #region SHOW_HELP_MENU

        /// <summary>
        /// Отобразить компонент
        /// </summary>
        private void ShowFinalStory(int countProp, string nameProp)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("[ " + countProp + " ] " + nameProp, GUILayout.Width(150));
            EditorGUILayout.IntSlider(countProp, 0, GlobalCount());

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        /// <summary>
        /// Всего компонентов
        /// </summary>
        private int GlobalCount()
        {
            return _storyParts.Count +
                _eventsParts.Count +
                _items.Count +
                _characters.Count +
                _achivemants.Count +
                _effects.Count +
                _locations.Count +
                _notes.Count;
        }

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
                if (EditorApplication.isPlaying)
                {
                    if(MoveController._startPart != null) _selectedPart = MoveController._startPart;
                }

                for (int i = 0; i < _storyParts.Count; i++)
                {
                    CheckGamePart(_storyParts[i]);

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
        /// Проверить текущую главу во время игры
        /// </summary>
        private void CheckGamePart(GamePart part)
        {
            _isCheckStop = false;
            _isConnectPartEnable = false;

            if (EditorApplication.isPlaying && _selectedPart != null)
            {
                if (_selectedPart.movePart_1 != null && !_isCheckStop)
                {
                    if (part.Equals(_selectedPart.movePart_1))
                    {
                        GUI.backgroundColor = Color.yellow;
                        _isCheckStop = true;
                    }
                    else _isConnectPartEnable = true;
                }

                if (_selectedPart.movePart_2 != null && !_isCheckStop)
                {
                    if (part.Equals(_selectedPart.movePart_2))
                    {
                        GUI.backgroundColor = Color.yellow;
                        _isCheckStop = true;
                    }
                    else _isConnectPartEnable = true;
                }

                if (_selectedPart.movePart_3 != null && !_isCheckStop)
                {
                    if (part.Equals(_selectedPart.movePart_3))
                    {
                        GUI.backgroundColor = Color.yellow;
                        _isCheckStop = true;
                    }
                    else _isConnectPartEnable = true;
                }

                if (part.Equals(_selectedPart))
                {
                    if(!_isConnectPartEnable) GUI.backgroundColor = Color.red;
                    else GUI.backgroundColor = Color.blue;
                    _isCheckStop = true;
                }

                if (!_isCheckStop) GUI.backgroundColor = _baseColor;
            }
            else GUI.backgroundColor = _baseColor;
        }

        /// <summary>
        /// Меню "События"
        /// </summary>
        private void EventsMenu()
        {
            GUILayout.Label("Сейчас создано [ " + _eventsParts.Count + " ] Событий");
            EditorGUILayout.Space();

            _eventsMenuScroll = EditorGUILayout.BeginScrollView(_eventsMenuScroll);

            if (_eventsParts.Count > 0)
            {
                for (int i = 0; i < _eventsParts.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    _eventsParts[i] = (GameEvent)EditorGUILayout.ObjectField(_eventsParts[i], typeof(GameEvent), true);
                    if (GUILayout.Button("Удалить", GUILayout.Width(80)))
                    {
                        FileUtil.DeleteFileOrDirectory(AssetDatabase.GetAssetPath(_eventsParts[i]));

                        AssetDatabase.Refresh();
                        ReloadLoadObjects(LoadDataType.EVENT_PART);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            _eventType = EditorGUILayout.Popup(_eventType, _eventTypeName, GUILayout.Width(200));
            _nameNewEvent = EditorGUILayout.TextField(_nameNewEvent);
            if (GUILayout.Button("Создать", GUILayout.Width(100))) CreateNewEvent(_nameNewEvent, _eventType);

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Меню "Предметы"
        /// </summary>
        private void ItemsMenu()
        {
            GUILayout.Label("Сейчас создано [ " + _items.Count + " ] Предметов");
            EditorGUILayout.Space();

            _itemsMenuScroll = EditorGUILayout.BeginScrollView(_itemsMenuScroll);

            if (_items.Count > 0)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    _items[i] = (GameItem)EditorGUILayout.ObjectField(_items[i], typeof(GameItem), true);
                    if (GUILayout.Button("Удалить", GUILayout.Width(80)))
                    {
                        FileUtil.DeleteFileOrDirectory(AssetDatabase.GetAssetPath(_items[i]));

                        AssetDatabase.Refresh();
                        ReloadLoadObjects(LoadDataType.ITEM);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            _itemType = EditorGUILayout.Popup(_itemType, _itemTypeName, GUILayout.Width(100));
            _nameNewItem = EditorGUILayout.TextField(_nameNewItem);
            if (GUILayout.Button("Создать", GUILayout.Width(100))) CreateNewItem(_nameNewItem, _itemType);

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Меню "Персонажи"
        /// </summary>
        private void CharactersMenu()
        {
            GUILayout.Label("Сейчас создано [ " + _characters.Count + " ] Персонажей");
            EditorGUILayout.Space();

            _charactersMenuScroll = EditorGUILayout.BeginScrollView(_charactersMenuScroll);

            if (_characters.Count > 0)
            {
                for (int i = 0; i < _characters.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    _characters[i] = (Character)EditorGUILayout.ObjectField(_characters[i], typeof(Achivemants), true);
                    if (GUILayout.Button("Удалить", GUILayout.Width(80)))
                    {
                        FileUtil.DeleteFileOrDirectory(AssetDatabase.GetAssetPath(_characters[i]));

                        AssetDatabase.Refresh();
                        ReloadLoadObjects(LoadDataType.CHARACTER);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            _characterType = EditorGUILayout.Popup(_characterType, _characterTypeName, GUILayout.Width(100));
            _nameNewCharacter = EditorGUILayout.TextField(_nameNewCharacter);
            if (GUILayout.Button("Создать", GUILayout.Width(100))) CreateNewCharacter(_nameNewCharacter, _characterType);

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Меню "Достижения"
        /// </summary>
        private void AchivemantsMenu()
        {
            GUILayout.Label("Сейчас создано [ " + _achivemants.Count + " ] Достижений");
            EditorGUILayout.Space();

            _achivemantsMenuScroll = EditorGUILayout.BeginScrollView(_achivemantsMenuScroll);

            if (_achivemants.Count > 0)
            {
                for (int i = 0; i < _achivemants.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    _achivemants[i] = (Achivemants)EditorGUILayout.ObjectField(_achivemants[i], typeof(Achivemants), true);
                    if (GUILayout.Button("Удалить", GUILayout.Width(80)))
                    {
                        FileUtil.DeleteFileOrDirectory(AssetDatabase.GetAssetPath(_achivemants[i]));

                        AssetDatabase.Refresh();
                        ReloadLoadObjects(LoadDataType.ACHIVE);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            _nameNewAchive = EditorGUILayout.TextField("Достижение",_nameNewAchive);
            if (GUILayout.Button("Создать", GUILayout.Width(100))) CreateNewAchive(_nameNewAchive);

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Меню "Эффекты"
        /// </summary>
        private void EffectsMenu()
        {
            GUILayout.Label("Сейчас создано [ " + _effects.Count + " ] Еффектов");
            EditorGUILayout.Space();

            _effectsMenuScroll = EditorGUILayout.BeginScrollView(_effectsMenuScroll);

            if (_effects.Count > 0)
            {
                for (int i = 0; i < _effects.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    _effects[i] = (GameEffect)EditorGUILayout.ObjectField(_effects[i], typeof(GameEffect), true);
                    if (GUILayout.Button("Удалить", GUILayout.Width(80)))
                    {
                        FileUtil.DeleteFileOrDirectory(AssetDatabase.GetAssetPath(_effects[i]));

                        AssetDatabase.Refresh();
                        ReloadLoadObjects(LoadDataType.EFFECT);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            _effectType = EditorGUILayout.Popup(_effectType, _effectTypeName, GUILayout.Width(150));
            _nameNewEffect = EditorGUILayout.TextField(_nameNewEffect);
            if (GUILayout.Button("Создать", GUILayout.Width(100))) CreateNewEffect(_nameNewEffect, _effectType);

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Меню "Локации"
        /// </summary>
        private void LocationsMenu()
        {
            GUILayout.Label("Сейчас создано [ " + _locations.Count + " ] Локаций");
            EditorGUILayout.Space();

            _locationsMenuScroll = EditorGUILayout.BeginScrollView(_locationsMenuScroll);

            if (_locations.Count > 0)
            {
                for (int i = 0; i < _locations.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    _locations[i] = (MapMark)EditorGUILayout.ObjectField(_locations[i], typeof(MapMark), true);
                    if (GUILayout.Button("Удалить", GUILayout.Width(80)))
                    {
                        FileUtil.DeleteFileOrDirectory(AssetDatabase.GetAssetPath(_locations[i]));

                        AssetDatabase.Refresh();
                        ReloadLoadObjects(LoadDataType.LOCATION);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            _nameNewLocation = EditorGUILayout.TextField("Локация", _nameNewLocation);
            if (GUILayout.Button("Создать", GUILayout.Width(100))) CreateNewLocation(_nameNewLocation);

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Меню "Заметки"
        /// </summary>
        private void NotesMenu()
        {
            GUILayout.Label("Сейчас создано [ " + _notes.Count + " ] Заметок");
            EditorGUILayout.Space();

            _notesMenuScroll = EditorGUILayout.BeginScrollView(_notesMenuScroll);

            if (_notes.Count > 0)
            {
                for (int i = 0; i < _notes.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    _notes[i] = (Notes)EditorGUILayout.ObjectField(_notes[i], typeof(Notes), true);
                    if (GUILayout.Button("Удалить", GUILayout.Width(80)))
                    {
                        FileUtil.DeleteFileOrDirectory(AssetDatabase.GetAssetPath(_notes[i]));

                        AssetDatabase.Refresh();
                        ReloadLoadObjects(LoadDataType.NOTE);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            _nameNewNote = EditorGUILayout.TextField("Заметка", _nameNewNote);
            if (GUILayout.Button("Создать", GUILayout.Width(100))) CreateNewNote(_nameNewNote);

            EditorGUILayout.EndHorizontal();
        }

        #endregion

        #region CREATE_OBJECTS

        /// <summary>
        /// Создать главу
        /// </summary>
        private void CreateNewPart(string namePart, int partType)
        {
            switch (partType)
            {
                case 0:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(TextPart)),
                        "Assets/Resources/GameParts/" +
                        _storyParts.Count +
                        "_" +
                        namePart + ".asset");
                    break;

                case 1:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(ChangePart)),
                        "Assets/Resources/GameParts/" +
                        _storyParts.Count +
                        "_" +
                        namePart +
                        ".asset");
                    break;

                case 2:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(BattlePart)),
                        "Assets/Resources/GameParts/" +
                        _storyParts.Count +
                        "_" +
                        namePart +
                        ".asset");
                    break;

                case 3:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(EventPart)),
                        "Assets/Resources/GameParts/" +
                        _storyParts.Count +
                        "_" +
                        namePart +
                        ".asset");
                    break;

                case 4:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(MoviePart)),
                        "Assets/Resources/GameParts/" +
                        _storyParts.Count +
                        "_" +
                        namePart +
                        ".asset");
                    break;

                case 5:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(LeandPart)),
                        "Assets/Resources/GameParts/" +
                        _storyParts.Count +
                        "_" +
                        namePart +
                        ".asset");
                    break;

                case 6:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(PazzlePart)),
                        "Assets/Resources/GameParts/" +
                        _storyParts.Count +
                        "_" +
                        namePart +
                        ".asset");
                    break;

                case 7:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(FinalPart)),
                        "Assets/Resources/GameParts/" +
                        _storyParts.Count +
                        "_" +
                        namePart +
                        ".asset");
                    break;
            }

            ReloadLoadObjects(LoadDataType.GAME_PART);
        }

        /// <summary>
        /// Создать новый эвент
        /// </summary>
        private void CreateNewEvent(string nameEvent, int typeEvent)
        {
            switch (typeEvent)
            {
                case 0:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(CheckPlayerInfl)),
                        "Assets/Resources/GameEvents/" +
                        _eventsParts.Count +
                        "_" +
                        nameEvent +
                        ".asset");
                    break;

                case 1:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(CheckPoint)),
                        "Assets/Resources/GameEvents/" +
                        _eventsParts.Count +
                        "_" +
                        nameEvent +
                        ".asset");
                    break;

                case 2:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(EffectInteract)),
                        "Assets/Resources/GameEvents/" +
                        _eventsParts.Count +
                        "_" +
                        nameEvent +
                        ".asset");
                    break;

                case 3:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(ImportantDecision)),
                        "Assets/Resources/GameEvents/" +
                        _eventsParts.Count +
                        "_" +
                        nameEvent +
                        ".asset");
                    break;

                case 4:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(ItemInfl)),
                        "Assets/Resources/GameEvents/" +
                        _eventsParts.Count +
                        "_" +
                        nameEvent +
                        ".asset");
                    break;

                case 5:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(ItemInteract)),
                        "Assets/Resources/GameEvents/" +
                        _eventsParts.Count +
                        "_" +
                        nameEvent +
                        ".asset");
                    break;

                case 6:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(NonPlayerInfl)),
                        "Assets/Resources/GameEvents/" +
                        _eventsParts.Count +
                        "_" +
                        nameEvent +
                        ".asset");
                    break;

                case 7:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(PlayerInfl)),
                        "Assets/Resources/GameEvents/" +
                        _eventsParts.Count +
                        "_" +
                        nameEvent +
                        ".asset");
                    break;
            }

            ReloadLoadObjects(LoadDataType.EVENT_PART);
        }

        /// <summary>
        /// Создать новый предмет
        /// </summary>
        private void CreateNewItem(string nameItem, int typeItem)
        {
            switch (typeItem)
            {
                case 0:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(UsableItem)),
                        "Assets/Resources/GameItems/" +
                        _items.Count +
                        "_" +
                        nameItem +
                        ".asset");
                    break;

                case 1:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(PasiveItem)),
                        "Assets/Resources/GameItems/" +
                        _items.Count +
                        "_" +
                        nameItem +
                        ".asset");
                    break;
            }

            ReloadLoadObjects(LoadDataType.ITEM);
        }

        /// <summary>
        /// Создать нового персонажа
        /// </summary>
        private void CreateNewCharacter(string nameCharacter, int typeCharacter)
        {
            switch (typeCharacter)
            {
                case 0:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(Player)),
                        "Assets/Resources/Players/" +
                        _characters.Count +
                        "_" +
                        nameCharacter +
                        ".asset");
                    break;

                case 1:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(NonPlayer)),
                        "Assets/Resources/Players/NonPlayers/" +
                        _characters.Count +
                        "_" +
                        nameCharacter +
                        ".asset");
                    break;
            }

            ReloadLoadObjects(LoadDataType.CHARACTER);
        }

        /// <summary>
        /// Создать новое достижение
        /// </summary>
        private void CreateNewAchive(string nameAchive)
        {
            AssetDatabase.CreateAsset(CreateInstance(typeof(Achivemants)),
                "Assets/Resources/Achivemants/" +
                _achivemants.Count +
                "_" +
                nameAchive +
                ".asset");

            ReloadLoadObjects(LoadDataType.ACHIVE);
        }

        /// <summary>
        /// Создать новый эффект
        /// </summary>
        private void CreateNewEffect(string nameEffect, int typeEffect)
        {
            switch (typeEffect)
            {
                case 0:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(PositiveEffect)),
                        "Assets/Resources/PlayerEffects/" +
                        _effects.Count +
                        "_" +
                        nameEffect +
                        ".asset");
                    break;

                case 1:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(NegativeEffect)),
                        "Assets/Resources/PlayerEffects/" +
                        _effects.Count +
                        "_" +
                        nameEffect +
                        ".asset");
                    break;
            }

            ReloadLoadObjects(LoadDataType.EFFECT);
        }

        /// <summary>
        /// Создать новую локацию
        /// </summary>
        private void CreateNewLocation(string nameLocation)
        {
            AssetDatabase.CreateAsset(CreateInstance(typeof(MapMark)),
                "Assets/Resources/Locations/" +
                _locations.Count +
                "_" +
                nameLocation +
                ".asset");

            ReloadLoadObjects(LoadDataType.LOCATION);
        }

        /// <summary>
        /// Создать новую заметку
        /// </summary>
        private void CreateNewNote(string nameNote)
        {
            AssetDatabase.CreateAsset(CreateInstance(typeof(Notes)),
                "Assets/Resources/Notes/" +
                _notes.Count +
                "_" +
                nameNote +
                ".asset");

            ReloadLoadObjects(LoadDataType.NOTE);
        }

        #endregion

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
                        if (loadedObj[i] is GameEvent) _eventsParts.Add((GameEvent)loadedObj[i]);
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

        /// <summary>
        /// Сохранить все данные
        /// </summary>
        public void SaveAllDataFiles()
        {
            for (int i = 0; i < _storyParts.Count; i++) { EditorUtility.SetDirty(_storyParts[i]); }
            for (int i = 0; i < _eventsParts.Count; i++) { EditorUtility.SetDirty(_eventsParts[i]); }
            for (int i = 0; i < _items.Count; i++) { EditorUtility.SetDirty(_items[i]); }
            for (int i = 0; i < _characters.Count; i++) { EditorUtility.SetDirty(_characters[i]); }
            for (int i = 0; i < _achivemants.Count; i++) { EditorUtility.SetDirty(_achivemants[i]); }
            for (int i = 0; i < _effects.Count; i++) { EditorUtility.SetDirty(_effects[i]); }
            for (int i = 0; i < _locations.Count; i++) { EditorUtility.SetDirty(_locations[i]); }
            for (int i = 0; i < _notes.Count; i++) { EditorUtility.SetDirty(_notes[i]); }
        }

        #endregion
    }
}