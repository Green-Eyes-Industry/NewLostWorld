using UnityEngine;

[CreateAssetMenu(fileName = "New achivemant",menuName = "Игровые обьекты/Достижение")]
public class Achivemants : ScriptableObject
{
    /// <summary> Значек достижения </summary>
    public Sprite achiveIco;

    /// <summary> Название достижения </summary>
    public string achiveName;

    /// <summary> Описание достижения </summary>
    public string achiveDescript;
}