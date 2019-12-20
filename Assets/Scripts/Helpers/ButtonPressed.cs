using UnityEngine;

namespace NLW.Helpers
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
                case MenuType.MAIN: MainController.Instance.animController.MainMenuPress(buttonId, press); break;
                case MenuType.ACHIVE_CASE: MainController.Instance.animController.AchiveCaseMenuPress(buttonId, press); break;
                case MenuType.ACHIVE: MainController.Instance.animController.AchiveMenuPress(buttonId, press); break;
                case MenuType.GAME_PREVIEW: MainController.Instance.animController.PreviewExit(false); break;

                case MenuType.GAME_PART: MainController.Instance.animController.GamePartPress(buttonId, press); break;
                case MenuType.EVENT_PART: MainController.Instance.animController.EventPartPress(buttonId, press); break;
                case MenuType.MAZE_PART: MainController.Instance.animController.MazePartPress(buttonId, press); break;

                case MenuType.INVENTORY: MainController.Instance.animController.InventoryMenuPress(buttonId, press); break;
                case MenuType.PLAYER: MainController.Instance.animController.PlayerMenuPress(buttonId, press); break;
                case MenuType.MAP: MainController.Instance.animController.MapMenuPress(buttonId, press); break;
                case MenuType.NOTES: MainController.Instance.animController.NotesMenuPress(buttonId, press); break;

                case MenuType.PAUSE: break;
            }
        }
    }
}