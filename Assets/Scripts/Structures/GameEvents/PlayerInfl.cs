using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Влияние на игрока")]
public class PlayerInfl : GameEvent
{
    /// <summary> Влияние на здоровье </summary>
    public int _healthInfl;

    /// <summary> Влияние на рассудок </summary>
    public int _mindInfl;

    /// <summary> Влияние </summary>
    public override bool EventStart()
    {
        DataController.playerData.playerHealth += _healthInfl;
        DataController.playerData.playerMind += _mindInfl;
        return true;
    }
}