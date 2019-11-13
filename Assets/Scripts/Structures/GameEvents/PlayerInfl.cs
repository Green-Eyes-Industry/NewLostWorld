using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Влияние на игрока")]
public class PlayerInfl : GameEvent
{
    // Влияние главы на состояние игрока

    /// <summary>
    /// Старт события
    /// </summary>
    public override bool EventStart()
    {
        return false;
    }
}