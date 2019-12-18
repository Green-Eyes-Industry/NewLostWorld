using UnityEngine;

/// <summary> Управление игровыми параметрами </summary>
public class MainController : MonoBehaviour
{
    private AnimController _animController;

    public Player mainPlayer;
    public GameSettings mainGameSettings;

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
        DataController.gameSettingsData = mainGameSettings;
        DataController.playerData = mainPlayer;
    }

    /// <summary> Подключение компонентов </summary>
    private void InitComponents()
    {
        _animController.Init();
        DataController.LoadData();
    }
}