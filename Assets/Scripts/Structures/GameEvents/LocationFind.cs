using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Найдена локация")]
public class LocationFind : GameEvent
{
    /// <summary> Найденная локация </summary>
    public MapMark _location;

    /// <summary> Найти локацию </summary>
    public override bool EventStart()
    {
        if(!DataController.playerData.playerMap.Contains(_location))
        {
            DataController.playerData.playerMap.Add(_location);
        }

        return true;
    }
}