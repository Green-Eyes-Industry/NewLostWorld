using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Важное решение")]
public class ImportantDecision : GameEvent
{
    /// <summary> Решение </summary>
    public Decision decision;

    /// <summary> Принять решение </summary>
    public override bool EventStart()
    {
        if (DataController.playerData.playerDecisions.Contains(decision))
        {
            DataController.playerData.playerDecisions.Add(decision);
        }
        return true;
    }
}