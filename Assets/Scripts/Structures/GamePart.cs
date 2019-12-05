using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

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

        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;

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

#endif
}