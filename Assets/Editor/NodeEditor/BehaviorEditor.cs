using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GUIInspector.NodeEditor
{
    public class BehaviorEditor : EditorWindow
    {
        #region VARIABLES

        private Color _gridColor = new Color(0.55f, 0.55f, 0.55f); // Цвет сетки
        private Vector2 _offset; // Отступ поля
        private Vector2 _drag; // Отступ нод

        private GameSettings _mainSettings; // Настройки поля
        public static BehaviorEditor trBehaviorEditor; // ссылка на себя
        private StoryData _storyData; // Данные сюжета
        private Vector3 _mousePosition; // Позиция мыши
        private bool _isClickOnWindow; // Нажал на окно или нет
        private GamePart _selectedNode; // Выбранная нода

        private Texture _connectTexture; // Текстура подключения
        private GamePart _sellectedToConnect; // Нода в памяти при подключении полключения
        private int _sellectionId; // Идентификатор типа подключения

        public enum UserActions
        {
            ADD_TEXT_PART,
            ADD_CHANGE_PART,
            ADD_BATTLE_PART,
            ADD_MAZE_PART,
            ADD_EVENT_PART,
            ADD_FINAL_PART,
            ADD_LABEL_PART,
            ADD_SLIDESHOW_PART,
            ADD_TRANSIT
        }

        public enum AddEventActions
        {
            CHECK_DECISION,
            CHECK_PLAYER_INFL,
            CHECK_POINT,
            EFFECT_INTERACT,
            IMPORTANT_DECISION,
            ITEM_INFL,
            ITEM_INTERACT,
            LOCATION_FIND,
            MEMBER_TIME,
            NON_PLAYER_INFL,
            PLAYER_INFL,
            RANDOM_PART
        }

        #endregion

        #region INIT

        [MenuItem("Story/Node Editor")]
        [System.Obsolete]
        public static void ShowEditor()
        {
            BehaviorEditor editor = GetWindow<BehaviorEditor>();
            editor.title = "Node Editor";
            editor._gridColor = new Color(0.55f, 0.55f, 0.55f);
            trBehaviorEditor = editor;

            editor._connectTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeEditor/Images/Connect.png", typeof(Texture));
        }

        #endregion

        #region GUI_METHODS

        private void OnFocus()
        {
            if (EditorApplication.isPlaying) EditorApplication.isPaused = true;
        }

        private void OnLostFocus()
        {
            if (EditorApplication.isPaused) EditorApplication.isPaused = false;
        }

        private void OnGUI()
        {
            if (_storyData != null)
            {
                DrawGrid(10, 0.2f, _gridColor);
                DrawGrid(50, 0.4f, _gridColor);

                Event e = Event.current;
                _mousePosition = e.mousePosition;
                UserInput(e);

                DrawWindows();

                GUI.backgroundColor = Color.white;

                EditorGUILayout.BeginHorizontal("TextArea");

                EditorGUILayout.SelectableLabel("Текущие : ", GUILayout.Height(22));

                if (GUILayout.Button("Данные сценария", GUILayout.Width(200))) Selection.activeObject = _storyData;

                if (_mainSettings == null) _mainSettings = (GameSettings)Resources.Load("BaseParameters");
                else
                {
                    if (GUILayout.Button("Данные", GUILayout.Width(100), GUILayout.Height(18))) Selection.activeObject = _mainSettings;
                }

                if (GUILayout.Button("Сохранить", GUILayout.Width(100), GUILayout.Height(18))) SaveData();

                EditorGUILayout.EndVertical();

            }
            else
            {
                EditorGUILayout.LabelField("Нет данных сюжета");
                EditorGUILayout.Space();
                _storyData = (StoryData)EditorGUILayout.ObjectField("Файл данных", _storyData, typeof(StoryData), false);
            }

        }

        /// <summary> Отрисовка всех нод </summary>
        private void DrawWindows()
        {
            _drag = Vector2.zero;

            try
            {
                BeginWindows();

                foreach (GamePart bn in _storyData.nodesData)
                {
                    if (bn != null)
                    {
                        bn.DrawCurve(_storyData.nodesData);
                        DrawConnectPoint(bn);
                        bn.DrawEvents();
                    }
                }

                for (int i = 0; i < _storyData.nodesData.Count; i++)
                {
                    if (_storyData.nodesData[i] != null)
                    {
                        GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f);

                        if (EditorApplication.isPlaying)
                        {
                            if (MoveController.thisPart != null)
                            {
                                if (MoveController.thisPart == _storyData.nodesData[i]) GUI.backgroundColor = new Color(0.5f, 0.5f, 0.75f);
                            }
                        }
                        else
                        {
                            switch (_storyData.nodesData[i].workStady)
                            {
                                case 0: GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f); ; break;
                                case 1: GUI.backgroundColor = Color.yellow; break;
                                case 2: GUI.backgroundColor = Color.green; break;
                            }

                            if (_sellectedToConnect != null)
                            {
                                if (_sellectedToConnect.Equals(_storyData.nodesData[i])) GUI.backgroundColor = Color.red;
                            }
                        }

                        _storyData.nodesData[i].windowRect = GUI.Window(
                         i,
                         _storyData.nodesData[i].windowRect,
                         DrawNodeWindow,
                         _storyData.nodesData[i].windowTitle);
                    }
                }

                EndWindows();
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return;
            }
        }

        #region CONNECTORS_WORK

        /// <summary> Отрисовка подключений и логика их работы </summary>
        private void DrawConnectPoint(GamePart bn)
        {
            if (bn is TextPart)
            {
                if (GUI.Button(bn.ConnectPosition(0), _connectTexture)) ConnectorClick(0, bn);
            }
            else if (bn is ChangePart)
            {
                if (GUI.Button(bn.ConnectPosition(0), _connectTexture)) ConnectorClick(0, bn);
                if (GUI.Button(bn.ConnectPosition(1), _connectTexture)) ConnectorClick(1, bn);
            }
            else if (bn is BattlePart)
            {
                if (GUI.Button(bn.ConnectPosition(0), _connectTexture)) ConnectorClick(0, bn);
                if (GUI.Button(bn.ConnectPosition(1), _connectTexture)) ConnectorClick(1, bn);
                if (GUI.Button(bn.ConnectPosition(2), _connectTexture)) ConnectorClick(2, bn);
            }
        }

        /// <summary> Нажатие на коннектор </summary>
        private void ConnectorClick(int id, GamePart part)
        {
            _sellectedToConnect = part;
            _sellectionId = id;
        }

        #endregion

        /// <summary> Отрисовка отдельной ноды </summary>
        private void DrawNodeWindow(int id)
        {
            _storyData.nodesData[id].DrawWindow();
            GUI.DragWindow();
        }

        /// <summary> Действия пользователя </summary>
        private void UserInput(Event e)
        {
            if (e.button == 1)
            {
                if (e.type == EventType.MouseDown) RightMouseClick(e);
            }

            if (e.button == 0)
            {
                if (e.type == EventType.MouseDown) LeftMouseClick();
            }

            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.Delete) DeleteKeyDown();
            }

            if (e.type == EventType.MouseDrag)
            {
                if (e.button == 2)
                {
                    OnDrag(e.delta);
                }
            }

            if(e.type == EventType.ScrollWheel)
            {
                for (int i = 0; i < _storyData.nodesData.Count; i++)
                {
                    _storyData.nodesData[i].SetWindowStady();
                }
                Repaint();
            }
        }

        /// <summary> Правый клик мыши </summary>
        private void RightMouseClick(Event e)
        {
            _selectedNode = null;
            _isClickOnWindow = false;

            for (int i = 0; i < _storyData.nodesData.Count; i++)
            {
                if (_storyData.nodesData[i] != null)
                {
                    if (_storyData.nodesData[i].windowRect.Contains(_mousePosition))
                    {
                        _isClickOnWindow = true;
                        _selectedNode = _storyData.nodesData[i];
                        Selection.activeObject = _storyData.nodesData[i];
                        break;
                    }
                }
            }

            if (!_isClickOnWindow) AddNewNode(e);
            else AddEventToPart(e);
        }

        /// <summary> Левый клик мыши </summary>
        private void LeftMouseClick()
        {
            for (int i = 0; i < _storyData.nodesData.Count; i++)
            {
                if (_storyData.nodesData[i] != null)
                {
                    if (_storyData.nodesData[i].windowRect.Contains(_mousePosition))
                    {
                        if (_sellectedToConnect != null)
                        {
                            switch (_sellectionId)
                            {
                                case 0:
                                    _sellectedToConnect.movePart_1 = _storyData.nodesData[i];
                                    _sellectedToConnect = null;
                                    break;

                                case 1:
                                    _sellectedToConnect.movePart_2 = _storyData.nodesData[i];
                                    _sellectedToConnect = null;
                                    break;

                                case 2:
                                    _sellectedToConnect.movePart_3 = _storyData.nodesData[i];
                                    _sellectedToConnect = null;
                                    break;
                            }
                        }
                        else
                        {
                            _selectedNode = _storyData.nodesData[i];
                            Selection.activeObject = _storyData.nodesData[i];
                        }
                        break;
                    }
                }
            }
        }

        /// <summary> Панорамирование поля </summary>
        private void OnDrag(Vector2 delta)
        {
            _drag = delta;

            if (_storyData.nodesData != null)
            {
                for (int i = 0; i < _storyData.nodesData.Count; i++)
                {
                    _storyData.nodesData[i].windowRect.position += _drag;
                    _offset += delta / _storyData.nodesData.Count;
                }
            }

            GUI.changed = true;
        }

        /// <summary> Нажатие на Delete </summary>
        private void DeleteKeyDown()
        {
            if (_selectedNode != null)
            {
                _storyData.nodesData.Remove(_selectedNode);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_selectedNode));
                _selectedNode = null;
                SaveData();
                Repaint();
            }
        }

        /// <summary> Проверка действия </summary>
        private void AddNodeToWindow(object o)
        {
            Object[] loadedObj = Resources.LoadAll("GameParts", typeof(GamePart));

            UserActions a = (UserActions)o;

            string nameNode = loadedObj.Length.ToString();
            string pathToNode = "Assets/Resources/GameParts/";
            bool sizeStady = false;
            Rect nodeRect;

            switch (a)
            {
                case UserActions.ADD_TEXT_PART: nameNode += "_TextPart"; break;
                case UserActions.ADD_CHANGE_PART: nameNode += "_ChangePart"; break;
                case UserActions.ADD_BATTLE_PART: nameNode += "_BattlePart"; break;
                case UserActions.ADD_MAZE_PART: nameNode += "_PazzlePart"; break;
                case UserActions.ADD_EVENT_PART: nameNode += "_EventPart"; break;
                case UserActions.ADD_FINAL_PART: nameNode += "_FinalPart"; break;
                case UserActions.ADD_LABEL_PART: nameNode += "_LeandPart"; break;
                case UserActions.ADD_SLIDESHOW_PART: nameNode += "_MoviePart"; break;
                case UserActions.ADD_TRANSIT: nameNode += ""; break;
            }

            pathToNode += nameNode + ".asset";

            if (_storyData.nodesData != null)
            {
                if(_storyData.nodesData[0] != null) sizeStady = _storyData.nodesData[0].windowSizeStady;
            }

            if (sizeStady) nodeRect = new Rect(_mousePosition.x, _mousePosition.y, 40, 38);
            else nodeRect = new Rect(_mousePosition.x, _mousePosition.y, 120, 40);

            switch (a)
            {
                case UserActions.ADD_TEXT_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(TextPart)), pathToNode);
                    TextPart textPart = (TextPart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(TextPart));

                    textPart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        textPart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        textPart._memberTitle = nameNode;
                    }
                    else textPart.windowTitle = nameNode;

                    textPart.windowRect = nodeRect;
                    _storyData.nodesData.Add(textPart);
                    break;

                case UserActions.ADD_CHANGE_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(ChangePart)), pathToNode);
                    ChangePart changePart = (ChangePart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(ChangePart));

                    changePart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        changePart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        changePart._memberTitle = nameNode;
                    }
                    else changePart.windowTitle = nameNode;

                    changePart.windowRect = nodeRect;
                    _storyData.nodesData.Add(changePart);
                    break;

                case UserActions.ADD_BATTLE_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(BattlePart)), pathToNode);
                    BattlePart battlePart = (BattlePart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(BattlePart));

                    battlePart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        battlePart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        battlePart._memberTitle = nameNode;
                    }
                    else battlePart.windowTitle = nameNode;

                    battlePart.windowRect = nodeRect;
                    _storyData.nodesData.Add(battlePart);
                    break;

                case UserActions.ADD_MAZE_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(PazzlePart)), pathToNode);
                    PazzlePart pazzlePart = (PazzlePart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(PazzlePart));

                    pazzlePart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        pazzlePart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        pazzlePart._memberTitle = nameNode;
                    }
                    else pazzlePart.windowTitle = nameNode;

                    pazzlePart.windowRect = nodeRect;
                    _storyData.nodesData.Add(pazzlePart);
                    break;

                case UserActions.ADD_EVENT_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(EventPart)), pathToNode);
                    EventPart eventPart = (EventPart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(EventPart));

                    eventPart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        eventPart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        eventPart._memberTitle = nameNode;
                    }
                    else eventPart.windowTitle = nameNode;

                    eventPart.windowRect = nodeRect;
                    _storyData.nodesData.Add(eventPart);
                    break;

                case UserActions.ADD_FINAL_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(FinalPart)), pathToNode);
                    FinalPart finalPart = (FinalPart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(FinalPart));

                    finalPart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        finalPart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        finalPart._memberTitle = nameNode;
                    }
                    else finalPart.windowTitle = nameNode;

                    finalPart.windowRect = nodeRect;
                    _storyData.nodesData.Add(finalPart);
                    break;

                case UserActions.ADD_LABEL_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(LeandPart)), pathToNode);
                    LeandPart leandPart = (LeandPart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(LeandPart));

                    leandPart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        leandPart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        leandPart._memberTitle = nameNode;
                    }
                    else leandPart.windowTitle = nameNode;

                    leandPart.windowRect = nodeRect;
                    _storyData.nodesData.Add(leandPart);
                    break;

                case UserActions.ADD_SLIDESHOW_PART:
                    AssetDatabase.CreateAsset(CreateInstance(typeof(MoviePart)), pathToNode);
                    MoviePart moviePart = (MoviePart)AssetDatabase.LoadAssetAtPath(pathToNode, typeof(MoviePart));

                    moviePart.windowSizeStady = sizeStady;
                    if (sizeStady)
                    {
                        moviePart.windowTitle = nameNode.Substring(0, GetShortNameNode(nameNode));
                        moviePart._memberTitle = nameNode;
                    }
                    else moviePart.windowTitle = nameNode;

                    moviePart.windowRect = nodeRect;
                    _storyData.nodesData.Add(moviePart);
                    break;

                case UserActions.ADD_TRANSIT: break;
            }

            SaveData();
        }

        /// <summary> Насколько сокращать имя </summary>
        private int GetShortNameNode(string longName)
        {
            for (int i = 0; i < 4; i++)
            {
                if (longName[i] == '_') return i;
            }

            return 4;
        }

        /// <summary> Проверка действия добавления евента </summary>
        private void AddEventMethod(object o)
        {
            AddEventActions a = (AddEventActions)o;

            string path = "Assets/Resources/GameEvents/";
            string nameEvent = _selectedNode.mainEvents.Count.ToString();

            switch (a)
            {
                case AddEventActions.CHECK_DECISION: nameEvent += "_CheckDecision.asset"; break;
                case AddEventActions.CHECK_PLAYER_INFL: nameEvent += "_CheckPlayerInfl.asset"; break;
                case AddEventActions.CHECK_POINT: nameEvent += "_CheckPoint.asset"; break;
                case AddEventActions.EFFECT_INTERACT: nameEvent += "_EffectInteract.asset"; break;
                case AddEventActions.IMPORTANT_DECISION: nameEvent += "_ImportantDecision.asset"; break;
                case AddEventActions.ITEM_INFL: nameEvent += "_ItemInfl.asset"; break;
                case AddEventActions.ITEM_INTERACT: nameEvent += "_ItemInteract.asset"; break;
                case AddEventActions.LOCATION_FIND: nameEvent += "_LocationFind.asset"; break;
                case AddEventActions.MEMBER_TIME: nameEvent += "_MemberTime.asset"; break;
                case AddEventActions.NON_PLAYER_INFL: nameEvent += "_NonPlayerInfl.asset"; break;
                case AddEventActions.PLAYER_INFL: nameEvent += "_PlayerInfl.asset"; break;
                case AddEventActions.RANDOM_PART: nameEvent += "_RandomPart.asset"; break;
            }
            
            switch (a)
            {
                case AddEventActions.CHECK_DECISION: AssetDatabase.CreateAsset(CreateInstance(typeof(CheckDecision)), path + nameEvent);
                    _selectedNode.mainEvents.Add((CheckDecision)AssetDatabase.LoadAssetAtPath(path + nameEvent,typeof(CheckDecision)));
                    break;
            
                case AddEventActions.CHECK_PLAYER_INFL: AssetDatabase.CreateAsset(CreateInstance(typeof(CheckPlayerInfl)), path + nameEvent);
                    _selectedNode.mainEvents.Add((CheckPlayerInfl)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(CheckPlayerInfl)));
                    break;
            
                case AddEventActions.CHECK_POINT: AssetDatabase.CreateAsset(CreateInstance(typeof(CheckPoint)), path + nameEvent);
                    _selectedNode.mainEvents.Add((CheckPoint)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(CheckPoint)));
                    break;
            
                case AddEventActions.EFFECT_INTERACT: AssetDatabase.CreateAsset(CreateInstance(typeof(EffectInteract)), path + nameEvent);
                    _selectedNode.mainEvents.Add((EffectInteract)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(EffectInteract)));
                    break;
            
                case AddEventActions.IMPORTANT_DECISION: AssetDatabase.CreateAsset(CreateInstance(typeof(ImportantDecision)), path + nameEvent);
                    _selectedNode.mainEvents.Add((ImportantDecision)AssetDatabase.LoadAssetAtPath(path + nameEvent,typeof(ImportantDecision)));
                    break;
            
                case AddEventActions.ITEM_INFL: AssetDatabase.CreateAsset(CreateInstance(typeof(ItemInfl)), path + nameEvent);
                    _selectedNode.mainEvents.Add((ItemInfl)AssetDatabase.LoadAssetAtPath(path + nameEvent,typeof(ItemInfl)));
                    break;
            
                case AddEventActions.ITEM_INTERACT: AssetDatabase.CreateAsset(CreateInstance(typeof(ItemInteract)), path + nameEvent);
                    _selectedNode.mainEvents.Add((ItemInteract)AssetDatabase.LoadAssetAtPath(path + nameEvent,typeof(ItemInteract)));
                    break;
            
                case AddEventActions.LOCATION_FIND: AssetDatabase.CreateAsset(CreateInstance(typeof(LocationFind)), path + nameEvent);
                    _selectedNode.mainEvents.Add((LocationFind)AssetDatabase.LoadAssetAtPath(path + nameEvent,typeof(LocationFind)));
                    break;
            
                case AddEventActions.MEMBER_TIME: AssetDatabase.CreateAsset(CreateInstance(typeof(MemberTime)), path + nameEvent);
                    _selectedNode.mainEvents.Add((MemberTime)AssetDatabase.LoadAssetAtPath(path + nameEvent,typeof(MemberTime)));
                    break;
            
                case AddEventActions.NON_PLAYER_INFL: AssetDatabase.CreateAsset(CreateInstance(typeof(NonPlayerInfl)), path + nameEvent);
                    _selectedNode.mainEvents.Add((NonPlayerInfl)AssetDatabase.LoadAssetAtPath(path + nameEvent,typeof(NonPlayerInfl)));
                    break;
            
                case AddEventActions.PLAYER_INFL: AssetDatabase.CreateAsset(CreateInstance(typeof(PlayerInfl)), path + nameEvent);
                    _selectedNode.mainEvents.Add((PlayerInfl)AssetDatabase.LoadAssetAtPath(path + nameEvent,typeof(PlayerInfl)));
                    break;
            
                case AddEventActions.RANDOM_PART: AssetDatabase.CreateAsset(CreateInstance(typeof(RandomPart)), path + nameEvent);
                    _selectedNode.mainEvents.Add((RandomPart)AssetDatabase.LoadAssetAtPath(path + nameEvent, typeof(RandomPart)));
                    break;
            }
        }

        #endregion

        #region HELPERS

        /// <summary> Создать новую ноду </summary>
        private void AddNewNode(Event e)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Создать главу/Текстовая"), false, AddNodeToWindow, UserActions.ADD_TEXT_PART);
            menu.AddItem(new GUIContent("Создать главу/Выбора"), false, AddNodeToWindow, UserActions.ADD_CHANGE_PART);
            menu.AddItem(new GUIContent("Создать главу/Боя"), false, AddNodeToWindow, UserActions.ADD_BATTLE_PART);
            menu.AddItem(new GUIContent("Создать главу/Загадка"), false, AddNodeToWindow, UserActions.ADD_MAZE_PART);
            menu.AddItem(new GUIContent("Создать главу/Эвент"), false, AddNodeToWindow, UserActions.ADD_EVENT_PART);
            menu.AddItem(new GUIContent("Создать главу/Финальная"), false, AddNodeToWindow, UserActions.ADD_FINAL_PART);
            menu.AddItem(new GUIContent("Создать главу/Вставка"), false, AddNodeToWindow, UserActions.ADD_LABEL_PART);
            menu.AddItem(new GUIContent("Создать главу/Слайдшоу"), false, AddNodeToWindow, UserActions.ADD_SLIDESHOW_PART);
            menu.ShowAsContext();
            e.Use();
        }

        /// <summary> Добавить событие к главе </summary>
        private void AddEventToPart(Event e)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Добавить событие/Контрольная точка"), false, AddEventMethod, AddEventActions.CHECK_POINT);
            menu.AddItem(new GUIContent("Добавить событие/Важное решение"), false, AddEventMethod, AddEventActions.IMPORTANT_DECISION);
            menu.AddItem(new GUIContent("Добавить событие/Проверка решения"), false, AddEventMethod, AddEventActions.CHECK_DECISION);
            menu.AddItem(new GUIContent("Добавить событие/Влияние на игрока"), false, AddEventMethod, AddEventActions.PLAYER_INFL);
            menu.AddItem(new GUIContent("Добавить событие/Влияние на НПС"), false, AddEventMethod, AddEventActions.NON_PLAYER_INFL);
            menu.AddItem(new GUIContent("Добавить событие/Проверка влияния персонажа"), false, AddEventMethod, AddEventActions.CHECK_PLAYER_INFL);
            menu.AddItem(new GUIContent("Добавить событие/Взаимодействие с эффектом"), false, AddEventMethod, AddEventActions.EFFECT_INTERACT);
            menu.AddItem(new GUIContent("Добавить событие/Взаимодействие с предметом"), false, AddEventMethod, AddEventActions.ITEM_INTERACT);
            menu.AddItem(new GUIContent("Добавить событие/Использование предмета"), false, AddEventMethod, AddEventActions.ITEM_INFL);
            menu.AddItem(new GUIContent("Добавить событие/Найдена локация"), false, AddEventMethod, AddEventActions.LOCATION_FIND);
            menu.AddItem(new GUIContent("Добавить событие/Воспоминание"), false, AddEventMethod, AddEventActions.MEMBER_TIME);
            menu.AddItem(new GUIContent("Добавить событие/Случайный переход"), false, AddEventMethod, AddEventActions.RANDOM_PART);
            menu.ShowAsContext();
            e.Use();
        }

        /// <summary> Сохранить данные </summary>
        private void SaveData()
        {
            for (int i = 0; i < _storyData.nodesData.Count; i++)
            {
                if (_storyData.nodesData[i] == null)
                {
                    _storyData.nodesData.RemoveAt(i);
                    SaveData();
                    return;
                }
            }

            for (int i = 0; i < _storyData.nodesData.Count; i++)
            {
                EditorUtility.SetDirty(_storyData.nodesData[i]);
            }

            EditorUtility.SetDirty(_storyData);
        }

        /// <summary> Отрисовка сетки </summary>
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            _offset += _drag * 0.5f;
            Vector3 newOffset = new Vector3(_offset.x % gridSpacing, _offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        #endregion
    }
}