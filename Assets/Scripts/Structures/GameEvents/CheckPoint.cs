using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Контрольная точка")]
public class CheckPoint : GameEvent
{
    /// <summary> Перезаписывает сохраненные данные </summary>
    public override bool EventStart()
    {
        DataController.gameSettingsData.lastPart = MoveController.thisPart;
        DataController.SaveGameData();

        return true;
    }
}