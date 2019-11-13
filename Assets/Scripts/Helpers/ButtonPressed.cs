using UnityEngine;

/// <summary>
/// Контроль над кнопкой
/// </summary>
public class ButtonPressed : MonoBehaviour
{
    [Tooltip("Кнопка в меню")] [SerializeField] private bool _itIsMenu;
    [Tooltip("ID кнопки")] [SerializeField] private int _buttonId;

    private MoveController _moveController;
    private BoxCollider2D _collider;

    private void Start()
    {
        _moveController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MoveController>();
        _collider = GetComponent<BoxCollider2D>();

        ResizeCollider();
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

    /// <summary>
    /// Подгонка коллайдера под разрешение экрана
    /// </summary>
    private void ResizeCollider()
    {
        _collider.size = new Vector2(
            Screen.width * (_collider.size.x / 720),
            Screen.height * (_collider.size.y / 1280)
            );
    }
}