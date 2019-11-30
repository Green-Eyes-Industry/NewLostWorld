using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GUIInspector.StoryEditor
{
    /// <summary>
    /// Базовая нода
    /// </summary>
    public abstract class BaseNode : ScriptableObject
    {
        /// <summary>
        /// Размер ноды
        /// </summary>
        public Rect windowRect;

        /// <summary>
        /// Заглавие ноды
        /// </summary>
        public string windowTitle;

        /// <summary>
        /// Отрисовка окна
        /// </summary>
        public virtual void DrawWindow() { }

        /// <summary>
        /// Отрисовка кривой
        /// </summary>
        public virtual void DrawCurve() { }
    }
}