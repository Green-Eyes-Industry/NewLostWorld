using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Игровые обьекты/Новый предмет/Активный")]
public class UsableItem : GameItem
{
    /// <summary> Влияние на здоровье </summary>
    public int healthInf;

    /// <summary> Влияние на сознание </summary>
    public int mindInf;

    /// <summary> Накладываемый эффект </summary>
    public GameEffect itemEffect;

    /// <summary> Использовать </summary>
    public void UseThisItem()
    {
        DataController.playerData.playerHealth += healthInf;
        DataController.playerData.playerMind += mindInf;

        if (!DataController.playerData.playerEffects.Contains(itemEffect))
        {
            DataController.playerData.playerEffects.Add(itemEffect);
        }
    }
}