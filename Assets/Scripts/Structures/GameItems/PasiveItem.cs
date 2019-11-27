using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Игровые обьекты/Новый предмет/Пасивный")]
public class PasiveItem : GameItem
{
    /// <summary>
    /// Накладываемый эффект пока предмет у персонажа
    /// </summary>
    public GameEffect itemEffect;
}