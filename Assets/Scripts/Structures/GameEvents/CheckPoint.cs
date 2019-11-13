using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Контрольная точка")]
public class CheckPoint : GameEvent
{
    // Контрольная точка

    /// <summary>
    /// Старт события
    /// </summary>
    public override bool EventStart()
    {
        return false;
    }
}