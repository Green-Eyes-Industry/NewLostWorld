using UnityEngine;

/// <summary> Контроль над кнопкой </summary>
public class ButtonPressed : MonoBehaviour
{
    [SerializeField] private bool _isMenu;
    [SerializeField] private int _buttonId;

    private MoveController _moveController;

    private void Awake() => _moveController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MoveController>();

    private void OnMouseDown()
    {
        if(_isMenu) _moveController.MenuButtonDown(_buttonId);
        else _moveController.GameButtonDown(_buttonId);
    }

    private void OnMouseUp()
    {
        if(_isMenu) _moveController.MenuButtonUp(_buttonId);
        else _moveController.GameButtonUp(_buttonId);
    }
}