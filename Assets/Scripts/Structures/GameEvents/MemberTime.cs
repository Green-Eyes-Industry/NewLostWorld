using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Воспоминание")]
public class MemberTime : GameEvent
{
    /// <summary> Воспоминание </summary>
    public Note _note;

    public override bool EventStart()
    {
        if (!DataController.playerData.playerNotes.Contains(_note))
        {
            DataController.playerData.playerNotes.Add(_note);
        }

        return true;
    }
}