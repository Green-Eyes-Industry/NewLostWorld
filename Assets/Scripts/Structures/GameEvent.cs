using UnityEngine;

/// <summary> Игровое событие </summary>
public class GameEvent : ScriptableObject
{
    public virtual bool EventStart() { return false; }

    #region UNITY_EDITOR

    public bool editorEventFoldout;

    #endregion
}