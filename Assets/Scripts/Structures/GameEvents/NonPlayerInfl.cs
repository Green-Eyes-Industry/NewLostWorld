using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Влияние на НПС")]
public class NonPlayerInfl : GameEvent
{
    /// <summary>
    /// На какого персонажа
    /// </summary>
    public NonPlayer nonPlayer;

    /// <summary>
    /// Влияние
    /// </summary>
    public int value;

    /// <summary>
    /// Старт события
    /// </summary>
    public override bool EventStart()
    {
        nonPlayer.npToPlayerRatio += value;

        return false;
    }
}