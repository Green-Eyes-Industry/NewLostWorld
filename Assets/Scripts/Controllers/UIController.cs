using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary> Замена текста на кнопках и контроль UI </summary>
public class UIController : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] private float _gameWaitToSwitch;

    [SerializeField] private RectTransform _eyeCenter;

    [SerializeField] private Image
        _achiveCase_1, _achiveCase_2,
        _achiveCase_3, _achiveCase_4,
        _achiveCase_5;

    [SerializeField] private Text
        _menuButton_1,
        _menuButton_2,
        _menuButton_3,
        _menuButton_4;

    [SerializeField] private Text _gameMain_Txt;
    [SerializeField] private Text
        _gameButton_1_Txt,
        _gameButton_2_Txt,
        _gameButton_3_Txt;

    [SerializeField] private Text _gameMessageInventoryTxt;
    [SerializeField] private Text _gameMessageCharacterTxt;
    [SerializeField] private Text _gameMessageMapTxt;
    [SerializeField] private Text _gameMessageNotesTxt;

    [SerializeField] private Image
        _inventCase_1, _inventCase_2,
        _inventCase_3, _inventCase_4,
        _inventCase_5, _inventCase_6,
        _inventCase_7, _inventCase_8;

    [SerializeField]
    private Image
        _effectCase_1, _effectCase_2,
        _effectCase_3, _effectCase_4,
        _effectCase_5, _effectCase_6;

    private Gyroscope _mainGyro;
    private bool _isGyroEnable;
    private Vector2 _eyeFixPosition;

    #endregion

    private void Start() => ConnectGyroscope();

    private void Update() => EyeWatching();

    /// <summary> Подключение гироскопа </summary>
    private void ConnectGyroscope()
    {
        if (SystemInfo.supportsGyroscope)
        {
            _mainGyro = Input.gyro;
            _mainGyro.enabled = true;
            _isGyroEnable = true;
        }
        else
        {
            _eyeCenter.anchoredPosition = new Vector2(0, 40);
            _isGyroEnable = false;
        }
    }

    /// <summary> Слежение глаза в меню </summary>
    private void EyeWatching()
    {
        if (_isGyroEnable)
        {
            if (_mainGyro.gravity.y > 0) _eyeFixPosition.y = (_mainGyro.gravity.y * 20) * -1;
            else _eyeFixPosition.y = (_mainGyro.gravity.y * 90) * -1;

            _eyeFixPosition.x = (_mainGyro.gravity.x * 50) * -1;

            _eyeCenter.anchoredPosition = _eyeFixPosition;
        }
    }

    #region TEXT_CONTROL

    /// <summary> Открыть главное меню </summary>
    public void MenuOpenMain() => StartCoroutine(WaitToMenuSwitch(0.2f));

    /// <summary> Открыть меню настроек </summary>
    public void MenuOpenSettings() => StartCoroutine(WaitToSettingsSwitch(0.2f));

    /// <summary> Открыть меню автора </summary>
    public void MenuOpenAbout() => StartCoroutine(WaitToAboutSwitch(0.2f));

    /// <summary> Замена основного текста в игре </summary>
    public void GameMain(string newTxt) => StartCoroutine(WaitToMainTextSwitch(_gameWaitToSwitch, newTxt));

    /// <summary> Замена текста на первой игровой кнопке </summary>
    public void GameButton_1(string newTxt) => StartCoroutine(WaitToGameButton_1_Switch(_gameWaitToSwitch, newTxt));

    /// <summary> Замена текста на второй игровой кнопке </summary>
    public void GameButton_2(string newTxt) => StartCoroutine(WaitToGameButton_2_Switch(_gameWaitToSwitch, newTxt));

    /// <summary> Замена текста на третьей игровой кнопке </summary>
    public void GameButton_3(string newTxt) => StartCoroutine(WaitToGameButton_3_Switch(_gameWaitToSwitch, newTxt));

    /// <summary> Текст сообщения инвентаря </summary>
    public void GameMessageInventory(string newTxt) => _gameMessageInventoryTxt.text = newTxt;

    /// <summary> Текст сообщения персонажа </summary>
    public void GameMessageCharacter(string newTxt) => _gameMessageCharacterTxt.text = newTxt;

    /// <summary> Текст сообщения карты </summary>
    public void GameMessageMap(string newTxt) => _gameMessageMapTxt.text = newTxt;

    /// <summary> Текст сообщения заметок </summary>
    public void GameMessageNotes(string newTxt) => _gameMessageNotesTxt.text = newTxt;

    #endregion

    #region UI_CONTROL

    /// <summary> Отобразить инвентарь </summary>
    public void ShowInventory(int page)
    {
        int InventLendth = DataController.playerData.playerInventory.Count;

        page *= 8;

        if (0 + page < InventLendth) _inventCase_1.sprite = DataController.playerData.playerInventory[0 + page].itemIco;
        if (1 + page < InventLendth) _inventCase_2.sprite = DataController.playerData.playerInventory[1 + page].itemIco;
        if (2 + page < InventLendth) _inventCase_3.sprite = DataController.playerData.playerInventory[2 + page].itemIco;
        if (3 + page < InventLendth) _inventCase_4.sprite = DataController.playerData.playerInventory[3 + page].itemIco;
        if (4 + page < InventLendth) _inventCase_5.sprite = DataController.playerData.playerInventory[4 + page].itemIco;
        if (5 + page < InventLendth) _inventCase_6.sprite = DataController.playerData.playerInventory[5 + page].itemIco;
        if (6 + page < InventLendth) _inventCase_7.sprite = DataController.playerData.playerInventory[6 + page].itemIco;
        if (7 + page < InventLendth) _inventCase_8.sprite = DataController.playerData.playerInventory[7 + page].itemIco;
    }

    /// <summary> Отобразить еффекты на персонаже </summary>
    public void ShowEffects()
    {
        int InventLendth = DataController.playerData.playerEffects.Count;

        if (0 < InventLendth) _effectCase_1.sprite = DataController.playerData.playerEffects[0].icoEffect;
        if (1 < InventLendth) _effectCase_2.sprite = DataController.playerData.playerEffects[1].icoEffect;
        if (2 < InventLendth) _effectCase_3.sprite = DataController.playerData.playerEffects[2].icoEffect;
        if (3 < InventLendth) _effectCase_4.sprite = DataController.playerData.playerEffects[3].icoEffect;
        if (4 < InventLendth) _effectCase_5.sprite = DataController.playerData.playerEffects[4].icoEffect;
        if (5 < InventLendth) _effectCase_6.sprite = DataController.playerData.playerEffects[5].icoEffect;
    }

    /// <summary> Отобразить достижения </summary>
    public void ShowAchive(int page)
    {
        int achivesLendth = DataController.gameSettingsData.gameAchivemants.Count;

        page *= 5;

        if (0 + page < achivesLendth) _achiveCase_1.sprite = DataController.gameSettingsData.gameAchivemants[0 + page].achiveIco;
        if (1 + page < achivesLendth) _achiveCase_2.sprite = DataController.gameSettingsData.gameAchivemants[1 + page].achiveIco;
        if (2 + page < achivesLendth) _achiveCase_3.sprite = DataController.gameSettingsData.gameAchivemants[2 + page].achiveIco;
        if (3 + page < achivesLendth) _achiveCase_4.sprite = DataController.gameSettingsData.gameAchivemants[3 + page].achiveIco;
        if (4 + page < achivesLendth) _achiveCase_5.sprite = DataController.gameSettingsData.gameAchivemants[4 + page].achiveIco;
    }

    #endregion

    #region WAIT_SWITCH

    /// <summary> Задержка при переходе  главное меню </summary>
    private IEnumerator WaitToMenuSwitch(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _menuButton_1.text = "Новая игра";
        _menuButton_2.text = "Настройки";
        _menuButton_3.text = "Об Авторах";
        _menuButton_4.text = "Достижения";
    }

    /// <summary> Задержка при переходе в настройки </summary>
    private IEnumerator WaitToSettingsSwitch(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _menuButton_1.text = "Звук";
        _menuButton_2.text = "Вибриция";
        _menuButton_3.text = "Эффекты";
        _menuButton_4.text = "Назад";
    }

    /// <summary> Задержка при переходе в меню автора </summary>
    private IEnumerator WaitToAboutSwitch(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _menuButton_4.text = "Назад";
    }

    /// <summary> Задержка замена основного текста в игре </summary>
    private IEnumerator WaitToMainTextSwitch(float waitTime, string newTxt)
    {
        yield return new WaitForSeconds(waitTime);

        _gameMain_Txt.text = newTxt;
    }

    /// <summary> Задержка при замене текста на первой кнопке </summary>
    private IEnumerator WaitToGameButton_1_Switch(float waitTime, string newTxt)
    {
        yield return new WaitForSeconds(waitTime);

        _gameButton_1_Txt.text = newTxt;
    }

    /// <summary> Задержка при замене текста на второй кнопке </summary>
    private IEnumerator WaitToGameButton_2_Switch(float waitTime, string newTxt)
    {
        yield return new WaitForSeconds(waitTime);

        _gameButton_2_Txt.text = newTxt;
    }

    /// <summary> Задержка при замене текста на третей кнопке </summary>
    private IEnumerator WaitToGameButton_3_Switch(float waitTime, string newTxt)
    {
        yield return new WaitForSeconds(waitTime);

        _gameButton_3_Txt.text = newTxt;
    }

    #endregion
}