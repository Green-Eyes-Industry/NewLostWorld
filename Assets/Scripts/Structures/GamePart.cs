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
    public bool memberComment;
    public Rect windowRect;
    public string _memTitle;
    public string windowTitle;
    public GamePart part;
    public int workStady;
    public string comment;
    public bool isShowComment;

#endif
}