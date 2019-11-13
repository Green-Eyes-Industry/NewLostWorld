using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Влияние на НПС")]
public class NonPlayerInfl : GameEvent
{
    // Проверка влияния на НПС

    /// <summary>
    /// Старт события
    /// </summary>
    public override bool EventStart()
    {
        return false;
    }
}