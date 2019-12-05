using UnityEngine;

[CreateAssetMenu(fileName = "new location", menuName = "Игровые обьекты/Метка на карте")]
public class MapMark : ScriptableObject
{
    /// <summary> Название локации </summary>
    public string nameLocation;

    /// <summary> Лор и описание локации </summary>
    public string loreLocation;

    /// <summary> Глава для перемещения </summary>
    public GamePart _partLocation;
}