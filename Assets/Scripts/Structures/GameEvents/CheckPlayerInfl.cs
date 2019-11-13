using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Проверка влияния персонажа")]
public class CheckPlayerInfl : GameEvent
{
    // Проверка характеристик персонажа

    /// <summary>
    /// Старт события
    /// </summary>
    public override bool EventStart()
    {
        return false;
    }
}