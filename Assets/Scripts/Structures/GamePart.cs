﻿using System.Collections.Generic;
using UnityEngine;

namespace NLW.Parts
{
    /// <summary> Группа игровых глав </summary>
    public abstract class GamePart : ScriptableObject
    {
        public string mainText;
        public List<Data.GameEvent> mainEvents;

        /// <summary> Следующая глава кнопки </summary>
        public GamePart[] movePart = new GamePart[3];

#if UNITY_EDITOR

        public bool windowSizeStady = false;
        public bool memberComment;
        public Rect windowRect;
        public string _memTitle;
        public string windowTitle;
        public GamePart part;
        public int workStady;
        public string comment;
        public bool isShowComment;

#endif
    }
}