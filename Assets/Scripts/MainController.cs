using UnityEngine;

/// <summary> Управление игровыми параметрами </summary>
public class MainController : MonoBehaviour
{
    private MoveController _moveController;
    [SerializeField] private Player _mainPlayer;
    [SerializeField] private GameSettings _mainGameSettings;

    private void Start()
    {
        ConnectControllers();
        ConnectComponents();
        ConnectLastSavePoint();
    }

    /// <summary> Подключение сохранений </summary>
    private void ConnectLastSavePoint()
    {
        MoveController._mainAnimator.SetBool("Settings_1_St", DataController.gameSettingsData.isSoundCheck);
        MoveController._mainAnimator.SetBool("Settings_2_St", DataController.gameSettingsData.isVibrationCheck);
        MoveController._mainAnimator.SetBool("Settings_3_St", DataController.gameSettingsData.isEffectCheck);
    }

    /// <summary> Подключение контроллеров и заполнение данных </summary>
    private void ConnectControllers()
    {
        _moveController = GetComponent<MoveController>();
        MoveController.moveContr = _moveController;

        DataController.gameSettingsData = _mainGameSettings;
        DataController.playerData = _mainPlayer;
    }

    /// <summary> Подключение компонентов </summary>
    private void ConnectComponents()
    {
        _moveController.Init();
    }
}