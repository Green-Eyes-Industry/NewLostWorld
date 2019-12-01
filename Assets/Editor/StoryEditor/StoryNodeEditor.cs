using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GUIInspector.StoryEditor
{
    public class StoryNodeEditor : Editor
    {
        public static Vector2 SizeParts;

        /// <summary>
        /// Показать редактор сюжета
        /// </summary>
        public static void ShowStoryEditor(List<GamePart> gameParts)
        {
            DrawGrid();

            // TODO : Дописать редактор сценария
        }

        /// <summary>
        /// Отрисовка сетки
        /// </summary>
        private static void DrawGrid()
        {

        }
    }
}