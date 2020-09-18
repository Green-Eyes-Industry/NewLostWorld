using Controllers;
using UnityEngine;

namespace Data
{
    public abstract class GameItem : ScriptableObject
    {
        /// <summary> Значек предмета </summary>
        public Sprite itemIco;

        /// <summary> Название предмета </summary>
        public string itemName;

        /// <summary> Описание предмета </summary>
        public string itemDescript;
    }
}