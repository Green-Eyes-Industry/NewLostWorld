using UnityEngine;

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

    private AnimController _animController;

    private void Awake() => _animController = GameObject.FindGameObjectWithTag("GameController").GetComponent<AnimController>();
    private void OnMouseDown() => PressButton(true);
    private void OnMouseUp() => PressButton(false);

    /// <summary> Нажатие кнопки </summary>
    private void PressButton(bool press)
    {
        switch (menuType)
        {
            case MenuType.MAIN: _animController.MainMenuPress(buttonId, press); break;
            case MenuType.ACHIVE_CASE: _animController.AchiveCaseMenuPress(buttonId, press); break;
            case MenuType.ACHIVE: _animController.AchiveMenuPress(buttonId, press); break;
            case MenuType.GAME_PREVIEW: _animController.PreviewExit(false); break;

            case MenuType.GAME_PART: _animController.GamePartPress(buttonId, press); break;
            case MenuType.EVENT_PART: _animController.EventPartPress(buttonId, press); break;
            case MenuType.MAZE_PART: _animController.MazePartPress(buttonId, press); break;

            case MenuType.INVENTORY: _animController.InventoryMenuPress(buttonId, press); break;
            case MenuType.PLAYER: _animController.PlayerMenuPress(buttonId, press); break;
            case MenuType.MAP: _animController.MapMenuPress(buttonId, press); break;
            case MenuType.NOTES: _animController.NotesMenuPress(buttonId, press); break;

            case MenuType.PAUSE: break;
        }
    }
}