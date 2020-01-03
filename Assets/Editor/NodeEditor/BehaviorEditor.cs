using System.IO;
using Data;
using Data.GameEvents;
using Data.GameParts;
using Helpers;
using UnityEditor;
using UnityEngine;

namespace Editor.NodeEditor
{
    public class BehaviorEditor : EditorWindow
    {
        #region VARIABLES

        public bool drawWindow;
        private Vector2 _distDifference;

        private Color _gridColor = new Color(0.2f, 0.2f, 0.2f); // Цвет сетки
        public static Vector2 offset; // Отступ поля
        private Vector2 _drag; // Отступ нод

        private GameSettings _mainSettings; // Настройки поля
        public static BehaviorEditor trBehaviorEditor; // ссылка на себя
        public static StoryData storyData; // Данные сюжета
        private StoryData _storyData;
        public static Vector3 mousePosition; // Позиция мыши
        private bool _isClickOnWindow; // Нажал на окно или нет
        private GamePart _selectedNode; // Выбранная нода
        private Texture _emptyTexture;

        private GamePart _sellectedToConnect; // Нода в памяти при подключении полключения
        private int _sellectionId; // Идентификатор типа подключения

        public int[] workStadyNum = new int[] { 0, 1, 2 };
        public string[] workStadyNames = new string[] { "Пусто", "Разработка", "Готово" };

        public int debugStady;
        public int[] debugNum = new int[] { 0, 1 };
        public string[] debugNumNames = new string[] { "Разработка", "Проверка"};

        public bool isWatchOnPlay;

        public int tempConnect = 0;

        #endregion

        #region INIT

        [MenuItem("Story/Node Editor")]
        public static void ShowEditor()
        {
            BehaviorEditor editor = GetWindow<BehaviorEditor>();
            editor.titleContent.text = "Node Editor";
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

            drawWindow = true;
        }

        private void OnLostFocus()
        {
            if (EditorApplication.isPaused) EditorApplication.isPaused = false;
        }

        private void OnGUI()
        {
            if (drawWindow) DrawGui();
        }

        /// <summary> Отрисовка интерфейса </summary>
        private void DrawGui()
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
                mousePosition = e.mousePosition;

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
                    EditorGUILayout.LabelField("Эвент " + EventEditor.eventGraph.name, GUILayout.Height(22));
                    if (GUILayout.Button("Вернуться", GUILayout.Width(100), GUILayout.Height(18))) EventEditor.eventGraph = null;
                }

                debugStady = EditorGUILayout.IntPopup(debugStady, debugNumNames, debugNum, GUILayout.Width(100));

                isWatchOnPlay = EditorGUILayout.Toggle(isWatchOnPlay, GUILayout.Width(15));

                if (GUILayout.Button("Сценарий", GUILayout.Width(100))) Selection.activeObject = _storyData;

                if (_mainSettings == null) _mainSettings = (GameSettings)Resources.Load("BaseParameters");
                else
                {
                    if (GUILayout.Button("Данные", GUILayout.Width(100), GUILayout.Height(18))) Selection.activeObject = _mainSettings;
                }

                GUI.backgroundColor = Color.green;
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
                    if (MainController.instance.animController.thisPart != null && isWatchOnPlay)
                    {
                        if (MainController.instance.animController.thisPart == part)
                        {
                            FocusPart(part);
                            GUI.backgroundColor = Color.blue;
                        }
                    }
                }
                else
                {
                    if (debugStady == 0)
                    {
                        switch (part.workStady)
                        {
                            case 0: GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f); ; break;
                            case 1: GUI.backgroundColor = Color.yellow; break;
                            case 2: GUI.backgroundColor = Color.green; break;
                        }
                    }
                    else GUI.backgroundColor = DebugColor(part);


                    if (_sellectedToConnect != null)
                    {
                        if (_sellectedToConnect.Equals(part))
                        {
                            CreateCurve(ConnectPosition(part, tempConnect, false),
                                new Rect(mousePosition, new Vector2(0, 0)), Color.blue);
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

        /// <summary> Цвет отладки </summary>
        private Color DebugColor(GamePart part)
        {
            int score = 0;
            int check;

            // Главы

            switch (part)
            {
                case TextPart textP:
                {
                    if (textP.mainText.Length > 230 || textP.mainText.Length == 0) score += 10;
                    if (textP.buttonText.Length > 109 || textP.buttonText.Length == 0) score += 10;

                    if (textP.movePart[0] == null) score++;
                    break;
                }
                case ChangePart changeP:
                {
                    if (changeP.mainText.Length > 230 && changeP.mainText.Length == 0) score += 10;

                    if (changeP.buttonText[0].Length > 109 || changeP.buttonText[0].Length == 0) score += 10;
                    if (changeP.buttonText[1].Length > 109 || changeP.buttonText[1].Length == 0) score += 10;

                    if (changeP.movePart[0] == null) score++;
                    if (changeP.movePart[1] == null) score++;
                    break;
                }
                case BattlePart battleP:
                {
                    if (battleP.mainText.Length > 230) score += 10;

                    if (battleP.buttonText[0].Length > 109 || battleP.buttonText[0].Length == 0) score += 10;
                    if (battleP.buttonText[1].Length > 109 || battleP.buttonText[1].Length == 0) score += 10;
                    if (battleP.buttonText[2].Length > 109 || battleP.buttonText[2].Length == 0) score += 10;

                    if (battleP.movePart[0] == null) score++;
                    if (battleP.movePart[1] == null) score++;
                    if (battleP.movePart[2] == null) score++;
                    break;
                }
                case EventPart eventP:
                {
                    if (eventP.movePart[0] == null) score++;
                    if (eventP.movePart[2] == null) score++;
                    break;
                }
                case FinalPart finalP:
                {
                    if (finalP.newAchive == null) score++;
                    break;
                }
            }

            // События

            if(part.mainEvents.Count > 0)
            {
                for (int i = 0; i < part.mainEvents.Count; i++)
                {
                    if (part.mainEvents[i] is CheckDecision ||
                        part.mainEvents[i] is CheckPlayerInfl ||
                        part.mainEvents[i] is EffectInteract ||
                        part.mainEvents[i] is ItemInfl ||
                        part.mainEvents[i] is ItemInteract)
                    {
                        if (part.mainEvents[i].FailPart() == null) score += 10;
                    }
                    else if (part.mainEvents[i] is RandomPart rnd)
                    {
                        if (part is TextPart)
                        {
                            if (rnd.partRandom[0] == null) score += 10;
                            else if (rnd.randomChance[0] == 0) score++;

                        }
                        else if (part is ChangePart)
                        {
                            check = 0;

                            if (rnd.partRandom[0] == null)
                            {
                                check++;
                                score += 10;
                            }
                            else if (rnd.randomChance[0] == 0) score++;

                            if (rnd.partRandom[1] == null)
                            {
                                check++;
                                score += 10;
                            }
                            else if (rnd.randomChance[1] == 0) score++;

                            if (check > 0) score -= 10 * check;
                        }
                        else if (part is BattlePart)
                        {
                            check = 0;

                            if (rnd.partRandom[0] == null)
                            {
                                check++;
                                score += 10;
                            }
                            if (rnd.randomChance[0] == 0) score++;

                            if (rnd.partRandom[1] == null)
                            {
                                check++;
                                score += 10;
                            }
                            if (rnd.randomChance[1] == 1) score++;

                            if (rnd.partRandom[2] == null)
                            {
                                check++;
                                score += 10;
                            }
                            if (rnd.randomChance[2] == 2) score++;

                            if (check > 0) score -= 10 * check;
                        }
                    }
                }
            }

            // Проверка

            if (score == 0) return Color.green;
            else if(score > 0 && score < 10) return Color.yellow;
            else return Color.red;
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
                if (GUI.Button(ConnectPosition(bn, 0, false), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(0, bn);
                    tempConnect = 0;
                }
            }
            else if (bn is ChangePart)
            {
                if (GUI.Button(ConnectPosition(bn, 0, false), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(0, bn);
                    tempConnect = 0;
                }

                if (GUI.Button(ConnectPosition(bn, 1, false), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(1, bn);
                    tempConnect = 1;
                }
            }
            else if (bn is BattlePart)
            {
                if (GUI.Button(ConnectPosition(bn, 0, false), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(0, bn);
                    tempConnect = 0;
                }

                if (GUI.Button(ConnectPosition(bn, 1, false), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(1, bn);
                    tempConnect = 1;
                }

                if (GUI.Button(ConnectPosition(bn, 2, false), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(2, bn);
                    tempConnect = 2;
                }
            }
            else if (bn is LeandPart)
            {
                if (GUI.Button(ConnectPosition(bn, 0, false), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(0, bn);
                    tempConnect = 0;
                }
            }

            else if (bn is EventPart)
            {
                if (GUI.Button(ConnectPosition(bn, 0, false), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(0, bn);
                    tempConnect = 0;
                }

                if (GUI.Button(ConnectPosition(bn, 2, false), _emptyTexture, _storyData.graphSkin.FindStyle("Button")))
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
                    if (_storyData.nodesData[i].windowRect.Contains(mousePosition))
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
                    if (_storyData.nodesData[i].windowRect.Contains(mousePosition))
                    {
                        if (_sellectedToConnect != null)
                        {
                            _sellectedToConnect.movePart[_sellectionId] = _storyData.nodesData[i];
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
                    offset += _drag / _storyData.nodesData.Count;
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
                
                drawWindow = true;
                Repaint();
                SaveData();
            }
        }

        #endregion

        #region NODE_DRAW

        /// <summary> Отрисовка связей </summary>
        private void DrawCurve(GamePart partNode)
        {
            Color baseConnectColor = new Color(1, 1, 1, 0.75f);

            for (int i = 0; i < partNode.movePart.Length; i++)
            {
                if (partNode.movePart[i] != null)
                {
                    if (storyData.nodesData.Contains(partNode.movePart[i]) && partNode.movePart[i] != this)
                        CreateCurve(ConnectPosition(partNode, i, false), partNode.movePart[i].windowRect, baseConnectColor);
                }
            }

            DrawEventCurve(partNode);
        }

        /// <summary> Отрисовка связей Random Event </summary>
        private void DrawEventCurve(GamePart partNode)
        {
            if (partNode.mainEvents != null)
            {
                Color randomColor = Color.cyan;
                Color eventFailColor = Color.red;

                foreach (GameEvent ePart in partNode.mainEvents)
                {
                    switch (ePart)
                    {
                        case RandomPart randomPart:

                            if (randomPart.partRandom[0] != null && randomPart.partRandom[0] != this)
                            {
                                CreateCurve(ConnectPosition(partNode, 0, false), randomPart.partRandom[0].windowRect, randomColor);
                            }

                            if (randomPart.partRandom[1] != null && randomPart.partRandom[1] != this)
                            {
                                CreateCurve(ConnectPosition(partNode, 1, false), randomPart.partRandom[1].windowRect, randomColor);
                            }

                            if (randomPart.partRandom[2] != null && randomPart.partRandom[2] != this)
                            {
                                CreateCurve(ConnectPosition(partNode, 2, false), randomPart.partRandom[2].windowRect, randomColor);
                            }
                            break;

                        case CheckDecision checkDecision:

                            if (checkDecision.failPart != null && checkDecision.failPart != this)
                            {
                                CreateEventCurve(ConnectPosition(partNode, 1, true), ConnectPosition(checkDecision.failPart, 0, true), eventFailColor);
                            }
                            break;

                        case ItemInfl itemInfl:

                            if (itemInfl.failPart != null && itemInfl.failPart != this)
                            {
                                CreateEventCurve(ConnectPosition(partNode, 1, true), ConnectPosition(itemInfl.failPart, 0, true), eventFailColor);
                            }
                            break;

                        case ItemInteract itemInteract:

                            if (itemInteract.failPart != null && itemInteract.failPart != this)
                            {
                                CreateEventCurve(ConnectPosition(partNode, 1, true), ConnectPosition(itemInteract.failPart, 0, true), eventFailColor);
                            }
                            break;
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

            Vector3 startTan = startPos + Vector3.right * (Vector3.Distance(startPos, endPos) * 0.6f);
            Vector3 endTan = endPos + Vector3.left * (Vector3.Distance(startPos, endPos) * 0.6f);

            Handles.DrawBezier(startPos, endPos, startTan, endTan, colorCurve, null, 3f);
        }

        /// <summary> Связь </summary>
        private void CreateEventCurve(Rect start, Rect end, Color colorCurve)
        {
            Vector3 startPos = new Vector3(
                start.x + start.width,
                start.y + (start.height * .5f),
                0);

            Vector3 endPos = new Vector3(
                end.x,
                end.y + (end.height * .5f),
                0);

            Vector3 startTan = startPos + Vector3.up * (Vector3.Distance(startPos, endPos) * 0.6f);
            Vector3 endTan = endPos + Vector3.down * (Vector3.Distance(startPos, endPos) * 0.6f);

            Handles.DrawBezier(startPos, endPos, startTan, endTan, colorCurve, null, 3f);
        }

        /// <summary> Позиция подключения следующей главы </summary>
        private Rect ConnectPosition(GamePart partNode, int id, bool isEvent)
        {
            Rect nodeConnectPosition;

            if (isEvent)
            {
                if(id == 0)
                {
                    nodeConnectPosition = new Rect(
                    partNode.windowRect.x + partNode.windowRect.width / 2,
                    partNode.windowRect.y,
                    0,
                    0);
                }
                else
                {
                    nodeConnectPosition = new Rect(
                    partNode.windowRect.x + partNode.windowRect.width / 2,
                    partNode.windowRect.y + partNode.windowRect.height,
                    0,
                    0);
                }
            }
            else
            {
                nodeConnectPosition = new Rect(
                    partNode.windowRect.x + partNode.windowRect.width + 5,
                    (partNode.windowRect.y + 2) + (13 * id),
                    11,
                    11);
            }

            return nodeConnectPosition;
        }

        /// <summary> Отрисовка эвентов </summary>
        private void DrawEvents(GamePart partNode)
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
        private void SetWindowStady(bool stady)
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

                        partNode.memTitle = partNode.windowTitle;
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

                        partNode.windowTitle = partNode.memTitle;

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

            object[] obj = Resources.LoadAll("");
            
            for (int i = 0; i < obj.Length; i++)
            {
                if(obj[i] is ScriptableObject sc) EditorUtility.SetDirty(sc);
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
            
            offset += _drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

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