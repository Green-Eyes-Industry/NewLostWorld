using System;
using UnityEngine;

namespace Helpers
{
    /// <summary> Контроль над кнопкой </summary>
    public class ButtonPressed : MonoBehaviour
    {
        public enum MenuType
        {
            MAIN,
            ACHIVE_CASE,
            ACHIVE,

            GAME_PREVIEW,
            GAME_PART,
            EVENT_PART,
            MAZE_PART,

            INVENTORY,
            PLAYER,
            MAP,
            NOTES,

            PAUSE
        };

        public MenuType menuType;

        public int buttonId;

        private void OnMouseDown() => PressButton(true);
        private void OnMouseUp() => PressButton(false);

        /// <summary> Нажатие кнопки </summary>
        private void PressButton(bool press)
        {
            switch (menuType)
            {
                case MenuType.MAIN: MainController.instance.animController.MainMenuPress(buttonId, press); break;
                case MenuType.ACHIVE_CASE: MainController.instance.animController.AchiveCaseMenuPress(buttonId, press); break;
                case MenuType.ACHIVE: MainController.instance.animController.AchiveMenuPress(buttonId, press); break;
                case MenuType.GAME_PREVIEW: MainController.instance.animController.PreviewExit(false); break;

                case MenuType.GAME_PART: MainController.instance.animController.GamePartPress(buttonId, press); break;
                case MenuType.EVENT_PART: MainController.instance.animController.EventPartPress(buttonId, press); break;
                case MenuType.MAZE_PART: MainController.instance.animController.MazePartPress(buttonId, press); break;

                case MenuType.INVENTORY: MainController.instance.animController.InventoryMenuPress(buttonId, press); break;
                case MenuType.PLAYER: MainController.instance.animController.PlayerMenuPress(buttonId, press); break;
                case MenuType.MAP: MainController.instance.animController.MapMenuPress(buttonId, press); break;
                case MenuType.NOTES: MainController.instance.animController.NotesMenuPress(buttonId, press); break;

                case MenuType.PAUSE: break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}