using UnityEngine;

[CreateAssetMenu(fileName = "new note", menuName = "Игровые обьекты/Заметка")]
public class Notes : ScriptableObject
{
    /// <summary>
    /// Название заметки
    /// </summary>
    public string noteName;

    /// <summary>
    /// Текст заметки
    /// </summary>
    public string noteDescription;

    /// <summary>
    /// Глава воспоминания
    /// </summary>
    public GamePart _partNote;
}