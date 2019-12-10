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
        public static StoryData storyData; // Данные сюжета
        private StoryData _storyData;
        public static Vector3 _mousePosition; // Позиция мыши
        private bool _isClickOnWindow; // Нажал на окно или нет
        private GamePart _selectedNode; // Выбранная нода

        private Texture _connectTexture; // Текстура подключения
        private GamePart _sellectedToConnect; // Нода в памяти при подключении полключения
        private int _sellectionId; // Идентификатор типа подключения

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
                if (storyData == null) storyData = _storyData;
                
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
                        GraphChangeController.selectedNode = _storyData.nodesData[i];
                        Selection.activeObject = _storyData.nodesData[i];
                        break;
                    }
                }
            }

            if (!_isClickOnWindow) GraphChangeController.AddNewNode(e);
            else GraphChangeController.AddEventToPart(e); //AddEventToPart(e);
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
                GraphChangeController.selectedNode = null;
                SaveData();
                Repaint();
            }
        }

        #endregion

        #region HELPERS

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

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        #endregion
    }
}