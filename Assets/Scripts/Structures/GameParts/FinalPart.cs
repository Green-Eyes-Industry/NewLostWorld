using UnityEngine;

[CreateAssetMenu(fileName = "New part", menuName = "Игровые обьекты/Новая глава/Финальная глава", order = 5)]
public class FinalPart : GamePart
{
    /// <summary> Получаемое достижение, если нужно </summary>
    public Achivemants newAchive;

    /// <summary> Текст кнопки возврата в меню </summary>
    public string backButtonText;
}