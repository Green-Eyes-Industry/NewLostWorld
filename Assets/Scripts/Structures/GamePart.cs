using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Группа игровых глав
/// </summary>
public class GamePart : ScriptableObject
{
    public string mainText;
    public List<GameEvent> mainEvents;

    /// <summary>
    /// Следующая глава первой кнопки
    /// </summary>
    public GamePart movePart_1;

    /// <summary>
    /// Следующая глава второй кнопки
    /// </summary>
    public GamePart movePart_2;

    /// <summary>
    /// Следующая глава третей кнопки
    /// </summary>
    public GamePart movePart_3;

#if UNITY_EDITOR

    /// <summary>
    /// Позиция главы в редакторе
    /// </summary>
    public Vector2 partPosition;

#endif
}