using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New part", menuName = "Игровые обьекты/Новая глава/Глава выбора", order = 1)]
public class ChangePart : GamePart
{
    /// <summary>
    /// Текст первой кнопки
    /// </summary>
    public string buttonText_1;

    /// <summary>
    /// Текст второй кнопки
    /// </summary>
    public string buttonText_2;
}