using System.Collections.Generic;
using UnityEngine;

/// <summary> Группа игровых глав </summary>
public class GamePart : ScriptableObject
{
    public string mainText;
    public List<GameEvent> mainEvents;

    /// <summary> Следующая глава первой кнопки </summary>
    public GamePart movePart_1;

    /// <summary> Следующая глава второй кнопки </summary>
    public GamePart movePart_2;

    /// <summary> Следующая глава третей кнопки </summary>
    public GamePart movePart_3;

#if UNITY_EDITOR

    public bool windowSizeStady = false;
    private bool memberComment;
    public Rect windowRect;
    public float openedHeight = 120f;
    public string windowTitle;
    public string _memberTitle;
    public GamePart part;

    public int workStady;
    public int[] workStadyNum = new int[] { 0, 1, 2};
    public string[] workStadyNames = new string[] { "Пусто", "Разработка", "Готово" };

    public string comment;
    public bool isShowComment;

    public void DrawWindow()
    {
        if (!windowSizeStady)
        {
            UnityEditor.EditorGUILayout.BeginHorizontal();
            workStady = UnityEditor.EditorGUILayout.IntPopup(workStady, workStadyNames, workStadyNum, GUILayout.Width(90f));
            isShowComment = UnityEditor.EditorGUILayout.Toggle(isShowComment);
            UnityEditor.EditorGUILayout.EndHorizontal();

            if (isShowComment)
            {
                windowRect.height = openedHeight;
                comment = UnityEditor.EditorGUILayout.TextArea(comment, GUILayout.Width(110f), GUILayout.Height(78));
            }
            else windowRect.height = 40f;
        }
    }

    /// <summary> Отрисовка связей </summary>
    public void DrawCurve(List<GamePart> partList)
    {
        Color baseConnectColor = new Color(0, 0, 0, 0.75f);
        
        if (movePart_1 != null)
        {
            if (partList.Contains(movePart_1) && movePart_1 != this) CreateCurve(ConnectPosition(0), movePart_1.windowRect, baseConnectColor);
        }

        if (movePart_2 != null)
        {
            if (partList.Contains(movePart_2) && movePart_2 != this) CreateCurve(ConnectPosition(1), movePart_2.windowRect, baseConnectColor);
        }

        if (movePart_3 != null)
        {
            if (partList.Contains(movePart_3) && movePart_3 != this) CreateCurve(ConnectPosition(2), movePart_3.windowRect, baseConnectColor);
        }

        if(mainEvents != null)
        {
            bool checkRandom = false;
            RandomPart randomEvent = null;

            for (int i = 0; i < mainEvents.Count; i++)
            {
                if (mainEvents[i] is RandomPart)
                {
                    checkRandom = true;
                    randomEvent = (RandomPart)mainEvents[i];
                }
            }

            if (checkRandom)
            {
                Color randomConnectColor = new Color(1f, 0, 0, 0.75f);

                if(randomEvent.part_1_random != null && randomEvent.part_1_random != this)
                {
                    CreateCurve(ConnectPosition(0), randomEvent.part_1_random.windowRect, randomConnectColor);
                }

                if (randomEvent.part_2_random != null && randomEvent.part_2_random != this)
                {
                    CreateCurve(ConnectPosition(1), randomEvent.part_2_random.windowRect, randomConnectColor);
                }

                if (randomEvent.part_3_random != null && randomEvent.part_3_random != this)
                {
                    CreateCurve(ConnectPosition(2), randomEvent.part_3_random.windowRect, randomConnectColor);
                }
            }
        }
    }

    /// <summary> Связь </summary>
    private void CreateCurve(Rect start, Rect end,Color colorCurve)
    {
        Vector3 startPos = new Vector3(
            start.x,
            start.y + (start.height * .5f),
            0);

        Vector3 endPos = new Vector3(
            end.x,
            end.y + (end.height * .5f),
            0);

        Vector3 startTan = startPos + Vector3.right * 30;
        Vector3 endTan = endPos + Vector3.left * 30;

        UnityEditor.Handles.DrawBezier(startPos, endPos, startTan, endTan, colorCurve, null, 3f);
    }

    /// <summary> Позиция подключения следующей главы </summary>
    public Rect ConnectPosition(int id)
    {
        Rect nodeConnectPosition;

        switch (id)
        {
            case 1:
                nodeConnectPosition = new Rect(
                windowRect.x + windowRect.width + 3,
                windowRect.y + 12,
                8,
                8);
                break;

            case 2:
                nodeConnectPosition = new Rect(
                windowRect.x + windowRect.width + 3,
                windowRect.y + 24,
                8,
                8);
                break;

            default:
                nodeConnectPosition = new Rect(
                windowRect.x + windowRect.width + 3,
                windowRect.y,
                8,
                8);
                break;
        }

        return nodeConnectPosition;
    }

    /// <summary> Отрисовка евентов </summary>
    public void DrawEvents()
    {
        int sizeEvent;

        if (windowSizeStady) sizeEvent = 7;
        else sizeEvent = 20;

        GUIStyle st = new GUIStyle();

        if(mainEvents != null)
        {
            for (int i = 0; i < mainEvents.Count; i++)
            {
                if (i < 6)
                {
                    GUI.Box(new Rect(
                windowRect.x + (sizeEvent * i),
                windowRect.y + windowRect.height,
                sizeEvent,
                sizeEvent),
                GetEventTextures(mainEvents[i]), st);
                }
                else
                {
                    if (i < 11)
                    {
                        GUI.Box(new Rect(
                    windowRect.x + (sizeEvent * (i - 6)),
                    windowRect.y + windowRect.height + sizeEvent,
                    sizeEvent,
                    sizeEvent),
                    GetEventTextures(mainEvents[i]), st);
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

        if(crEvent is CheckDecision) pathToIco = "Assets/Editor/NodeEditor/Images/EventsIco/CheckDecision.png";
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

        eventIco = (Texture)UnityEditor.AssetDatabase.LoadAssetAtPath(pathToIco, typeof(Texture));
        return eventIco;
    }

    /// <summary> Масштаб окна </summary>
    public void SetWindowStady()
    {
        windowSizeStady = !windowSizeStady;

        if (windowSizeStady)
        {
            windowRect.width = 40;
            windowRect.height = 38;

            windowRect.x /= 2f;
            windowRect.y /= 2f;

            windowRect.x += Screen.width / 4f;
            windowRect.y += Screen.height / 4f;

            _memberTitle = windowTitle;
            windowTitle = windowTitle.Substring(0, GetShortNameNode(windowTitle));

            memberComment = isShowComment;

            if (isShowComment) isShowComment = false;
        }
        else
        {
            windowRect.width = 120;
            windowRect.height = 40;

            windowRect.x *= 2f;
            windowRect.y *= 2f;

            windowRect.x -= Screen.width / 2f;
            windowRect.y -= Screen.height / 2f;

            windowTitle = _memberTitle;

            isShowComment = memberComment;
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

#endif
}