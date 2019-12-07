using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Использование предмета")]
public class ItemInfl : GameEvent
{
    /// <summary> Уничтожить в процессе </summary>
    public bool isRemove;

    /// <summary> Предмет </summary>
    public UsableItem useItem;

    /// <summary> Глава при провале </summary>
    public GamePart failPart;

    /// <summary> Влияние </summary>
    public override bool EventStart()
    {
        if (DataController.playerData.playerInventory.Contains(useItem))
        {
            useItem.UseThisItem();
            if (isRemove) DataController.playerData.playerInventory.Remove(useItem);
            return true;
        }
        else return false;
    }
}