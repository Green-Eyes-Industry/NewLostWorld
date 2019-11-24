using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Взаимодействие с предметом")]
public class ItemInteract : GameEvent
{
    // Получение или потеря предмета

    private PlayerController plController;

    /// <summary>
    /// Получение или потеря
    /// </summary>
    public bool addOrLostItem;

    /// <summary>
    /// Получаемый предмет
    /// </summary>
    public GameItem gameItem;

    /// <summary>
    /// Старт события
    /// </summary>
    public override bool EventStart()
    {
        plController = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerController>();

        if (addOrLostItem) plController.AddItemToInventory(gameItem);
        else plController.RemoveItemFromInventory(gameItem);

        return false;
    }
}