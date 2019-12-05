using System.Collections.Generic;
using UnityEngine;

/// <summary> Игровые события и переключения </summary>
public class PlayerController : MonoBehaviour
{
    #region PARTS_EVENTS

    /// <summary> Запуск событий главы </summary>
    public void EventsStart(List<GameEvent> partEvents)
    {
        if (partEvents.Count != 0)
        {
            for (int i = 0; i < partEvents.Count; i++)
            {
                partEvents[i].EventStart();
            }
        }
    }

    #endregion

    #region INVENTORY_CONTROLLER

    /// <summary> Добавление предмета в инвентарь </summary>
    public void AddItemToInventory(GameItem gItem)
    {
        DataController.playerData.playerInventory.Add(gItem);
        DataController.SavePlayerInventory();
    }

    /// <summary> Удаление предмета из инвентаря </summary>
    public void RemoveItemFromInventory(GameItem gItem)
    {
        DataController.playerData.playerInventory.Remove(gItem);
        DataController.SavePlayerInventory();
    }

    #endregion
}