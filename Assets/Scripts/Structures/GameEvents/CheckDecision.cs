using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Проверка решения")]
public class CheckDecision : GameEvent
{
    /// <summary>
    /// Решение
    /// </summary>
    public Decision decision;

    /// <summary>
    /// Глава при проверке
    /// </summary>
    public GamePart failPart;

    /// <summary>
    /// Проверка на принятое ранее решение
    /// </summary>
    /// <returns>Вернет False при провале проверки</returns>
    public override bool EventStart()
    {
        for (int i = 0; i < DataController.playerData.playerDecisions.Count; i++)
        {
            if (DataController.playerData.playerDecisions[i].Equals(decision)) return true;
        }

        return false;
    }
}