using UnityEngine;

/// <summary>
/// Контроль над кнопкой
/// </summary>
public class ButtonPressed : MonoBehaviour
{
    [Tooltip("Кнопка в меню")] [SerializeField] private bool _itIsMenu;
    [Tooltip("ID кнопки")] [SerializeField] private int _buttonId;

    private MoveController _moveController;

    private void Awake()
    {
        _moveController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MoveController>();
    }

    private void OnMouseDown()
    {
        if(_itIsMenu) _moveController.MenuButtonDown(_buttonId);
        else _moveController.GameButtonDown(_buttonId);
    }

    private void OnMouseUp()
    {
        if(_itIsMenu) _moveController.MenuButtonUp(_buttonId);
        else _moveController.GameButtonUp(_buttonId);
    }
}