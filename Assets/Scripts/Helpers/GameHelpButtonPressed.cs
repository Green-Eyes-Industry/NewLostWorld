using UnityEngine;

public class GameHelpButtonPressed : MonoBehaviour
{
    public enum HelpMenuType
    {
        INVENTORY_MENU,
        PLAYER_MENU,
        MAP_MENU,
        NOTES_MENU
    }

    public HelpMenuType thisMenuType;

    public int buttonId;

    private MoveController _moveController;

    private void Awake() => _moveController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MoveController>();

    private void OnMouseDown() => PressedMenu(true);

    private void OnMouseUp() => PressedMenu(false);

    /// <summary>
    /// Запуск меню по типу
    /// </summary>
    private void PressedMenu(bool pressedType)
    {
        switch (thisMenuType)
        {
            case HelpMenuType.INVENTORY_MENU: _moveController.GameHelpInventoryButton(pressedType, buttonId); break;
            case HelpMenuType.PLAYER_MENU: _moveController.GameHelpPlayerButton(pressedType, buttonId); break;
            case HelpMenuType.MAP_MENU: _moveController.GameHelpMapButton(pressedType, buttonId); break;
            case HelpMenuType.NOTES_MENU: _moveController.GameHelpNotesButton(pressedType, buttonId); break;
        }
    }
}