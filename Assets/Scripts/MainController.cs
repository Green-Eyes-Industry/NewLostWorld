using UnityEngine;

/// <summary> Управление игровыми параметрами </summary>
public class MainController : MonoBehaviour
{
    private AnimController _animController;

    [SerializeField] private Player _mainPlayer;
    [SerializeField] private GameSettings _mainGameSettings;

    private void Start()
    {
        ConnectControllers();
        InitComponents();
    }

    /// <summary> Подключение контроллеров и заполнение данных </summary>
    private void ConnectControllers()
    {
        _animController = GetComponent<AnimController>();

        AnimController.moveContr = _animController;
        DataController.gameSettingsData = _mainGameSettings;
        DataController.playerData = _mainPlayer;
    }

    /// <summary> Подключение компонентов </summary>
    private void InitComponents()
    {
        _animController.Init();
    }
}