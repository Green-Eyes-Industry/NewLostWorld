using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Проверка влияния персонажа")]
public class CheckPlayerInfl : GameEvent
{
    /// <summary> Персонаж </summary>
    public NonPlayer nonPlayer;

    /// <summary> Влияние </summary>
    public int value;

    /// <summary> Глава при провале </summary>
    public GamePart failPart;

    /// <summary> Проверка соответствия влияния </summary>
    /// <returns> Вернет False при провале проверки </returns>
    public override bool EventStart()
    {
        if(nonPlayer.npToPlayerRatio >= value) return true;
        else return false;
    }
}