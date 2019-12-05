using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New game settings", menuName = "Игровые обьекты/Игровые параметры")]
public class GameSettings : ScriptableObject
{
    /// <summary> Последняя глава </summary>
    public GamePart lastPart;

    /// <summary> Настройка звука </summary>
    public bool isSoundCheck;

    /// <summary> Настройка вибрации </summary>
    public bool isVibrationCheck;

    /// <summary> Настройка еффектов </summary>
    public bool isEffectCheck;

    /// <summary> Цвет глаза </summary>
    public Color eyeColor;

    /// <summary> Полученные достижения </summary>
    public List<Achivemants> gameAchivemants;
}