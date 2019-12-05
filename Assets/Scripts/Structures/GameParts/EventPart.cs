using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New part", menuName = "Игровые обьекты/Новая глава/Глава Эвента", order = 4)]
public class EventPart : GamePart
{
    /// <summary> Время на эвент </summary>
    public int timeToEvent;

    /// <summary> Финальная глава </summary>
    public GamePart finalPart;

    /// <summary> Глава при провале </summary>
    public GamePart failPart;

    /// <summary> Список глав эвента </summary>
    public List<GamePart> eventParts;

    /// <summary> Проверка главы </summary>
    /// <param name="currentPart"> Текущая глава </param>
    /// <returns> True если вы добрались к последней главе </returns>
    public bool CheckEvent(GamePart currentPart)
    {
        if (currentPart == finalPart) return true;
        else return false;
    }
}