using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary> Замена текста на кнопках и контроль UI </summary>
public class UIController : MonoBehaviour
{
    #region VARIABLES

    [Header("Базовые")]

    [SerializeField] private float _gameWaitToSwitch;

    [SerializeField] private RectTransform _eyeCenter;

    [Header("Достижения")]

    [SerializeField] private Sprite _achiveClose;
    [SerializeField] private Sprite _achiveOpen;

    [SerializeField] private Image _achiveCase_1;
    [SerializeField] private Image _achiveCase_2;
    [SerializeField] private Image _achiveCase_3;
    [SerializeField] private Image _achiveCase_4;
    [SerializeField] private Image _achiveCase_5;

    [Header("Главное меню")]

    [SerializeField] private Text _menuButton_1;
    [SerializeField] private Text _menuButton_2;
    [SerializeField] private Text _menuButton_3;
    [SerializeField] private Text _menuButton_4;

    [Header("Игровая глава")]

    [SerializeField] private Text _gameMain_Txt;
    [SerializeField] private Text _gameButton_1_Txt;
    [SerializeField] private Text _gameButton_2_Txt;
    [SerializeField] private Text _gameButton_3_Txt;

    [Header("Сообщения в игре")]

    [SerializeField] private Text _gameMessageInventoryTxt;
    [SerializeField] private Text _gameMessageCharacterTxt;
    [SerializeField] private Text _gameMessageMapTxt;
    [SerializeField] private Text _gameMessageNotesTxt;

    [Header("Инвентарь")]

    [SerializeField] private Image _inventCase_1;
    [SerializeField] private Image _inventCase_2;
    [SerializeField] private Image _inventCase_3;
    [SerializeField] private Image _inventCase_4;
    [SerializeField] private Image _inventCase_5;
    [SerializeField] private Image _inventCase_6;
    [SerializeField] private Image _inventCase_7;
    [SerializeField] private Image _inventCase_8;

    [Header("Эффекты в меню персонажа")]

    [SerializeField] private Image _effectCase_1;
    [SerializeField] private Image _effectCase_2;
    [SerializeField] private Image _effectCase_3;
    [SerializeField] private Image _effectCase_4;
    [SerializeField] private Image _effectCase_5;
    [SerializeField] private Image _effectCase_6;

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

        if (0 + page < InventLendth)
        {
            _inventCase_1.gameObject.SetActive(true);
            _inventCase_1.sprite = DataController.playerData.playerInventory[0 + page].itemIco;
        }
        else _inventCase_8.gameObject.SetActive(false);

        if (1 + page < InventLendth)
        {
            _inventCase_2.gameObject.SetActive(true);
            _inventCase_2.sprite = DataController.playerData.playerInventory[1 + page].itemIco;
        }
        else _inventCase_8.gameObject.SetActive(false);

        if (2 + page < InventLendth)
        {
            _inventCase_3.gameObject.SetActive(true);
            _inventCase_3.sprite = DataController.playerData.playerInventory[2 + page].itemIco;
        }
        else _inventCase_8.gameObject.SetActive(false);

        if (3 + page < InventLendth)
        {
            _inventCase_4.gameObject.SetActive(true);
            _inventCase_4.sprite = DataController.playerData.playerInventory[3 + page].itemIco;
        }
        else _inventCase_8.gameObject.SetActive(false);

        if (4 + page < InventLendth)
        {
            _inventCase_5.gameObject.SetActive(true);
            _inventCase_5.sprite = DataController.playerData.playerInventory[4 + page].itemIco;
        }
        else _inventCase_8.gameObject.SetActive(false);

        if (5 + page < InventLendth)
        {
            _inventCase_6.gameObject.SetActive(true);
            _inventCase_6.sprite = DataController.playerData.playerInventory[5 + page].itemIco;
        }
        else _inventCase_8.gameObject.SetActive(false);

        if (6 + page < InventLendth)
        {
            _inventCase_7.gameObject.SetActive(true);
            _inventCase_7.sprite = DataController.playerData.playerInventory[6 + page].itemIco;
        }
        else _inventCase_8.gameObject.SetActive(false);

        if (7 + page < InventLendth)
        {
            _inventCase_8.gameObject.SetActive(true);
            _inventCase_8.sprite = DataController.playerData.playerInventory[7 + page].itemIco;
        }
        else _inventCase_8.gameObject.SetActive(false);

    }

    /// <summary> Отобразить еффекты на персонаже </summary>
    public void ShowEffects()
    {
        int InventLendth = DataController.playerData.playerEffects.Count;

        if (0 < InventLendth)
        {
            _effectCase_1.gameObject.SetActive(true);
            _effectCase_1.sprite = DataController.playerData.playerEffects[0].icoEffect;
        }
        else _effectCase_1.gameObject.SetActive(false);

        if (1 < InventLendth)
        {
            _effectCase_2.gameObject.SetActive(true);
            _effectCase_2.sprite = DataController.playerData.playerEffects[1].icoEffect;
        }
        else _effectCase_2.gameObject.SetActive(false);

        if (2 < InventLendth)
        {
            _effectCase_3.gameObject.SetActive(true);
            _effectCase_3.sprite = DataController.playerData.playerEffects[2].icoEffect;
        }
        else _effectCase_3.gameObject.SetActive(false);

        if (3 < InventLendth)
        {
            _effectCase_4.gameObject.SetActive(true);
            _effectCase_4.sprite = DataController.playerData.playerEffects[3].icoEffect;
        }
        else _effectCase_4.gameObject.SetActive(false);

        if (4 < InventLendth)
        {
            _effectCase_5.gameObject.SetActive(true);
            _effectCase_5.sprite = DataController.playerData.playerEffects[4].icoEffect;
        }
        else _effectCase_5.gameObject.SetActive(false);

        if (5 < InventLendth)
        {
            _effectCase_6.gameObject.SetActive(true);
            _effectCase_6.sprite = DataController.playerData.playerEffects[5].icoEffect;
        }
        else _effectCase_6.gameObject.SetActive(false);
    }

    /// <summary> Отобразить достижения </summary>
    public void ShowAchive(int page)
    {
        int achivesLendth = DataController.gameSettingsData.gameAchivemants.Count;

        page *= 5;

        if (0 + page < achivesLendth)
        {
            _achiveCase_1.gameObject.transform.parent.GetComponent<Image>().sprite = _achiveOpen;
            _achiveCase_1.gameObject.SetActive(true);
            _achiveCase_1.sprite = DataController.gameSettingsData.gameAchivemants[0 + page].achiveIco;
        }
        else
        {
            _achiveCase_1.gameObject.transform.parent.GetComponent<Image>().sprite = _achiveClose;
            _achiveCase_1.gameObject.SetActive(false);
        }

        if (1 + page < achivesLendth)
        {
            _achiveCase_2.gameObject.transform.parent.GetComponent<Image>().sprite = _achiveOpen;
            _achiveCase_2.gameObject.SetActive(true);
            _achiveCase_2.sprite = DataController.gameSettingsData.gameAchivemants[1 + page].achiveIco;
        }
        else
        {
            _achiveCase_2.gameObject.transform.parent.GetComponent<Image>().sprite = _achiveClose;
            _achiveCase_2.gameObject.SetActive(false);
        }

        if (2 + page < achivesLendth)
        {
            _achiveCase_3.gameObject.transform.parent.GetComponent<Image>().sprite = _achiveOpen;
            _achiveCase_3.gameObject.SetActive(true);
            _achiveCase_3.sprite = DataController.gameSettingsData.gameAchivemants[2 + page].achiveIco;
        }
        else
        {
            _achiveCase_3.gameObject.transform.parent.GetComponent<Image>().sprite = _achiveClose;
            _achiveCase_3.gameObject.SetActive(false);
        }

        if (3 + page < achivesLendth)
        {
            _achiveCase_4.gameObject.transform.parent.GetComponent<Image>().sprite = _achiveOpen;
            _achiveCase_4.gameObject.SetActive(true);
            _achiveCase_4.sprite = DataController.gameSettingsData.gameAchivemants[3 + page].achiveIco;
        }
        else
        {
            _achiveCase_4.gameObject.transform.parent.GetComponent<Image>().sprite = _achiveClose;
            _achiveCase_4.gameObject.SetActive(false);
        }

        if (4 + page < achivesLendth)
        {
            _achiveCase_5.gameObject.transform.parent.GetComponent<Image>().sprite = _achiveOpen;
            _achiveCase_5.gameObject.SetActive(true);
            _achiveCase_5.sprite = DataController.gameSettingsData.gameAchivemants[4 + page].achiveIco;
        }
        else
        {
            _achiveCase_5.gameObject.transform.parent.GetComponent<Image>().sprite = _achiveClose;
            _achiveCase_5.gameObject.SetActive(false);
        }
    }

    #endregion

    #region WAIT_SWITCH

    /// <summary> Задержка при переходе  главное меню </summary>
    private IEnumerator WaitToMenuSwitch(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        string mainText;

        if (DataController.gameSettingsData.lastPart != null) mainText = "Продолжить";
        else mainText = "Новая игра";

        _menuButton_1.text = mainText;
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