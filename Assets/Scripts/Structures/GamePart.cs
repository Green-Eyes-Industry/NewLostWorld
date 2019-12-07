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

    public Rect windowRect;
    public string windowTitle;
    public GamePart part;

    public int workStady;
    public int[] workStadyNum = new int[] { 0, 1, 2};
    public string[] workStadyNames = new string[] { "Пусто", "Разработка", "Готово" };

    public void DrawWindow()
    {
        workStady = UnityEditor.EditorGUILayout.IntPopup(workStady, workStadyNames, workStadyNum, GUILayout.Width(110f));

    }

    /// <summary> Отрисовка связей </summary>
    public void DrawCurve(List<GamePart> partList)
    {
        if (movePart_1 != null)
        {
            if (partList.Contains(movePart_1) && movePart_1 != this) CreateCurve(ConnectPosition(0), movePart_1.windowRect, true);
        }
        if (movePart_2 != null)
        {
            if (partList.Contains(movePart_2) && movePart_1 != this) CreateCurve(ConnectPosition(1), movePart_2.windowRect, true);
        }

        if (movePart_3 != null)
        {
            if (partList.Contains(movePart_2) && movePart_1 != this) CreateCurve(ConnectPosition(2), movePart_3.windowRect, true);
        }
    }

    /// <summary> Связь </summary>
    private void CreateCurve(Rect start, Rect end, bool left)
    {
        Vector3 startPos = new Vector3(
            (left) ? start.x + start.width : start.x,
            start.y + (start.height * .5f),
            0);

        Vector3 endPos = new Vector3(
            end.x,
            end.y + (end.height * .5f),
            0);

        Vector3 startTan = startPos + Vector3.right * 30;
        Vector3 endTan = endPos + Vector3.left * 30;

        Color shadow = new Color(0, 0, 0, 0.75f);

        UnityEditor.Handles.DrawBezier(startPos, endPos, startTan, endTan, shadow, null, 3f);
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
        GUIStyle st = new GUIStyle();

        for (int i = 0; i < mainEvents.Count; i++)
        {
            if (i < 6)
            {
                GUI.Box(new Rect(
            windowRect.x + (20 * i),
            windowRect.y + windowRect.height,
            20,
            20),
            GetEventTextures(mainEvents[i]), st);
            }
            else
            {
                if (i < 11)
                {
                    GUI.Box(new Rect(
                windowRect.x + (20 * (i - 6)),
                windowRect.y + windowRect.height + 20,
                20,
                20),
                GetEventTextures(mainEvents[i]), st);
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
        else return null;

        eventIco = (Texture)UnityEditor.AssetDatabase.LoadAssetAtPath(pathToIco, typeof(Texture));
        return eventIco;
    }

#endif
}