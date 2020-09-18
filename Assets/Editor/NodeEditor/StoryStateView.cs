using UnityEngine;
using UnityEditor;
using Data;
using Data.Characters;
using Helpers;

public class StoryStateView : EditorWindow
{
    #region VARIABLES

    private Player _mainPlayer;

    private GameItem[] _itemsData;
    private Decision[] _decisionsData;
    private MapMark[] _locationsData;
    private Note[] _notesData;
    private NonPlayer[] _playerInflData;

    private Vector2 _mainScroll;

    private int _upMenuId;
    private string[] _nameUpMenu = new string[] {"Предметы", "Решения","Локации", "Заметки", "Отношения"};
    private int _buttonSize;

    #endregion

    #region GUI

    private void OnEnable()
    {
        _itemsData = Resources.LoadAll<GameItem>("GameItems");
        _decisionsData = Resources.LoadAll<Decision>("Decisions");
        _locationsData = Resources.LoadAll<MapMark>("Locations");
        _notesData = Resources.LoadAll<Note>("Notes");
        _playerInflData = Resources.LoadAll<NonPlayer>("Players/NonPlayers");
    }

    [MenuItem("Story/State View")]
    public static void ShowStoryStateWindow()
    {
        StoryStateView sView = GetWindow<StoryStateView>();
        sView.titleContent.text = "State View";
    }

    private void OnGUI()
    {
        if(_mainPlayer == null)
        {
            EditorGUILayout.LabelField("Нет персонажа, выберите в окне ниже");
            _mainPlayer = (Player)EditorGUILayout.ObjectField(_mainPlayer, typeof(Player), true);
            return;
        }

        _buttonSize = Screen.width / _nameUpMenu.Length;

        EditorGUILayout.BeginHorizontal();
        _upMenuId = GUILayout.SelectionGrid(_upMenuId, _nameUpMenu, _nameUpMenu.Length);
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Обновить", GUILayout.Width(_buttonSize)))
        {
            OnEnable();
            return;
        }
        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = Color.white;

        _mainScroll = EditorGUILayout.BeginScrollView(_mainScroll, "TextArea");

        switch (_upMenuId)
        {
            case 0: ShowItemsMenu(); break;
            case 1: ShowDecisionsMenu(); break;
            case 2: ShowLocationsMenu(); break;
            case 3: ShowNotesMenu(); break;
            case 4: ShowNonPlayersInflMenu(); break;
        }

        EditorGUILayout.EndScrollView();
    }

    #endregion

    #region MENU

    /// <summary> Отобразить меню "Предметы" </summary>
    private void ShowItemsMenu()
    {
        if (_itemsData == null) OnEnable();

        EditorGUILayout.LabelField("Создано " + _itemsData.Length + " предметов");

        foreach (GameItem dt in _itemsData)
        {
            if (_mainPlayer.playerInventory.Contains(dt)) GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.white;

            EditorGUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(dt.itemName);

            if (GUILayout.Button("Найти", GUILayout.Width(_buttonSize))) Selection.activeObject = dt;

            EditorGUILayout.EndHorizontal();
        }
    }

    /// <summary> Отобразить меню "Решения" </summary>
    private void ShowDecisionsMenu()
    {
        if (_decisionsData == null) OnEnable();

        EditorGUILayout.LabelField("Создано " + _decisionsData.Length + " решений");

        foreach (Decision dt in _decisionsData)
        {
            if (_mainPlayer.playerDecisions.Contains(dt)) GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.white;

            EditorGUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(dt.nameDecision);

            if (GUILayout.Button("Найти", GUILayout.Width(_buttonSize))) Selection.activeObject = dt;

            EditorGUILayout.EndHorizontal();
        }
    }

    /// <summary> Отобразить меню "Локации" </summary>
    private void ShowLocationsMenu()
    {
        if (_locationsData == null) OnEnable();

        EditorGUILayout.LabelField("Создано " + _locationsData.Length + " локаций");

        foreach (MapMark dt in _locationsData)
        {
            if (_mainPlayer.playerMap.Contains(dt)) GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.white;

            EditorGUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(dt.nameLocation);

            if (dt.partLocation != null && GUILayout.Button("Показать " + dt.partLocation.name)) Selection.activeObject = dt.partLocation;
            else EditorGUILayout.LabelField("Глава перехода не добавлена");

            if (GUILayout.Button("Найти", GUILayout.Width(_buttonSize))) Selection.activeObject = dt;

            EditorGUILayout.EndHorizontal();
        }
    }

    /// <summary> Отобразить меню "Заметки" </summary>
    private void ShowNotesMenu()
    {
        if (_notesData == null) OnEnable();

        EditorGUILayout.LabelField("Создано " + _notesData.Length + " заметок");

        foreach (Note dt in _notesData)
        {
            if (_mainPlayer.playerNotes.Contains(dt)) GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.white;

            EditorGUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(dt.noteName);

            if (dt.partNote != null && GUILayout.Button("Показать " + dt.partNote.name)) Selection.activeObject = dt.partNote;
            else EditorGUILayout.LabelField("Глава перехода не добавлена");

            if (GUILayout.Button("Найти", GUILayout.Width(_buttonSize))) Selection.activeObject = dt;

            EditorGUILayout.EndHorizontal();
        }
    }

    /// <summary> Отобразить меню "Отношения" </summary>
    private void ShowNonPlayersInflMenu()
    {
        if (_playerInflData == null) OnEnable();

        EditorGUILayout.LabelField("Создано " + _playerInflData.Length + " персонажей");

        foreach (NonPlayer dt in _playerInflData)
        {
            EditorGUILayout.BeginHorizontal("Box");

            EditorGUILayout.LabelField(dt.npName);

            if (dt.npToPlayerRatio < 0) GUI.backgroundColor = Color.red;
            else if (dt.npToPlayerRatio > 0) GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.white;

            EditorGUILayout.IntSlider(dt.npToPlayerRatio, -10, 10);

            if (GUILayout.Button("Найти", GUILayout.Width(_buttonSize))) Selection.activeObject = dt;

            EditorGUILayout.EndHorizontal();
        }
    }

    #endregion
}