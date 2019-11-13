using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Игровое событие
/// </summary>
public class GameEvent : ScriptableObject
{
    public virtual bool EventStart() { return false; }
}