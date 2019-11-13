using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New game settings", menuName = "Игровые обьекты/Игровые параметры")]
public class GameSettings : ScriptableObject
{
    // Хранит данные о текущих игровых настройках

    /// <summary>
    /// Последняя глава
    /// </summary>
    public GamePart lastPart;

    /// <summary>
    /// Настройка звука
    /// </summary>
    public bool soundCheck;

    /// <summary>
    /// Настройка вибрации
    /// </summary>
    public bool vibrationCheck;

    /// <summary>
    /// Настройка еффектов
    /// </summary>
    public bool effectCheck;

    /// <summary>
    /// Цвет глаза
    /// </summary>
    public float eyeColor;
}