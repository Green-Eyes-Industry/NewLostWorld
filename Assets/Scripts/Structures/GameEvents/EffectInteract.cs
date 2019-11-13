using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Взаимодействие с эффектом")]
public class EffectInteract : GameEvent
{
    // Получение или потеря евента

    /// <summary>
    /// Старт события
    /// </summary>
    public override bool EventStart()
    {
        return false;
    }
}