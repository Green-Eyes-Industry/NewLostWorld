using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Важное решение")]
public class ImportantDecision : GameEvent
{
    //Важное игровое решение

    /// <summary>
    /// Старт события
    /// </summary>
    public override bool EventStart()
    {
        return true;
    }
}