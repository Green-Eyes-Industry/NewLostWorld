using UnityEngine;
using UnityEditor;

namespace GUIInspector.NodeEditor
{
    public class BehaviorEditor : EditorWindow
    {
        #region VARIABLES

        private Color _gridColor;
        private Vector2 _offset;
        private Vector2 _drag;
        private GameSettings _mainSettings;
        public static BehaviorEditor trBehaviorEditor;
        private StoryData _storyData;
        private Vector3 _mousePosition;
        private bool _isClickOnWindow;
        private GamePart _selectedNode;

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

        #endregion

        #region INIT

        [MenuItem("Story/Node Editor")]
        [System.Obsolete]
        static void ShowEditor()
        {
            BehaviorEditor editor = GetWindow<BehaviorEditor>();
            editor.title = "Node Editor";
            editor._gridColor = new Color(0.6f, 0.6f, 0.6f);
            trBehaviorEditor = editor;
        }

        #endregion

        #region GUI_METHODS

        private void OnGUI()
        {
            if(_storyData != null)
            {
                DrawGrid(10, 0.2f, _gridColor);
                DrawGrid(50, 0.4f, _gridColor);

                Event e = Event.current;
                _mousePosition = e.mousePosition;
                UserInput(e);

                EditorGUILayout.BeginHorizontal("TextArea");

                EditorGUILayout.SelectableLabel("Текущие : ", GUILayout.Height(22));

                if(GUILayout.Button("Данные сценария", GUILayout.Width(200))) Selection.activeObject = _storyData;

                if (_mainSettings == null) _mainSettings = (GameSettings)Resources.Load("BaseParameters");
                else
                {
                    if (GUILayout.Button("Данные", GUILayout.Width(100), GUILayout.Height(18))) Selection.activeObject = _mainSettings;
                }

                if (GUILayout.Button("Сохранить", GUILayout.Width(100), GUILayout.Height(18))) SaveData();

                EditorGUILayout.EndVertical();

                DrawWindows();
            }
            else
            {
                EditorGUILayout.LabelField("Нед данных сюжета");
                EditorGUILayout.Space();
                _storyData = (StoryData)EditorGUILayout.ObjectField("Файл данных", _storyData, typeof(StoryData), false);
            }
            
        }

        /// <summary> Отрисовка всех нод </summary>
        private void DrawWindows()
        {
            _drag = Vector2.zero;

            BeginWindows();

            foreach (GamePart bn in _storyData.nodesData)
            {
                if(bn != null) bn.DrawCurve(_storyData.nodesData);
            }

            for (int i = 0; i < _storyData.nodesData.Count; i++)
            {
                if (_storyData.nodesData[i] != null)
                {
                    GUI.backgroundColor = new Color(0.75f,0.75f,0.75f);

                    if (EditorApplication.isPlaying)
                    {
                        if (MoveController._startPart != null)
                        {
                            if (MoveController._startPart == _storyData.nodesData[i]) GUI.backgroundColor = new Color(0.5f, 0.5f, 0.75f);
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
                if (e.type == EventType.MouseDown) LeftMouseClick(e);
            }

            if(e.isKey)
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
        }

        /// <summary> Правый клик мыши </summary>
        private void RightMouseClick(Event e)
        {
            _selectedNode = null;
            _isClickOnWindow = false;

            for (int i = 0; i < _storyData.nodesData.Count; i++)
            {
                if(_storyData.nodesData[i] != null)
                {
                    if (_storyData.nodesData[i].windowRect.Contains(_mousePosition))
                    {
                        _isClickOnWindow = true;
                        _selectedNode = _storyData.nodesData[i];
                        break;
                    }
                }
            }

            if (!_isClickOnWindow) AddNewNode(e);
        }

        /// <summary> Левый клик мыши </summary>
        private void LeftMouseClick(Event e)
        {
            for (int i = 0; i < _storyData.nodesData.Count; i++)
            {
                if (_storyData.nodesData[i] != null)
                {
                    if (_storyData.nodesData[i].windowRect.Contains(_mousePosition))
                    {
                        _selectedNode = _storyData.nodesData[i];
                        Selection.activeObject = _storyData.nodesData[i];
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
            if(_selectedNode != null)
            {
                _storyData.nodesData.Remove(_selectedNode);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_selectedNode));
                _selectedNode = null;
                SaveData();
                Repaint();
            }
        }

        /// <summary> Проверка действия </summary>
        private void ContexCallBack(object o)
        {
            Object[] loadedObj = Resources.LoadAll("GameParts",typeof(GamePart));

            UserActions a = (UserActions)o;

            switch (a)
            {
                case UserActions.ADD_TEXT_PART:

                    string nameTextPart = loadedObj.Length + "_TextPart";
                    string pathTextPartAsset = "Assets/Resources/GameParts/" + nameTextPart + ".asset";

                    AssetDatabase.CreateAsset(CreateInstance(typeof(TextPart)), pathTextPartAsset);
                    TextPart textPart = (TextPart)AssetDatabase.LoadAssetAtPath(pathTextPartAsset,typeof(TextPart));

                    textPart.windowTitle = nameTextPart;
                    textPart.windowRect = new Rect(_mousePosition.x, _mousePosition.y, 120, 40);

                    _storyData.nodesData.Add(textPart);
                    SaveData();

                    break;

                case UserActions.ADD_CHANGE_PART:

                    string nameChangePart = loadedObj.Length + "_ChangePart";
                    string pathChangePartAsset = "Assets/Resources/GameParts/" + nameChangePart + ".asset";

                    AssetDatabase.CreateAsset(CreateInstance(typeof(ChangePart)), pathChangePartAsset);
                    ChangePart changePart = (ChangePart)AssetDatabase.LoadAssetAtPath(pathChangePartAsset, typeof(ChangePart));

                    changePart.windowTitle = nameChangePart;
                    changePart.windowRect = new Rect(_mousePosition.x, _mousePosition.y, 120, 40);

                    _storyData.nodesData.Add(changePart);
                    SaveData();

                    break;

                case UserActions.ADD_BATTLE_PART:

                    string nameBattlePart = loadedObj.Length + "_BattlePart";
                    string pathBattlePartAsset = "Assets/Resources/GameParts/" + nameBattlePart + ".asset";

                    AssetDatabase.CreateAsset(CreateInstance(typeof(BattlePart)), pathBattlePartAsset);
                    BattlePart battlePart = (BattlePart)AssetDatabase.LoadAssetAtPath(pathBattlePartAsset, typeof(BattlePart));

                    battlePart.windowTitle = nameBattlePart;
                    battlePart.windowRect = new Rect(_mousePosition.x, _mousePosition.y, 120, 40);

                    _storyData.nodesData.Add(battlePart);
                    SaveData();

                    break;

                case UserActions.ADD_MAZE_PART:

                    string namePazzlePart = loadedObj.Length + "_PazzlePart";
                    string pathPazzlePartAsset = "Assets/Resources/GameParts/" + namePazzlePart + ".asset";

                    AssetDatabase.CreateAsset(CreateInstance(typeof(PazzlePart)), pathPazzlePartAsset);
                    PazzlePart pazzlePart = (PazzlePart)AssetDatabase.LoadAssetAtPath(pathPazzlePartAsset, typeof(PazzlePart));

                    pazzlePart.windowTitle = namePazzlePart;
                    pazzlePart.windowRect = new Rect(_mousePosition.x, _mousePosition.y, 120, 40);

                    _storyData.nodesData.Add(pazzlePart);
                    SaveData();

                    break;

                case UserActions.ADD_EVENT_PART:

                    string nameEventPart = loadedObj.Length + "_EventPart";
                    string pathEventPartAsset = "Assets/Resources/GameParts/" + nameEventPart + ".asset";

                    AssetDatabase.CreateAsset(CreateInstance(typeof(EventPart)), pathEventPartAsset);
                    EventPart eventPart = (EventPart)AssetDatabase.LoadAssetAtPath(pathEventPartAsset, typeof(EventPart));

                    eventPart.windowTitle = nameEventPart;
                    eventPart.windowRect = new Rect(_mousePosition.x, _mousePosition.y, 120, 40);

                    _storyData.nodesData.Add(eventPart);
                    SaveData();

                    break;

                case UserActions.ADD_FINAL_PART:

                    string nameFinalPart = loadedObj.Length + "_FinalPart";
                    string pathFinalPartAsset = "Assets/Resources/GameParts/" + nameFinalPart + ".asset";

                    AssetDatabase.CreateAsset(CreateInstance(typeof(FinalPart)), pathFinalPartAsset);
                    FinalPart finalPart = (FinalPart)AssetDatabase.LoadAssetAtPath(pathFinalPartAsset, typeof(FinalPart));

                    finalPart.windowTitle = nameFinalPart;
                    finalPart.windowRect = new Rect(_mousePosition.x, _mousePosition.y, 120, 40);

                    _storyData.nodesData.Add(finalPart);
                    SaveData();

                    break;

                case UserActions.ADD_LABEL_PART:

                    string nameLeandPart = loadedObj.Length + "_LeandPart";
                    string pathLeandPartAsset = "Assets/Resources/GameParts/" + nameLeandPart + ".asset";

                    AssetDatabase.CreateAsset(CreateInstance(typeof(LeandPart)), pathLeandPartAsset);
                    LeandPart leandPart = (LeandPart)AssetDatabase.LoadAssetAtPath(pathLeandPartAsset, typeof(LeandPart));

                    leandPart.windowTitle = nameLeandPart;
                    leandPart.windowRect = new Rect(_mousePosition.x, _mousePosition.y, 120, 40);

                    _storyData.nodesData.Add(leandPart);
                    SaveData();

                    break;

                case UserActions.ADD_SLIDESHOW_PART:

                    string nameMoviePart = loadedObj.Length + "_MoviePart";
                    string pathMoviePartAsset = "Assets/Resources/GameParts/" + nameMoviePart + ".asset";

                    AssetDatabase.CreateAsset(CreateInstance(typeof(MoviePart)), pathMoviePartAsset);
                    MoviePart moviePart = (MoviePart)AssetDatabase.LoadAssetAtPath(pathMoviePartAsset, typeof(MoviePart));

                    moviePart.windowTitle = nameMoviePart;
                    moviePart.windowRect = new Rect(_mousePosition.x, _mousePosition.y, 120, 40);

                    _storyData.nodesData.Add(moviePart);
                    SaveData();

                    break;

                case UserActions.ADD_TRANSIT:

                    break;
            }   
        }

        #endregion

        #region HELPERS

        /// <summary> Создать новую ноду </summary>
        private void AddNewNode(Event e)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Текстовая"), false, ContexCallBack, UserActions.ADD_TEXT_PART);
            menu.AddItem(new GUIContent("Выбора"), false, ContexCallBack, UserActions.ADD_CHANGE_PART);
            menu.AddItem(new GUIContent("Боя"), false, ContexCallBack, UserActions.ADD_BATTLE_PART);
            menu.AddItem(new GUIContent("Загадка"), false, ContexCallBack, UserActions.ADD_MAZE_PART);
            menu.AddItem(new GUIContent("Эвент"), false, ContexCallBack, UserActions.ADD_EVENT_PART);
            menu.AddItem(new GUIContent("Финальная"), false, ContexCallBack, UserActions.ADD_FINAL_PART);
            menu.AddItem(new GUIContent("Вставка"), false, ContexCallBack, UserActions.ADD_LABEL_PART);
            menu.AddItem(new GUIContent("Слайдшоу"), false, ContexCallBack, UserActions.ADD_SLIDESHOW_PART);
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