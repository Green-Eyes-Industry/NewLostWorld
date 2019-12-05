﻿using UnityEngine;

[CreateAssetMenu(fileName = "New npc", menuName = "Игровые обьекты/Новый персонаж/НПС", order = 1)]
public class NonPlayer : Character
{
    /// <summary> Имя НПС </summary>
    public string npName;

    /// <summary> Отношение НПС к игроку </summary>
    public int npToPlayerRatio;
}