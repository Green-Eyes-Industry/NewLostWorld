using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New part", menuName = "Игровые обьекты/Новая глава/Глава слайдшоу", order = 6)]
public class MoviePart : GamePart
{
    // Слайд-шоу вставка

        /// <summary>
        /// Список спрайтов в слайд-шоу
        /// </summary>
    public List<Sprite> movieSprites;
}