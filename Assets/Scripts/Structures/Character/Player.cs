using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New player", menuName = "Игровые обьекты/Новый персонаж/Игрок", order = 0)]
public class Player : Character
{
    // Описывает игрока

    /// <summary>
    /// Здоровье игрока
    /// </summary>
    public int playerHealth;

    /// <summary>
    /// Рассудок игрока
    /// </summary>
    public int playerMind;

    /// <summary>
    /// Список эффектов на игрроке
    /// </summary>
    public List<GameEffect> playerEffects;

    /// <summary>
    /// Инвентарь игрока
    /// </summary>
    public List<GameItem> playerInventory;

    /// <summary>
    /// Заметки игрока
    /// </summary>
    public List<Notes> playerNotes;

    /// <summary>
    /// Открытиые локации
    /// </summary>
    public List<MapMark> playerMap;

    /// <summary>
    /// Важные решения
    /// </summary>
    public List<Decision> playerDecisions;
}