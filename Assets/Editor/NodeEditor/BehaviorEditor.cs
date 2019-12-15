using UnityEngine;
using UnityEditor;

namespace GUIInspector.NodeEditor
{
    public class BehaviorEditor : EditorWindow
    {
        #region VARIABLES

        public bool _drawWindow;
        private Vector2 _distDifference;

        private Color _gridColor = new Color(0.2f, 0.2f, 0.2f); // Цвет сетки
        public static Vector2 _offset; // Отступ поля
        private Vector2 _drag; // Отступ нод

        private GameSettings _mainSettings; // Настройки поля
        public static BehaviorEditor trBehaviorEditor; // ссылка на себя
        public static StoryData storyData; // Данные сюжета
        private StoryData _storyData;
        public static Vector3 _mousePosition; // Позиция мыши
        private bool _isClickOnWindow; // Нажал на окно или нет
        private GamePart _selectedNode; // Выбранная нода
        private Texture _emptyTexture;

        private GamePart _sellectedToConnect; // Нода в памяти при подключении полключения
        private int _sellectionId; // Идентификатор типа подключения

        public int[] workStadyNum = new int[] { 0, 1, 2 };
        public string[] workStadyNames = new string[] { "Пусто", "Разработка", "Готово" };
        public int tempConnect = 0;

        #endregion

        #region INIT

        [MenuItem("Story/Node Editor")]
        [System.Obsolete]
        public static void ShowEditor()
        {
            BehaviorEditor editor = GetWindow<BehaviorEditor>();
            editor.title = "Node Editor";
            editor._gridColor = new Color(0.2f, 0.2f, 0.2f);
            trBehaviorEditor = editor;

            editor._emptyTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeEditor/Images/Connect.png", typeof(Texture));
        }

        #endregion

        #region GUI_METHODS

        private void OnFocus()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPaused = true;
            }
            if (EventEditor.eventEditor == null) EventEditor.eventEditor = (EventEditor)CreateInstance(typeof(EventEditor));

            _drawWindow = true;
        }

        private void OnLostFocus()
        {
            if (EditorApplication.isPaused) EditorApplication.isPaused = false;
        }

        private void OnGUI()
        {
            if (_drawWindow) DrawGUI();
        }

        /// <summary> Отрисовка интерфейса </summary>
        private void DrawGUI()
        {
            if (_storyData != null)
            {
                // Подключение связей

                if (storyData == null) storyData = _storyData;
                if (trBehaviorEditor == null) trBehaviorEditor = this;

                EditorGUILayout.BeginVertical(_storyData.graphSkin.GetStyle("Box"), GUILayout.Width(Screen.width), GUILayout.Height(Screen.height));

                DrawGrid(10, 0.2f, _gridColor);
                DrawGrid(50, 0.4f, _gridColor);

                Event e = Event.current;
                _mousePosition = e.mousePosition;

                // Отображение

                if (EventEditor.eventGraph == null)
                {
                    UserInput(e);
                    GUI.Label(new Rect(Screen.width - 300, Screen.height - 50, 300, 50),
                    storyData.name,
                    storyData.graphSkin.GetStyle("Label"));
                    DrawWindows();
                    GUI.backgroundColor = Color.white;
                }
                else
                {
                    EventEditor.eventThis = e;
                    GUI.Label(new Rect(Screen.width - 300, Screen.height - 50, 300, 50),
                    EventEditor.eventGraph.name,
                    storyData.graphSkin.GetStyle("Label"));
                    EventEditor.ShowWindow();
                }

                // Верхняя панель

                EditorGUILayout.BeginHorizontal("TextArea");
                if (EventEditor.eventGraph == null) EditorGUILayout.LabelField("Сценарий", GUILayout.Height(22));
                else
                {
                    EditorGUILayout.LabelField("Евент " + EventEditor.eventGraph.name, GUILayout.Height(22));
                    if (GUILayout.Button("Вернуться", GUILayout.Width(100), GUILayout.Height(18))) EventEditor.eventGraph = null;
                }

                if (GUILayout.Button("Сценарий", GUILayout.Width(100))) Selection.activeObject = _storyData;

                if (_mainSettings == null) _mainSettings = (GameSettings)Resources.Load("BaseParameters");
                else
                {
                    if (GUILayout.Button("Данные", GUILayout.Width(100), GUILayout.Height(18))) Selection.activeObject = _mainSettings;
                }

                if (GUILayout.Button("Сохранить", GUILayout.Width(100), GUILayout.Height(18))) SaveData();

                EditorGUILayout.EndHorizontal();
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

            BeginWindows();

            int idWindow = 0;

            foreach (GamePart part in _storyData.nodesData)
            {
                GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f);

                if (EditorApplication.isPlaying)
                {
                    if (AnimController.thisPart != null)
                    {
                        if (AnimController.thisPart == part)
                        {
                            FocusPart(part);
                            GUI.backgroundColor = Color.blue;
                        }
                    }
                }
                else
                {
                    switch (part.workStady)
                    {
                        case 0: GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f); ; break;
                        case 1: GUI.backgroundColor = Color.yellow; break;
                        case 2: GUI.backgroundColor = Color.green; break;
                    }

                    if (_sellectedToConnect != null)
                    {
                        if (_sellectedToConnect.Equals(part))
                        {
                            CreateCurve(ConnectPosition(part, tempConnect),
                                new Rect(_mousePosition, new Vector2(0, 0)), Color.blue);
                            Repaint();
                        }
                    }
                }

                part.windowRect = GUI.Window(
                    idWindow,
                    part.windowRect,
                    DrawNodeWindow,
                    part.windowTitle, storyData.graphSkin.GetStyle("Window"));

                GUI.backgroundColor = Color.white;

                if (!part.Equals(_selectedNode))
                {
                    DrawConnectPoint(part);
                    DrawEvents(part);
                    DrawCurve(part);
                }

                idWindow++;

            }

            EndWindows();

            if (_selectedNode != null)
            {
                DrawConnectPoint(_selectedNode);
                DrawEvents(_selectedNode);
                DrawCurve(_selectedNode);
            }
        }

        /// <summary> Сфокусироваться на главе </summary>
        private void FocusPart(GamePart part)
        {
            _distDifference = new Vector2(Screen.width / 2, Screen.height / 2);
            _distDifference -= part.windowRect.position;

            OnDrag(_distDifference);
        }

        #region CONNECTORS_WORK

        /// <summary> Отрисовка подключений и логика их работы </summary>
        private void DrawConnectPoint(GamePart bn)
        {
            if (bn is TextPart)
            {
                if (GUI.Button(ConnectPosition(bn, 0), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(0, bn);
                    tempConnect = 0;
                }
            }
            else if (bn is ChangePart)
            {
                if (GUI.Button(ConnectPosition(bn, 0), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(0, bn);
                    tempConnect = 0;
                }

                if (GUI.Button(ConnectPosition(bn, 1), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(1, bn);
                    tempConnect = 1;
                }
            }
            else if (bn is BattlePart)
            {
                if (GUI.Button(ConnectPosition(bn, 0), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(0, bn);
                    tempConnect = 0;
                }

                if (GUI.Button(ConnectPosition(bn, 1), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(1, bn);
                    tempConnect = 1;
                }

                if (GUI.Button(ConnectPosition(bn, 2), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(2, bn);
                    tempConnect = 2;
                }
            }
            else if (bn is LeandPart)
            {
                if (GUI.Button(ConnectPosition(bn, 0), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(0, bn);
                    tempConnect = 0;
                }
            }

            else if (bn is EventPart)
            {
                if (GUI.Button(ConnectPosition(bn, 0), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(0, bn);
                    tempConnect = 0;
                }

                if (GUI.Button(ConnectPosition(bn, 2), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(2, bn);
                    tempConnect = 2;
                }
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
            if (!_storyData.nodesData[id].windowSizeStady)
            {
                EditorGUILayout.BeginHorizontal();
                _storyData.nodesData[id].workStady = EditorGUILayout.IntPopup(
                    _storyData.nodesData[id].workStady,
                    workStadyNames, workStadyNum,
                    GUILayout.Width(85f));
                _storyData.nodesData[id].isShowComment = EditorGUILayout.Toggle(
                    _storyData.nodesData[id].isShowComment);
                EditorGUILayout.EndHorizontal();

                if (_storyData.nodesData[id].isShowComment)
                {
                    _storyData.nodesData[id].windowRect.height = _storyData.baseNodeCommentHeight;
                    _storyData.nodesData[id].comment = EditorGUILayout.TextArea(
                        _storyData.nodesData[id].comment, _storyData.graphSkin.GetStyle("TextArea"),
                        GUILayout.Width(100f),
                        GUILayout.Height(70));
                }
                else _storyData.nodesData[id].windowRect.height = _storyData.baseNodeLgHeight;
            }

            GUI.DragWindow();
        }

        /// <summary> Действия пользователя </summary>
        private void UserInput(Event e)
        {
            if (e.button == 1) { if (e.type == EventType.MouseDown) RightMouseClick(e); }
            if (e.button == 0) { if (e.type == EventType.MouseDown) LeftMouseClick(); }
            if (e.type == EventType.KeyDown) { if (e.keyCode == KeyCode.Delete) DeleteKeyDown(); }
            if (e.type == EventType.MouseDrag) { if (e.button == 2) OnDrag(e.delta); }

            if (e.type == EventType.ScrollWheel)
            {
                if (e.delta.y > 0) SetWindowStady(true);
                else SetWindowStady(false);
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
                        GraphChangeController.selectedNode = _storyData.nodesData[i];
                        Selection.activeObject = _storyData.nodesData[i];
                        break;
                    }
                }
            }

            if (!_isClickOnWindow) GraphChangeController.AddNewNode(e);
            else GraphChangeController.AddEventToPart(e);
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
                                case 0: _sellectedToConnect.movePart_1 = _storyData.nodesData[i]; break;
                                case 1: _sellectedToConnect.movePart_2 = _storyData.nodesData[i]; break;
                                case 2: _sellectedToConnect.movePart_3 = _storyData.nodesData[i]; break;
                            }

                            _sellectedToConnect = null;
                        }
                        else
                        {
                            _selectedNode = _storyData.nodesData[i];
                            GraphChangeController.selectedNode = _storyData.nodesData[i];
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
                    _offset += _drag / _storyData.nodesData.Count;
                }
            }

            GUI.changed = true;
        }

        /// <summary> Нажатие на Delete </summary>
        private void DeleteKeyDown()
        {
            if (_selectedNode != null)
            {
                if (_selectedNode is EventPart evPr)
                {
                    if (AssetDatabase.IsValidFolder("Assets/Resources/GameParts/" + evPr.name))
                    {
                        for (int i = 0; i < evPr.eventParts.Count; i++)
                        {
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(evPr.eventParts[i]));
                        }

                        AssetDatabase.DeleteAsset("Assets/Resources/GameParts/" + evPr.name);
                    }
                }

                _storyData.nodesData.Remove(_selectedNode);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_selectedNode));
                _selectedNode = null;
                GraphChangeController.selectedNode = null;
                
                _drawWindow = true;
                Repaint();
                SaveData();
            }
        }

        #endregion

        #region NODE_DRAW

        /// <summary> Отрисовка связей </summary>
        public void DrawCurve(GamePart partNode)
        {
            Color baseConnectColor = new Color(1, 1, 1, 0.75f);

            if (partNode.movePart_1 != null)
            {
                if (storyData.nodesData.Contains(partNode.movePart_1) && partNode.movePart_1 != this)
                    CreateCurve(ConnectPosition(partNode, 0), partNode.movePart_1.windowRect, baseConnectColor);
            }

            if (partNode.movePart_2 != null)
            {
                if (storyData.nodesData.Contains(partNode.movePart_2) && partNode.movePart_2 != this)
                    CreateCurve(ConnectPosition(partNode, 1), partNode.movePart_2.windowRect, baseConnectColor);
            }

            if (partNode.movePart_3 != null)
            {
                if (storyData.nodesData.Contains(partNode.movePart_3) && partNode.movePart_3 != this)
                    CreateCurve(ConnectPosition(partNode, 2), partNode.movePart_3.windowRect, baseConnectColor);
            }

            DrawRandomCurve(partNode);
        }

        /// <summary> Отрисовка связей Random Event </summary>
        private void DrawRandomCurve(GamePart partNode)
        {
            if (partNode.mainEvents != null)
            {
                bool checkRandom = false;
                RandomPart randomEvent = null;

                for (int i = 0; i < partNode.mainEvents.Count; i++)
                {
                    if (partNode.mainEvents[i] is RandomPart)
                    {
                        checkRandom = true;
                        randomEvent = (RandomPart)partNode.mainEvents[i];
                    }
                }

                if (checkRandom)
                {
                    Color randomConnectColor = new Color(1f, 0, 0, 0.75f);

                    if (randomEvent.part_1_random != null && randomEvent.part_1_random != this)
                    {
                        CreateCurve(ConnectPosition(partNode, 0), randomEvent.part_1_random.windowRect, randomConnectColor);
                    }

                    if (randomEvent.part_2_random != null && randomEvent.part_2_random != this)
                    {
                        CreateCurve(ConnectPosition(partNode, 1), randomEvent.part_2_random.windowRect, randomConnectColor);
                    }

                    if (randomEvent.part_3_random != null && randomEvent.part_3_random != this)
                    {
                        CreateCurve(ConnectPosition(partNode, 2), randomEvent.part_3_random.windowRect, randomConnectColor);
                    }
                }
            }
        }

        /// <summary> Связь </summary>
        private void CreateCurve(Rect start, Rect end, Color colorCurve)
        {
            Vector3 startPos = new Vector3(
                start.x + start.width,
                start.y + (start.height * .5f),
                0);

            Vector3 endPos = new Vector3(
                end.x,
                end.y + (end.height * .5f),
                0);

            // Vector3 startTan = startPos + Vector3.right * 40;
            // Vector3 endTan = endPos + Vector3.left * 40;


            Vector3 startTan = startPos + Vector3.right * (Vector3.Distance(startPos, endPos) * 0.6f);
            Vector3 endTan = endPos + Vector3.left * (Vector3.Distance(startPos, endPos) * 0.6f);

            Handles.DrawBezier(startPos, endPos, startTan, endTan, colorCurve, null, 3f);
        }

        /// <summary> Позиция подключения следующей главы </summary>
        public Rect ConnectPosition(GamePart partNode, int id)
        {
            Rect nodeConnectPosition = new Rect(
                    partNode.windowRect.x + partNode.windowRect.width + 5,
                    (partNode.windowRect.y + 2) + (13 * id),
                    11,
                    11);

            return nodeConnectPosition;
        }

        /// <summary> Отрисовка евентов </summary>
        public void DrawEvents(GamePart partNode)
        {
            int sizeEvent;

            if (partNode.windowSizeStady) sizeEvent = 7;
            else sizeEvent = 20;

            GUIStyle st = new GUIStyle();

            if (partNode.mainEvents != null)
            {
                for (int i = 0; i < partNode.mainEvents.Count; i++)
                {
                    if (i < 6)
                    {
                        GUI.Box(new Rect(
                    partNode.windowRect.x + (sizeEvent * i),
                    partNode.windowRect.y + partNode.windowRect.height,
                    sizeEvent,
                    sizeEvent),
                    GetEventTextures(partNode.mainEvents[i]), st);
                    }
                    else
                    {
                        if (i < 11)
                        {
                            GUI.Box(new Rect(
                        partNode.windowRect.x + (sizeEvent * (i - 6)),
                        partNode.windowRect.y + partNode.windowRect.height + sizeEvent,
                        sizeEvent,
                        sizeEvent),
                        GetEventTextures(partNode.mainEvents[i]), st);
                        }
                    }
                }
            }
        }

        /// <summary> Получить текстуру эвента </summary>
        private Texture GetEventTextures(GameEvent crEvent)
        {
            Texture eventIco;
            string pathToIco;

            if (crEvent is CheckDecision) pathToIco = "Assets/Editor/NodeEditor/Images/EventsIco/CheckDecision.png";
            else if (crEvent is CheckPlayerInfl) pathToIco = "Assets/Editor/NodeEditor/Images/EventsIco/CheckPlayerInfl.png";
            else if (crEvent is CheckPoint) pathToIco = "Assets/Editor/NodeEditor/Images/EventsIco/CheckPoint.png";
            else if (crEvent is EffectInteract) pathToIco = "Assets/Editor/NodeEditor/Images/EventsIco/EffectInteract.png";
            else if (crEvent is ImportantDecision) pathToIco = "Assets/Editor/NodeEditor/Images/EventsIco/ImportantDecision.png";
            else if (crEvent is ItemInfl) pathToIco = "Assets/Editor/NodeEditor/Images/EventsIco/ItemInfl.png";
            else if (crEvent is ItemInteract) pathToIco = "Assets/Editor/NodeEditor/Images/EventsIco/ItemInteract.png";
            else if (crEvent is NonPlayerInfl) pathToIco = "Assets/Editor/NodeEditor/Images/EventsIco/NonPlayerInfl.png";
            else if (crEvent is PlayerInfl) pathToIco = "Assets/Editor/NodeEditor/Images/EventsIco/PlayerInfl.png";
            else if (crEvent is LocationFind) pathToIco = "Assets/Editor/NodeEditor/Images/EventsIco/LocationFind.png";
            else if (crEvent is MemberTime) pathToIco = "Assets/Editor/NodeEditor/Images/EventsIco/MemberTime.png";
            else if (crEvent is RandomPart) pathToIco = "Assets/Editor/NodeEditor/Images/EventsIco/RandomPart.png";
            else return null;

            eventIco = (Texture)AssetDatabase.LoadAssetAtPath(pathToIco, typeof(Texture));
            return eventIco;
        }

        /// <summary> Масштаб окна </summary>
        public void SetWindowStady(bool stady)
        {
            foreach (GamePart partNode in _storyData.nodesData)
            {
                if (partNode.windowSizeStady != stady)
                {
                    partNode.windowSizeStady = stady;

                    if (partNode.windowSizeStady)
                    {
                        partNode.windowRect.width = 40;
                        partNode.windowRect.height = 38;

                        partNode.windowRect.x /= 2f;
                        partNode.windowRect.y /= 2f;

                        partNode.windowRect.x += Screen.width / 4f;
                        partNode.windowRect.y += Screen.height / 4f;

                        partNode._memTitle = partNode.windowTitle;
                        partNode.windowTitle = partNode.windowTitle.Substring(0, GetShortNameNode(partNode.windowTitle));

                        partNode.memberComment = partNode.isShowComment;

                        if (partNode.isShowComment) partNode.isShowComment = false;
                    }
                    else
                    {
                        partNode.windowRect.width = 120;
                        partNode.windowRect.height = 40;

                        partNode.windowRect.x *= 2f;
                        partNode.windowRect.y *= 2f;

                        partNode.windowRect.x -= Screen.width / 2f;
                        partNode.windowRect.y -= Screen.height / 2f;

                        partNode.windowTitle = partNode._memTitle;

                        partNode.isShowComment = partNode.memberComment;
                    }
                }
            }
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

        /// <summary> Сохранить данные </summary>
        public static void SaveData()
        {
            for (int i = 0; i < storyData.nodesData.Count; i++)
            {
                if (storyData.nodesData[i] == null)
                {
                    storyData.nodesData.RemoveAt(i);
                    SaveData();
                    return;
                }
            }

            for (int i = 0; i < storyData.nodesData.Count; i++)
            {
                EditorUtility.SetDirty(storyData.nodesData[i]);
            }

            EditorUtility.SetDirty(storyData);
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

            Handles.color = Color.gray;
            Handles.EndGUI();
        }

        #endregion
    }
}