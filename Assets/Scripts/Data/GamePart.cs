using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    /// <summary> Группа игровых глав </summary>
    public abstract class GamePart : ScriptableObject
    {
        public string mainText;
        public List<GameEvent> mainEvents;

        /// <summary> Следующая глава кнопки </summary>
        public GamePart[] movePart = new GamePart[3];

#if UNITY_EDITOR

        // Отображение главы в редакторе сюжета
        public bool windowSizeStady = false;
        public bool memberComment;
        public Rect windowRect;
        public string memTitle;
        public string windowTitle;
        public GamePart part;
        public int workStady;
        public string comment;
        public bool isShowComment;

#endif
    }
}