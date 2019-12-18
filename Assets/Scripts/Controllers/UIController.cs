using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary> Замена текста на кнопках и контроль UI </summary>
public class UIController : MonoBehaviour
{
    public static UIController uIContr;

    #region VARIABLES

    private bool isEventStarted; // Вы в главе евента
    private float timeForEvent; // Время на эвент

    [Header("Базовые")]

    public float gameWaitToSwitch;

    public RectTransform eyeCenter;

    [Header("Достижения")]

    public Sprite achiveClose;
    public Sprite achiveOpen;

    public Image achiveCase_1;
    public Image achiveCase_2;
    public Image achiveCase_3;
    public Image achiveCase_4;
    public Image achiveCase_5;

    public Text achiveDescript_txt;

    [Header("Главное меню")]

    public Text menuButton_1;
    public Text menuButton_2;
    public Text menuButton_3;
    public Text menuButton_4;

    [Header("Игровая глава")]

    public Text gameMain_Txt;
    public Text gameButton_1_Txt;
    public Text gameButton_2_Txt;
    public Text gameButton_3_Txt;
    public Image timerImage;

    [Header("Сообщения в игре")]

    public Text gameMessageInventoryTxt;
    public Text gameMessageCharacterTxt;
    public Text gameMessageMapTxt;
    public Text gameMessageNotesTxt;

    [Header("Инвентарь")]

    public Image inventCase_1;
    public Image inventCase_2;
    public Image inventCase_3;
    public Image inventCase_4;
    public Image inventCase_5;
    public Image inventCase_6;
    public Image inventCase_7;
    public Image inventCase_8;

    [Header("Эффекты в меню персонажа")]

    public Image effectCase_1;
    public Image effectCase_2;
    public Image effectCase_3;
    public Image effectCase_4;
    public Image effectCase_5;
    public Image effectCase_6;

    private Gyroscope _mainGyro;
    private bool _isGyroEnable;
    private Vector2 _eyeFixPosition;

    #endregion

    private void Start()
    {
        uIContr = this;
        TimeEvent(false, 0f);
        ConnectGyroscope();
    }

    private void Update()
    {
        if (isEventStarted) TimerEventVision();
        EyeWatching();
    }

    #region VISUAL_EFFECTS

    /// <summary> Старт евента </summary>
    public static void TimeEvent(bool isStart, float timeSec)
    {
        uIContr.isEventStarted = isStart;
        if (isStart) uIContr.timeForEvent = (1 / timeSec);
        else uIContr.timeForEvent = 0f;
    }

    /// <summary> Таймер в главе евента </summary>
    private void TimerEventVision()
    {
        if (timerImage.fillAmount > 0) timerImage.fillAmount -= timeForEvent * Time.deltaTime;
        else
        {
            TimeEvent(false, 0f);
            GetComponent<AnimController>().FinalEvent(false);
        }
    }

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
            eyeCenter.anchoredPosition = new Vector2(0, 40);
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

            eyeCenter.anchoredPosition = _eyeFixPosition;
        }
    }

    #endregion

    #region TEXT_CONTROL

    /// <summary> Открыть главное меню </summary>
    public void MenuOpenMain() => StartCoroutine(WaitToMenuSwitch(0.2f));

    /// <summary> Открыть меню настроек </summary>
    public void MenuOpenSettings() => StartCoroutine(WaitToSettingsSwitch(0.2f));

    /// <summary> Открыть меню автора </summary>
    public void MenuOpenAbout() => StartCoroutine(WaitToAboutSwitch(0.2f));

    /// <summary> Замена основного текста в игре </summary>
    public void GameMain(string newTxt) => StartCoroutine(WaitToMainTextSwitch(gameWaitToSwitch, newTxt));

    /// <summary> Замена текста на первой игровой кнопке </summary>
    public void GameButton_1(string newTxt) => StartCoroutine(WaitToGameButton_1_Switch(gameWaitToSwitch, newTxt));

    /// <summary> Замена текста на второй игровой кнопке </summary>
    public void GameButton_2(string newTxt) => StartCoroutine(WaitToGameButton_2_Switch(gameWaitToSwitch, newTxt));

    /// <summary> Замена текста на третьей игровой кнопке </summary>
    public void GameButton_3(string newTxt) => StartCoroutine(WaitToGameButton_3_Switch(gameWaitToSwitch, newTxt));

    /// <summary> Текст сообщения инвентаря </summary>
    public void GameMessageInventory(string newTxt) => gameMessageInventoryTxt.text = newTxt;

    /// <summary> Текст сообщения персонажа </summary>
    public void GameMessageCharacter(string newTxt) => gameMessageCharacterTxt.text = newTxt;

    /// <summary> Текст сообщения карты </summary>
    public void GameMessageMap(string newTxt) => gameMessageMapTxt.text = newTxt;

    /// <summary> Текст сообщения заметок </summary>
    public void GameMessageNotes(string newTxt) => gameMessageNotesTxt.text = newTxt;

    #endregion

    #region UI_CONTROL

    /// <summary> Отобразить инвентарь </summary>
    public void ShowInventory(int page)
    {
        int InventLendth = DataController.playerData.playerInventory.Count;

        page *= 8;

        if (0 + page < InventLendth)
        {
            inventCase_1.gameObject.SetActive(true);
            inventCase_1.sprite = DataController.playerData.playerInventory[0 + page].itemIco;
        }
        else inventCase_8.gameObject.SetActive(false);

        if (1 + page < InventLendth)
        {
            inventCase_2.gameObject.SetActive(true);
            inventCase_2.sprite = DataController.playerData.playerInventory[1 + page].itemIco;
        }
        else inventCase_8.gameObject.SetActive(false);

        if (2 + page < InventLendth)
        {
            inventCase_3.gameObject.SetActive(true);
            inventCase_3.sprite = DataController.playerData.playerInventory[2 + page].itemIco;
        }
        else inventCase_8.gameObject.SetActive(false);

        if (3 + page < InventLendth)
        {
            inventCase_4.gameObject.SetActive(true);
            inventCase_4.sprite = DataController.playerData.playerInventory[3 + page].itemIco;
        }
        else inventCase_8.gameObject.SetActive(false);

        if (4 + page < InventLendth)
        {
            inventCase_5.gameObject.SetActive(true);
            inventCase_5.sprite = DataController.playerData.playerInventory[4 + page].itemIco;
        }
        else inventCase_8.gameObject.SetActive(false);

        if (5 + page < InventLendth)
        {
            inventCase_6.gameObject.SetActive(true);
            inventCase_6.sprite = DataController.playerData.playerInventory[5 + page].itemIco;
        }
        else inventCase_8.gameObject.SetActive(false);

        if (6 + page < InventLendth)
        {
            inventCase_7.gameObject.SetActive(true);
            inventCase_7.sprite = DataController.playerData.playerInventory[6 + page].itemIco;
        }
        else inventCase_8.gameObject.SetActive(false);

        if (7 + page < InventLendth)
        {
            inventCase_8.gameObject.SetActive(true);
            inventCase_8.sprite = DataController.playerData.playerInventory[7 + page].itemIco;
        }
        else inventCase_8.gameObject.SetActive(false);

    }

    /// <summary> Отобразить еффекты на персонаже </summary>
    public void ShowEffects()
    {
        int InventLendth = DataController.playerData.playerEffects.Count;

        if (0 < InventLendth)
        {
            effectCase_1.gameObject.SetActive(true);
            effectCase_1.sprite = DataController.playerData.playerEffects[0].icoEffect;
        }
        else effectCase_1.gameObject.SetActive(false);

        if (1 < InventLendth)
        {
            effectCase_2.gameObject.SetActive(true);
            effectCase_2.sprite = DataController.playerData.playerEffects[1].icoEffect;
        }
        else effectCase_2.gameObject.SetActive(false);

        if (2 < InventLendth)
        {
            effectCase_3.gameObject.SetActive(true);
            effectCase_3.sprite = DataController.playerData.playerEffects[2].icoEffect;
        }
        else effectCase_3.gameObject.SetActive(false);

        if (3 < InventLendth)
        {
            effectCase_4.gameObject.SetActive(true);
            effectCase_4.sprite = DataController.playerData.playerEffects[3].icoEffect;
        }
        else effectCase_4.gameObject.SetActive(false);

        if (4 < InventLendth)
        {
            effectCase_5.gameObject.SetActive(true);
            effectCase_5.sprite = DataController.playerData.playerEffects[4].icoEffect;
        }
        else effectCase_5.gameObject.SetActive(false);

        if (5 < InventLendth)
        {
            effectCase_6.gameObject.SetActive(true);
            effectCase_6.sprite = DataController.playerData.playerEffects[5].icoEffect;
        }
        else effectCase_6.gameObject.SetActive(false);
    }

    /// <summary> Отобразить достижения </summary>
    public void ShowAchive(int page)
    {
        int achivesLendth = DataController.gameSettingsData.gameAchivemants.Count;

        page *= 5;

        if (0 + page < achivesLendth)
        {
            achiveCase_1.gameObject.transform.parent.GetComponent<Image>().sprite = achiveOpen;
            achiveCase_1.gameObject.SetActive(true);
            achiveCase_1.sprite = DataController.gameSettingsData.gameAchivemants[0 + page].achiveIco;
        }
        else
        {
            achiveCase_1.gameObject.transform.parent.GetComponent<Image>().sprite = achiveClose;
            achiveCase_1.gameObject.SetActive(false);
        }

        if (1 + page < achivesLendth)
        {
            achiveCase_2.gameObject.transform.parent.GetComponent<Image>().sprite = achiveOpen;
            achiveCase_2.gameObject.SetActive(true);
            achiveCase_2.sprite = DataController.gameSettingsData.gameAchivemants[1 + page].achiveIco;
        }
        else
        {
            achiveCase_2.gameObject.transform.parent.GetComponent<Image>().sprite = achiveClose;
            achiveCase_2.gameObject.SetActive(false);
        }

        if (2 + page < achivesLendth)
        {
            achiveCase_3.gameObject.transform.parent.GetComponent<Image>().sprite = achiveOpen;
            achiveCase_3.gameObject.SetActive(true);
            achiveCase_3.sprite = DataController.gameSettingsData.gameAchivemants[2 + page].achiveIco;
        }
        else
        {
            achiveCase_3.gameObject.transform.parent.GetComponent<Image>().sprite = achiveClose;
            achiveCase_3.gameObject.SetActive(false);
        }

        if (3 + page < achivesLendth)
        {
            achiveCase_4.gameObject.transform.parent.GetComponent<Image>().sprite = achiveOpen;
            achiveCase_4.gameObject.SetActive(true);
            achiveCase_4.sprite = DataController.gameSettingsData.gameAchivemants[3 + page].achiveIco;
        }
        else
        {
            achiveCase_4.gameObject.transform.parent.GetComponent<Image>().sprite = achiveClose;
            achiveCase_4.gameObject.SetActive(false);
        }

        if (4 + page < achivesLendth)
        {
            achiveCase_5.gameObject.transform.parent.GetComponent<Image>().sprite = achiveOpen;
            achiveCase_5.gameObject.SetActive(true);
            achiveCase_5.sprite = DataController.gameSettingsData.gameAchivemants[4 + page].achiveIco;
        }
        else
        {
            achiveCase_5.gameObject.transform.parent.GetComponent<Image>().sprite = achiveClose;
            achiveCase_5.gameObject.SetActive(false);
        }
    }

    /// <summary> Показать подробности о достижении </summary>
    public void ShowAchiveDescript(Achivemants achive) => achiveDescript_txt.text = achive.achiveDescript;

    #endregion

    #region WAIT_SWITCH

    /// <summary> Задержка при переходе  главное меню </summary>
    private IEnumerator WaitToMenuSwitch(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        string mainText;

        if (DataController.gameSettingsData.lastPart != null) mainText = "Продолжить";
        else mainText = "Новая игра";

        menuButton_1.text = mainText;
        menuButton_2.text = "Настройки";
        menuButton_3.text = "Об Авторах";
        menuButton_4.text = "Достижения";
    }

    /// <summary> Задержка при переходе в настройки </summary>
    private IEnumerator WaitToSettingsSwitch(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        menuButton_1.text = "Звук";
        menuButton_2.text = "Вибриция";
        menuButton_3.text = "Эффекты";
        menuButton_4.text = "Назад";
    }

    /// <summary> Задержка при переходе в меню автора </summary>
    private IEnumerator WaitToAboutSwitch(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        menuButton_4.text = "Назад";
    }

    /// <summary> Задержка замена основного текста в игре </summary>
    private IEnumerator WaitToMainTextSwitch(float waitTime, string newTxt)
    {
        yield return new WaitForSeconds(waitTime);

        gameMain_Txt.text = newTxt;
    }

    /// <summary> Задержка при замене текста на первой кнопке </summary>
    private IEnumerator WaitToGameButton_1_Switch(float waitTime, string newTxt)
    {
        yield return new WaitForSeconds(waitTime);

        gameButton_1_Txt.text = newTxt;
    }

    /// <summary> Задержка при замене текста на второй кнопке </summary>
    private IEnumerator WaitToGameButton_2_Switch(float waitTime, string newTxt)
    {
        yield return new WaitForSeconds(waitTime);

        gameButton_2_Txt.text = newTxt;
    }

    /// <summary> Задержка при замене текста на третей кнопке </summary>
    private IEnumerator WaitToGameButton_3_Switch(float waitTime, string newTxt)
    {
        yield return new WaitForSeconds(waitTime);

        gameButton_3_Txt.text = newTxt;
    }

    #endregion
}