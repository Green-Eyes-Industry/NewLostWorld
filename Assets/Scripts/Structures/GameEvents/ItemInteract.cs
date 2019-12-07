using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Взаимодействие с предметом")]
public class ItemInteract : GameEvent
{
    /// <summary> Получение или потеря </summary>
    public bool isAddOrLostItem;

    /// <summary> Получаемый предмет </summary>
    public GameItem gameItem;

    /// <summary> Глава при провале </summary>
    public GamePart failPart;

    /// <summary> Старт события </summary>
    public override bool EventStart()
    {
        if (isAddOrLostItem)
        {
            if (!DataController.playerData.playerInventory.Contains(gameItem))
            {
                DataController.playerData.playerInventory.Add(gameItem);
            }
            return true;
        }
        else
        {
            if (DataController.playerData.playerInventory.Contains(gameItem))
            {
                DataController.playerData.playerInventory.Remove(gameItem);
                return true;
            }
            else return true;
        }
    }
}