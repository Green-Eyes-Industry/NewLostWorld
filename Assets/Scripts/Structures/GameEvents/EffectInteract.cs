using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Взаимодействие с эффектом")]
public class EffectInteract : GameEvent
{
    /// <summary> Эффект </summary>
    public GameEffect gameEffect;

    /// <summary> Получение или потеря </summary>
    public bool isAddOrRemove;

    /// <summary> Глава при провале </summary>
    public GamePart failPart;

    /// <summary> Взаимодействие </summary>
    public override bool EventStart()
    {
        if (isAddOrRemove)
        {
            if (!DataController.playerData.playerEffects.Contains(gameEffect))
            {
                DataController.playerData.playerEffects.Add(gameEffect);
            }
            return true;
        }
        else
        {
            if (DataController.playerData.playerEffects.Contains(gameEffect))
            {
                DataController.playerData.playerEffects.Remove(gameEffect);
                return true;
            }
            else return false;
        }
    }
}