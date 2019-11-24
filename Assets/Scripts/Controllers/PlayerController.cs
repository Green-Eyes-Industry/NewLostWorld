using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Игровые события и переключения
/// </summary>
public class PlayerController : MonoBehaviour
{
    private DataController gameData;

    private void Start() => gameData = GetComponent<DataController>();

    #region PARTS_EVENTS

    /// <summary>
    /// Запуск событий главы
    /// </summary>
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

    /// <summary>
    /// Добавление предмета в инвентарь
    /// </summary>
    public void AddItemToInventory(GameItem gItem)
    {
        gameData.playerData.playerInventory.Add(gItem);
        gameData.SavePlayerInventory();
    }

    /// <summary>
    /// Удаление предмета из инвентаря
    /// </summary>
    public void RemoveItemFromInventory(GameItem gItem)
    {
        gameData.playerData.playerInventory.Remove(gItem);
        gameData.SavePlayerInventory();
    }

    #endregion
}