using UnityEngine;

[CreateAssetMenu(fileName = "New decision", menuName = "Игровые обьекты/Решение")]
public class Decision : ScriptableObject
{
    // Важное игровое решение

    #region UNITY_EDITOR

    /// <summary>
    /// Описание решения
    /// </summary>
    public string _decisionDescription;

    #endregion
}