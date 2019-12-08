using UnityEngine;

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Случайный переход")]
public class RandomPart : GameEvent
{
    /// <summary> Случайная глава 1 </summary>
    public GamePart part_1_random;

    /// <summary> Случайная глава 2 </summary>
    public GamePart part_2_random;

    /// <summary> Случайная глава 3 </summary>
    public GamePart part_3_random;

    /// <summary> Вероятность попадания в главу 1 </summary>
    public int randomChance_1;

    /// <summary> Вероятность попадания в главу 2 </summary>
    public int randomChance_2;

    /// <summary> Вероятность попадания в главу 3 </summary>
    public int randomChance_3;
}