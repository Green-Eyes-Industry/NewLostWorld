using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Контрольная точка")]
public class CheckPoint : GameEvent
{
    /// <summary>
    /// Новый текст превью
    /// </summary>
    public string _newPreviewText;

    /// <summary>
    /// Старт события
    /// </summary>
    public override bool EventStart()
    {
        // TODO : Замена текста при вступлении

        DataController.gameSettingsData.lastPart = MoveController._startPart;
        DataController.SaveGamePreferences();

        return false;
    }
}