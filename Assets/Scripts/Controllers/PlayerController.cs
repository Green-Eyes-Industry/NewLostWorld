using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Игровые события и переключения
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Запуск событий главы
    /// </summary>
    public void EventsStart(List<GameEvent> partEvents)
    {
        if(partEvents.Count != 0)
        {
            for (int i = 0; i < partEvents.Count; i++)
            {
                partEvents[i].EventStart();
            }
        }
    }
}