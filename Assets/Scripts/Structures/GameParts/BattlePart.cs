using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New part", menuName = "Игровые обьекты/Новая глава/Глава боя", order = 2)]
public class BattlePart : GamePart
{
    /// <summary>
    /// Текст первой кнопки
    /// </summary>
    public string buttonText_1;

    /// <summary>
    /// Текст второй кнопки
    /// </summary>
    public string buttonText_2;

    /// <summary>
    /// Текст третей кнопки
    /// </summary>
    public string buttonText_3;
}