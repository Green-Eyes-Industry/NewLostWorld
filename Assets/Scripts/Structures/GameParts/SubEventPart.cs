using UnityEngine;

[CreateAssetMenu(fileName = "Sub part", menuName = "Игровые обьекты/Новая глава/Под-глава Евента", order = 2)]
public class SubEventPart : ScriptableObject
{
    /// <summary> Базовый текст </summary>
    public string mainText;

    /// <summary> Победа в событии </summary>
    public bool isFinal;

    /// <summary> Провал события </summary>
    public bool isFail;

    /// <summary> Глава при переходе влево </summary>
    public SubEventPart moveLeft;

    /// <summary> Глава при переходе вправо </summary>
    public SubEventPart moveRight;

#if UNITY_EDITOR

    public bool windowSizeStady = false;
    public Rect windowRect;
    public float openedHeight = 120f;
    public string windowTitle;

#endif

}