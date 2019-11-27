using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Влияние предмета")]
public class ItemInfl : GameEvent
{
    public UsableItem useItem;

    /// <summary>
    /// Старт события
    /// </summary>
    public override bool EventStart()
    {
        bool finded = false;

        if (DataController.playerData.playerInventory.Count != 0)
        {
            for (int i = 0; i < DataController.playerData.playerInventory.Count; i++)
            {
                if(DataController.playerData.playerInventory[i] is UsableItem)
                {
                    if(DataController.playerData.playerInventory[i].Equals(useItem))
                    {
                        finded = true;
                        UseThisItem();
                        DataController.playerData.playerInventory.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        return finded;
    }

    /// <summary>
    /// Использовать найденый предмет
    /// </summary>
    private void UseThisItem()
    {
        DataController.playerData.playerHealth += useItem.healthInf;
        DataController.playerData.playerMind += useItem.healthInf;
    }
}