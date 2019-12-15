using System.Collections;
using UnityEngine;

/// <summary> Анимация и переходы </summary>
public class AnimController : MonoBehaviour
{
    public static AnimController moveContr;

    #region VARIABLES

    /// <summary> Глава запускаемая по нажатию "Начало" </summary>
    public static GamePart thisPart;
    public static Animator mainAnimator;

    /// <summary> Первая глава сюжета </summary>
    public GamePart firstPart;

    public GameObject menuCanvas;
    public GameObject gameCanvas;

    private UIController _uiContr;
    private PlayerController _plContr;

    private bool _isAchiveDetail; // Вы в меню описания достижения
    private int _lastPartType; // Тип последней главы

    private int _achiveSelectId; // Глава для вывода описания
    private int _achiveDisplayPage; // Отображаемая страница достижений

    #endregion

    /// <summary> Начало игры </summary>
    public void GameStart()
    {
        if (thisPart == null) thisPart = firstPart;
        NextPart(thisPart);
    }

    /// <summary> Инициализация </summary>
    public void Init()
    {
        mainAnimator = GetComponent<Animator>();
        _uiContr = GetComponent<UIController>();
        _plContr = GetComponent<PlayerController>();

        mainAnimator.SetBool("Settings_1_St", DataController.gameSettingsData.isSoundCheck);
        mainAnimator.SetBool("Settings_2_St", DataController.gameSettingsData.isVibrationCheck);
        mainAnimator.SetBool("Settings_3_St", DataController.gameSettingsData.isEffectCheck);
        
        _uiContr.MenuOpenMain();

        _isAchiveDetail = false;
    }

    #region PART_START

    /// <summary> Запуск следующей главы </summary>
    public static void NextPart(GamePart nextPart)
    {
        mainAnimator.SetTrigger("SwitchGameText");
        if (nextPart is TextPart) moveContr.ShowTextPart((TextPart)nextPart);
        else if (nextPart is ChangePart) moveContr.ShowChangePart((ChangePart)nextPart);
        else if (nextPart is BattlePart) moveContr.ShowBattlePart((BattlePart)nextPart);
        else if (nextPart is FinalPart) moveContr.ShowFinalPart((FinalPart)nextPart);
        else if (nextPart is EventPart) moveContr.ShowFinalPart((EventPart)nextPart);

        moveContr._plContr.EventsStart(nextPart.mainEvents);
        
        thisPart = nextPart;
    }

    /// <summary> Запуск текстовой главы </summary>
    private void ShowTextPart(TextPart textPart)
    {
        mainAnimator.SetInteger("GameStady", 0);
        _uiContr.GameMain(textPart.mainText);
        _uiContr.GameButton_1(textPart.buttonText_1);
    }

    /// <summary> Запуск главы выбора </summary>
    private void ShowChangePart(ChangePart changePart)
    {
        // Код
        mainAnimator.SetInteger("GameStady", 1);
        _uiContr.GameMain(changePart.mainText);
        _uiContr.GameButton_1(changePart.buttonText_1);
        _uiContr.GameButton_2(changePart.buttonText_2);
    }

    /// <summary> Запуск главы боя </summary>
    private void ShowBattlePart(BattlePart battlePart)
    {
        // Код
        mainAnimator.SetInteger("GameStady", 2);
        _uiContr.GameMain(battlePart.mainText);
        _uiContr.GameButton_1(battlePart.buttonText_1);
        _uiContr.GameButton_2(battlePart.buttonText_2);
        _uiContr.GameButton_3(battlePart.buttonText_3);
    }

    /// <summary> Запуск финальной главы </summary>
    private void ShowFinalPart(FinalPart finalPart)
    {
        // TODO : Запуск финальной главы
    }

    /// <summary> Запуск временного евента </summary>
    private void ShowFinalPart(EventPart eventPart)
    {
        // TODO : Запуск главы эвента
    }

    #endregion

    #region MAIN_MENU

    /// <summary> Главное меню </summary>
    public void MainMenuPress(int id, bool press)
    {
        switch (id)
        {
            case 1:
                mainAnimator.SetBool("Menu_1_Button", press);

                if (!press)
                {
                    switch (mainAnimator.GetInteger("MenuStady"))
                    {
                        case 0:
                            mainAnimator.SetBool("StartGameDescript", true);
                            GameStart();
                            mainAnimator.SetInteger("MenuStady", 9);

                            int partType;

                            if (thisPart is TextPart) partType = 0;
                            else if (thisPart is ChangePart) partType = 1;
                            else if (thisPart is BattlePart) partType = 2;
                            else if (thisPart is EventPart) partType = 3;
                            else partType = 4;

                            mainAnimator.SetInteger("GameStady", partType);
                            StartCoroutine(HideStartDescriptMenu());

                            break;

                        case 1:
                            mainAnimator.SetBool("Settings_1_St", !mainAnimator.GetBool("Settings_1_St"));
                            DataController.gameSettingsData.isSoundCheck = mainAnimator.GetBool("Settings_1_St");
                            DataController.SaveSettingsData();
                            break;
                    }
                }
                break;

            case 2:
                mainAnimator.SetBool("Menu_2_Button", press);

                if (!press)
                {
                    switch (mainAnimator.GetInteger("MenuStady"))
                    {
                        case 0:
                            _uiContr.MenuOpenSettings();
                            mainAnimator.SetTrigger("SwitchTextToSettings");
                            mainAnimator.SetInteger("MenuStady", 1);
                            break;

                        case 1:
                            mainAnimator.SetBool("Settings_2_St", !mainAnimator.GetBool("Settings_2_St"));
                            DataController.gameSettingsData.isVibrationCheck = mainAnimator.GetBool("Settings_2_St");
                            DataController.SaveSettingsData();
                            break;
                    }
                }
                break;

            case 3:
                mainAnimator.SetBool("Menu_3_Button", press);

                if (!press)
                {
                    switch (mainAnimator.GetInteger("MenuStady"))
                    {
                        case 0:
                            _uiContr.MenuOpenAbout();
                            mainAnimator.SetTrigger("SwitchTextToAbout");
                            mainAnimator.SetInteger("MenuStady", 2);
                            break;

                        case 1:
                            mainAnimator.SetBool("Settings_3_St", !mainAnimator.GetBool("Settings_3_St"));
                            DataController.gameSettingsData.isEffectCheck = mainAnimator.GetBool("Settings_3_St");
                            DataController.SaveSettingsData();
                            break;
                    }
                }
                break;

            case 4:
                if (press)
                {
                    _achiveDisplayPage = 0;

                    switch (mainAnimator.GetInteger("MenuStady"))
                    {
                        case 0:
                        case 1:
                        case 2:
                            mainAnimator.SetBool("Menu_4_Button", true);
                            _uiContr.ShowAchive(_achiveDisplayPage);
                            break;

                        default: mainAnimator.SetBool("Achives_Back", true); break;
                    }
                }
                else
                {
                    switch (mainAnimator.GetInteger("MenuStady"))
                    {
                        case 0:
                            mainAnimator.SetBool("Menu_4_Button", press);
                            mainAnimator.SetInteger("MenuStady", 3);
                            break;

                        case 1:
                        case 2:
                            _uiContr.MenuOpenMain();
                            mainAnimator.SetBool("Menu_4_Button", press);
                            mainAnimator.SetInteger("MenuStady", 0);
                            mainAnimator.SetTrigger("ReturnToMenu");
                            break;

                        default:
                            mainAnimator.SetBool("Achives_Back", press);

                            if (!_isAchiveDetail) mainAnimator.SetInteger("MenuStady", 0);
                            else
                            {
                                mainAnimator.SetBool("AchiveDescript", false);
                                _isAchiveDetail = false;
                            }

                            break;
                    }
                }
                break;
        }
    }

    /// <summary> Ячейки Достижений </summary>
    public void AchiveCaseMenuPress(int id, bool press)
    {
        if ((_achiveDisplayPage * 5) + id <= DataController.gameSettingsData.gameAchivemants.Count)
        {
            mainAnimator.SetBool("AchiveCase_" + id, press);

            if (!press)
            {
                if (!_isAchiveDetail)
                {
                    mainAnimator.SetBool("AchiveDescript", true);
                    _isAchiveDetail = true;

                    _uiContr.ShowAchiveDescript(DataController.gameSettingsData.gameAchivemants[(_achiveDisplayPage * 5) + (id - 1)]);
                }
                else
                {
                    mainAnimator.SetBool("AchiveDescript", false);
                    _isAchiveDetail = false;
                }
            }
        }
    }

    /// <summary> Дополнительные кнопки меню достижению </summary>
    public void AchiveMenuPress(int id, bool press)
    {
        if (press)
        {
            switch (id)
            {
                case 1:
                    if (!_isAchiveDetail && _achiveDisplayPage > 0)
                    {
                        _achiveDisplayPage--;
                        _uiContr.ShowAchive(_achiveDisplayPage);
                        mainAnimator.SetBool("Achives_Left", true);
                    }
                    break;

                case 2:
                    if (!_isAchiveDetail && DataController.gameSettingsData.gameAchivemants.Count > 5 * (_achiveDisplayPage + 1))
                    {
                        _achiveDisplayPage++;
                        _uiContr.ShowAchive(_achiveDisplayPage);
                        mainAnimator.SetBool("Achives_Right", true);
                    }
                    break;
            }
        }
        else
        {
            switch (id)
            {
                case 1:
                    if (!_isAchiveDetail && _achiveDisplayPage > 0)
                    {
                        mainAnimator.SetBool("Achives_Left", false);
                        mainAnimator.SetTrigger("AchiveSlidePage");
                    }
                    break;

                case 2:
                    if (!_isAchiveDetail && 5 * (_achiveDisplayPage + 1) < DataController.gameSettingsData.gameAchivemants.Count)
                    {
                        mainAnimator.SetBool("Achives_Right", false);
                        mainAnimator.SetTrigger("AchiveSlidePage");
                    }
                    break;

                case 3:
                    mainAnimator.SetBool("AchiveDescript", false);
                    _isAchiveDetail = false;
                    break;
            }
        }
    }

    /// <summary> Выход из первоначальной вставки </summary>
    public void PreviewExit(bool press)
    {
        if(!press) mainAnimator.SetBool("StartGameDescript", false);
    }

    #endregion

    #region GAME_MENU

    /// <summary> Базовая игровая глава </summary>
    public void GamePartPress(int id, bool press)
    {
        switch (id)
        {
            case 1:
                mainAnimator.SetBool("GameButton_" + id, false);
                if (thisPart.movePart_1 != null && !press) NextPart(thisPart.movePart_1);
                break;
            case 2:
                mainAnimator.SetBool("GameButton_" + id, false);
                if (thisPart.movePart_2 != null && !press) NextPart(thisPart.movePart_2);
                break;
            case 3:
                mainAnimator.SetBool("GameButton_" + id, false);
                if (thisPart.movePart_3 != null && !press) NextPart(thisPart.movePart_3);
                break;
            case 4:
                // TODO : Далее в главах слайдшоу и текстовой вставки
                break;
        }
    }

    /// <summary> Глава эвента </summary>
    public void EventPartPress(int id, bool press)
    {
        switch (id)
        {
            case 1:
                // TODO : Влево в главе евента
                break;

            case 2:
                // TODO : Вправо в главе евента
                break;
        }
    }

    /// <summary> Глава загадки </summary>
    public void MazePartPress(int id, bool press)
    {
        // TODO : Разработка
    }

    #endregion

    #region GAME_HELP

    /// <summary> Меню инвентаря </summary>
    public void InventoryMenuPress(int id, bool press)
    {
        switch (id)
        {
            case 0:
                mainAnimator.SetBool("Btb_Inventory", press);
                if (!press)
                {
                    if (mainAnimator.GetInteger("GameStady") != 5)
                    {
                        _lastPartType = mainAnimator.GetInteger("GameStady");
                        mainAnimator.SetInteger("GameStady", 5);
                    }
                    else mainAnimator.SetInteger("GameStady", _lastPartType);
                }
                break;

            case 1: case 2: case 3: case 4: case 5: case 6: case 7: case 8:
                mainAnimator.SetBool("GameInvent_Case_" + id, press);
                if (!press)
                {
                    // TODO : Логика "Выбор предмета в ячейке"
                }
                break;

            case 9:
                mainAnimator.SetBool("GameInvent_Bt_Info", press);
                if (!press)
                {
                    // TODO : Логика "Подробности о предмете"
                }
                break;

            case 10:
                mainAnimator.SetBool("GameInvent_Bt_Use", press);
                if (!press)
                {
                    // TODO : Логика "Использовать предмет"
                }
                break;

            case 11:
                mainAnimator.SetBool("GameInvent_Bt_Remove", press);
                if (!press)
                {
                    // TODO : Логика "Выбросить предмет"
                }
                break;

            case 12:
                mainAnimator.SetBool("GameInvent_Bt_Left", press);
                if (!press)
                {
                    // TODO : Логика "Страница влево"
                }
                break;

            case 13:
                mainAnimator.SetBool("GameInvent_Bt_Right", press);
                if (!press)
                {
                    // TODO : Логика "Страница вправо"
                }
                break;
        }
    }

    /// <summary> Меню персонажа </summary>
    public void PlayerMenuPress(int id, bool press)
    {
        switch (id)
        {
            case 0:
                mainAnimator.SetBool("Btb_Player", press);
                if (!press)
                {
                    if (mainAnimator.GetInteger("GameStady") != 6)
                    {
                        _lastPartType = mainAnimator.GetInteger("GameStady");
                        mainAnimator.SetInteger("GameStady", 6);
                    }
                    else mainAnimator.SetInteger("GameStady", _lastPartType);
                }
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    /// <summary> Меню карты </summary>
    public void MapMenuPress(int id, bool press)
    {
        switch (id)
        {
            case 0:
                mainAnimator.SetBool("Btb_Map", press);
                if (!press)
                {
                    // TODO : Логика перехода в меню карты
                }
                break;
        }
    }

    /// <summary> Меню заметок </summary>
    public void NotesMenuPress(int id, bool press)
    {
        switch (id)
        {
            case 0:
                mainAnimator.SetBool("Btb_Notes", press);
                if (!press)
                {
                    // TODO : Логика перехода в меню заметок
                }
                break;
        }
    }

    #endregion

    #region ANIMATIONS_HELPERS

    /// <summary> Переход в игровое меню </summary>
    public void AnimMenuToGameSwitch()
    {
        menuCanvas.SetActive(false);
        gameCanvas.SetActive(true);
    }

    /// <summary> Задержка стартового меню </summary>
    private IEnumerator HideStartDescriptMenu()
    {
        yield return new WaitForSeconds(10f);

        mainAnimator.SetBool("StartGameDescript", false);

    }

    #endregion
}