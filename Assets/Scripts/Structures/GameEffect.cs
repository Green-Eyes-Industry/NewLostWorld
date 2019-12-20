﻿using UnityEngine;

namespace NLW.Data
{
    /// <summary> Группа еффектов </summary>
    public abstract class GameEffect : ScriptableObject
    {
        /// <summary> Значек еффекта </summary>
        public Sprite icoEffect;

        /// <summary> Длительность эффекта </summary>
        public int durationEffect;

        /// <summary> Влияние на здоровье </summary>
        public int healthInfluenceEffect;

        /// <summary> Влияние на рассудок </summary>
        public int mindInfluenceEffect;
    }
}