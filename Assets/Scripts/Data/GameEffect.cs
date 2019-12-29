using UnityEngine;

namespace Data
{
    /// <summary> Группа эффектов </summary>
    public abstract class GameEffect : ScriptableObject
    {
        /// <summary> Название еффекта </summary>
        public string nameEffect;

        /// <summary> Значек эффекта </summary>
        public Sprite icoEffect;

        /// <summary> Длительность эффекта </summary>
        public int durationEffect;

        /// <summary> Влияние на здоровье </summary>
        public int healthInfluenceEffect;

        /// <summary> Влияние на рассудок </summary>
        public int mindInfluenceEffect;
    }
}