using System.Collections;
using UnityEngine;

/// <summary> Анимация и переходы </summary>
public class AnimController : MonoBehaviour
{
    public static AnimController moveContr;

    #region VARIABLES

    [HideInInspector] public static Animator _mainAnimator;
    private UIController _uiContr;
    private PlayerController _plContr;

    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private GameObject _gameCanvas;

    /// <summary> Первая глава сюжета </summary>
    public GamePart firstPart;

    /// <summary> Глава запускаемая по нажатию "Начало" </summary>
    [HideInInspector] public static GamePart thisPart;

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

    /// <summary> Привязка компонентов </summary>
    public void Init()
    {
        _mainAnimator = GetComponent<Animator>();
        _uiContr = GetComponent<UIController>();
        _plContr = GetComponent<PlayerController>();

        _mainAnimator.SetBool("Settings_1_St", DataController.gameSettingsData.isSoundCheck);
        _mainAnimator.SetBool("Settings_2_St", DataController.gameSettingsData.isVibrationCheck);
        _mainAnimator.SetBool("Settings_3_St", DataController.gameSettingsData.isEffectCheck);
        
        _uiContr.MenuOpenMain();

        _isAchiveDetail = false;
    }

    #region PART_START

    /// <summary> Запуск следующей главы </summary>
    public static void NextPart(GamePart nextPart)
    {
        _mainAnimator.SetTrigger("SwitchGameText");
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
        _mainAnimator.SetInteger("GameStady", 0);
        _uiContr.GameMain(textPart.mainText);
        _uiContr.GameButton_1(textPart.buttonText_1);
    }

    /// <summary> Запуск главы выбора </summary>
    private void ShowChangePart(ChangePart changePart)
    {
        // Код
        _mainAnimator.SetInteger("GameStady", 1);
        _uiContr.GameMain(changePart.mainText);
        _uiContr.GameButton_1(changePart.buttonText_1);
        _uiContr.GameButton_2(changePart.buttonText_2);
    }

    /// <summary> Запуск главы боя </summary>
    private void ShowBattlePart(BattlePart battlePart)
    {
        // Код
        _mainAnimator.SetInteger("GameStady", 2);
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

    #region GAME_BUTTONS

    /// <summary> Нажатие на кнопки в игре </summary>
    public void GameButtonDown(int buttonID)
    {
        switch (buttonID)
        {
            case 0: _mainAnimator.SetBool("GameButton_1", true); break;     // Кнопка 1 в игре
            case 1: _mainAnimator.SetBool("GameButton_2", true); break;     // Кнопка 2 в игре
            case 2: _mainAnimator.SetBool("GameButton_3", true); break;     // Кнопка 3 в игре
            case 3: _mainAnimator.SetBool("Btb_Inventory", true); break;    // Вход в инвентарь
            case 4: _mainAnimator.SetBool("Btb_Map", true); break;          // Открыть карту
            case 5: _mainAnimator.SetBool("Btb_Player", true); break;       // Открыть меню персонажа
            case 6: _mainAnimator.SetBool("Btb_Notes", true); break;        // Открыть меню заметок
        }
    }

    /// <summary> Отпускание кнопок в игре </summary>
    public void GameButtonUp(int buttonID)
    {
        switch (buttonID)
        {
            case 0:
                // Первая кнопка в игре

                _mainAnimator.SetBool("GameButton_1", false);
                if(thisPart.movePart_1 != null) NextPart(thisPart.movePart_1);
                break;

            case 1:
                // Вторая кнопка в игре

                _mainAnimator.SetBool("GameButton_2", false);
                if (thisPart.movePart_2 != null) NextPart(thisPart.movePart_2);
                break;

            case 2:
                // Третья кнопка в игре

                _mainAnimator.SetBool("GameButton_3", false);
                if (thisPart.movePart_3 != null) NextPart(thisPart.movePart_3);
                break;

            case 3:
                // Инвентарь

                _mainAnimator.SetBool("Btb_Inventory", false);

                if (_mainAnimator.GetInteger("GameStady") != 5)
                {
                    _lastPartType = _mainAnimator.GetInteger("GameStady");
                    _mainAnimator.SetInteger("GameStady", 5);
                }
                else _mainAnimator.SetInteger("GameStady", _lastPartType);
                break;

            case 4:
                // Карта

                _mainAnimator.SetBool("Btb_Map", false);
                break;

            case 5:
                // Персонаж

                _mainAnimator.SetBool("Btb_Player", false);

                if (_mainAnimator.GetInteger("GameStady") != 6)
                {
                    _lastPartType = _mainAnimator.GetInteger("GameStady");
                    _mainAnimator.SetInteger("GameStady", 6);
                }
                else _mainAnimator.SetInteger("GameStady", _lastPartType);
                break;

            case 6:
                // Заметки

                _mainAnimator.SetBool("Btb_Notes", false);
                break;
        }
    }

    #endregion

    #region GAME_HELP_BUTTONS

    /// <summary> Взаимодействие с кнопкой в меню инвентаря </summary>
    public void GameHelpInventoryButton(bool btStady, int btType)
    {
        switch (btType)
        {
            case 0:
                // Подробнее о предмете
                if (btStady) _mainAnimator.SetBool("GameInvent_Bt_Info", true);
                else
                {
                    _mainAnimator.SetBool("GameInvent_Bt_Info", false);
                }
                break;

            case 1:
                // Ячейка инвентаря 1
                if (btStady) _mainAnimator.SetBool("GameInvent_Case_1", true);
                else
                {
                    _mainAnimator.SetBool("GameInvent_Case_1", false);
                }
                break;

            case 2:
                // Ячейка инвентаря 2
                if (btStady) _mainAnimator.SetBool("GameInvent_Case_2", true);
                else
                {
                    _mainAnimator.SetBool("GameInvent_Case_2", false);
                }
                break;

            case 3:
                // Ячейка инвентаря 3
                if (btStady) _mainAnimator.SetBool("GameInvent_Case_3", true);
                else
                {
                    _mainAnimator.SetBool("GameInvent_Case_3", false);
                }
                break;

            case 4:
                // Ячейка инвентаря 4
                if (btStady) _mainAnimator.SetBool("GameInvent_Case_4", true);
                else
                {
                    _mainAnimator.SetBool("GameInvent_Case_4", false);
                }
                break;

            case 5:
                // Ячейка инвентаря 5
                if (btStady) _mainAnimator.SetBool("GameInvent_Case_5", true);
                else
                {
                    _mainAnimator.SetBool("GameInvent_Case_5", false);
                }
                break;

            case 6:
                // Ячейка инвентаря 6
                if (btStady) _mainAnimator.SetBool("GameInvent_Case_6", true);
                else
                {
                    _mainAnimator.SetBool("GameInvent_Case_6", false);
                }
                break;

            case 7:
                // Ячейка инвентаря 7
                if (btStady) _mainAnimator.SetBool("GameInvent_Case_7", true);
                else
                {
                    _mainAnimator.SetBool("GameInvent_Case_7", false);
                }
                break;

            case 8:
                // Ячейка инвентаря 8
                if (btStady) _mainAnimator.SetBool("GameInvent_Case_8", true);
                else
                {
                    _mainAnimator.SetBool("GameInvent_Case_8", false);
                }
                break;

            case 9:
                // Использовать предмет
                if (btStady) _mainAnimator.SetBool("GameInvent_Bt_Use", true);
                else
                {
                    _mainAnimator.SetBool("GameInvent_Bt_Use", false);
                }
                break;

            case 10:
                // Выбросить предмет
                if (btStady) _mainAnimator.SetBool("GameInvent_Bt_Remove", true);
                else
                {
                    _mainAnimator.SetBool("GameInvent_Bt_Remove", false);
                }
                break;

            case 11:
                // Страница влево
                if (btStady) _mainAnimator.SetBool("GameInvent_Bt_Left", true);
                else
                {
                    _mainAnimator.SetBool("GameInvent_Bt_Left", false);
                }
                break;

            case 12:
                // Страница вправо
                if (btStady) _mainAnimator.SetBool("GameInvent_Bt_Right", true);
                else
                {
                    _mainAnimator.SetBool("GameInvent_Bt_Right", false);
                }
                break;
        }
    }

    /// <summary> Взаимодействие с кнопкой в меню персонажа </summary>
    public void GameHelpPlayerButton(bool btStady, int btType)
    {
        // TODO : Меню персонажа
    }

    /// <summary> Взаимодействие с кнопкой в меню карты </summary>
    public void GameHelpMapButton(bool btStady, int btType)
    {
        // TODO : Меню карты
    }

    /// <summary> Взаимодействие с кнопкой в меню заметок </summary>
    public void GameHelpNotesButton(bool btStady, int btType)
    {
        // TODO : Меню заметок
    }

    #endregion

    #region MENU_BUTTONS

    /// <summary> Нажатие на кнопок в меню </summary>
    public void MenuButtonDown(int buttonID)
    {
        switch (buttonID)
        {
            case 0: MenuB1(true); break;
            case 1: MenuB2(true); break;
            case 2: MenuB3(true); break;
            case 3: MenuB4(true); break;

            case 4:
                if (!_isAchiveDetail && _achiveDisplayPage > 0)
                {
                    _achiveDisplayPage--;
                    _uiContr.ShowAchive(_achiveDisplayPage);
                    _mainAnimator.SetBool("Achives_Left", true);
                }
                break; // Влево в меню достижений

            case 5:
                if (!_isAchiveDetail && DataController.gameSettingsData.gameAchivemants.Count > 5 * (_achiveDisplayPage + 1))
                {
                    _achiveDisplayPage++;
                    _uiContr.ShowAchive(_achiveDisplayPage);
                    _mainAnimator.SetBool("Achives_Right", true);
                }
                break; // Вправо в меню достижений

            case 7: AchivemantCase(true, 1); break; // Ячейка достижения 1
            case 8: AchivemantCase(true, 2); break; // Ячейка достижения 2
            case 9: AchivemantCase(true, 3); break; // Ячейка достижения 3
            case 10: AchivemantCase(true, 4); break; // Ячейка достижения 4
            case 11: AchivemantCase(true, 5); break; // Ячейка достижения 5
        }
    }

    /// <summary> Отпускание кнопок в меню </summary>
    public void MenuButtonUp (int buttonID)
    {
        switch (buttonID)
        {
            case 0: MenuB1(false); break;
            case 1: MenuB2(false); break;
            case 2: MenuB3(false); break;
            case 3: MenuB4(false); break;

            case 4:
                // Влево в меню достижений

                if (!_isAchiveDetail && _achiveDisplayPage > 0)
                {
                    _mainAnimator.SetBool("Achives_Left", false);
                    _mainAnimator.SetTrigger("AchiveSlidePage");
                }
                break;

            case 5:
                // Вправо в меню достижений

                if (!_isAchiveDetail && 5 * (_achiveDisplayPage + 1) < DataController.gameSettingsData.gameAchivemants.Count)
                {
                    _mainAnimator.SetBool("Achives_Right", false);
                    _mainAnimator.SetTrigger("AchiveSlidePage");
                }
                break;

            case 6:
                // Выход из Описания достижения

                _mainAnimator.SetBool("AchiveDescript", false);
                _isAchiveDetail = false;

                break;

            case 7: AchivemantCase(false, 1); break; // Ячейка достижения 1
            case 8: AchivemantCase(false, 2); break; // Ячейка достижения 2
            case 9: AchivemantCase(false, 3); break; // Ячейка достижения 3
            case 10: AchivemantCase(false, 4); break; // Ячейка достижения 4
            case 11: AchivemantCase(false, 5); break; // Ячейка достижения 5

            case 12: _mainAnimator.SetBool("StartGameDescript", false); break; // Выход из стартового превью
        }
    }

    /// <summary> Меню кнопка 1 </summary>
    private void MenuB1(bool press)
    {
        _mainAnimator.SetBool("Menu_1_Button", press);

        if (!press)
        {
            switch (_mainAnimator.GetInteger("MenuStady"))
            {
                case 0:
                    _mainAnimator.SetBool("StartGameDescript", true);
                    GameStart();
                    _mainAnimator.SetInteger("MenuStady", 9);

                    int partType;

                    if (thisPart is TextPart) partType = 0;
                    else if (thisPart is ChangePart) partType = 1;
                    else if (thisPart is BattlePart) partType = 2;
                    else if (thisPart is EventPart) partType = 3;
                    else partType = 4;

                    _mainAnimator.SetInteger("GameStady", partType);
                    StartCoroutine(HideStartDescriptMenu());

                    break;

                case 1:
                    _mainAnimator.SetBool("Settings_1_St", !_mainAnimator.GetBool("Settings_1_St"));
                    DataController.gameSettingsData.isSoundCheck = _mainAnimator.GetBool("Settings_1_St");
                    DataController.SaveSettingsData();
                    break;
            }
        }
    }

    /// <summary> Меню кнопка 2 </summary>
    private void MenuB2(bool press)
    {
        _mainAnimator.SetBool("Menu_2_Button", press);

        if (!press)
        {
            switch (_mainAnimator.GetInteger("MenuStady"))
            {
                case 0:
                    _uiContr.MenuOpenSettings();
                    _mainAnimator.SetTrigger("SwitchTextToSettings");
                    _mainAnimator.SetInteger("MenuStady", 1);
                    break;

                case 1:
                    _mainAnimator.SetBool("Settings_2_St", !_mainAnimator.GetBool("Settings_2_St"));
                    DataController.gameSettingsData.isVibrationCheck = _mainAnimator.GetBool("Settings_2_St");
                    DataController.SaveSettingsData();
                    break;
            }
        }
    }

    /// <summary> Меню кнопка 3 </summary>
    private void MenuB3(bool press)
    {
        _mainAnimator.SetBool("Menu_3_Button", press);

        if (!press)
        {
            switch (_mainAnimator.GetInteger("MenuStady"))
            {
                case 0:
                    _uiContr.MenuOpenAbout();
                    _mainAnimator.SetTrigger("SwitchTextToAbout");
                    _mainAnimator.SetInteger("MenuStady", 2);
                    break;

                case 1:
                    _mainAnimator.SetBool("Settings_3_St", !_mainAnimator.GetBool("Settings_3_St"));
                    DataController.gameSettingsData.isEffectCheck = _mainAnimator.GetBool("Settings_3_St");
                    DataController.SaveSettingsData();
                    break;
            }
        }
    }

    /// <summary> Меню кнопка 4 </summary>
    private void MenuB4(bool press)
    {
        if (press)
        {
            _achiveDisplayPage = 0;

            switch (_mainAnimator.GetInteger("MenuStady"))
            {
                case 0:
                case 1:
                case 2:
                    _mainAnimator.SetBool("Menu_4_Button", true);
                    _uiContr.ShowAchive(_achiveDisplayPage);
                    break;

                default: _mainAnimator.SetBool("Achives_Back", true); break;
            }
        }
        else
        {
            switch (_mainAnimator.GetInteger("MenuStady"))
            {
                case 0:
                    _mainAnimator.SetBool("Menu_4_Button", press);
                    _mainAnimator.SetInteger("MenuStady", 3);
                    break;

                case 1:
                case 2:
                    _uiContr.MenuOpenMain();
                    _mainAnimator.SetBool("Menu_4_Button", press);
                    _mainAnimator.SetInteger("MenuStady", 0);
                    _mainAnimator.SetTrigger("ReturnToMenu");
                    break;

                default:
                    _mainAnimator.SetBool("Achives_Back", press);

                    if (!_isAchiveDetail) _mainAnimator.SetInteger("MenuStady", 0);
                    else
                    {
                        _mainAnimator.SetBool("AchiveDescript", false);
                        _isAchiveDetail = false;
                    }

                    break;
            }
        }
    }

    /// <summary> Ячейки достижений </summary>
    private void AchivemantCase(bool press, int idCase)
    {
        if ((_achiveDisplayPage * 5) + idCase <= DataController.gameSettingsData.gameAchivemants.Count)
        {
            _mainAnimator.SetBool("AchiveCase_" + idCase, press);

            if (!press)
            {
                if (!_isAchiveDetail)
                {
                    _mainAnimator.SetBool("AchiveDescript", true);
                    _isAchiveDetail = true;

                    // TODO : Смена текста описания
                }
                else
                {
                    _mainAnimator.SetBool("AchiveDescript", false);
                    _isAchiveDetail = false;
                }
            }
        }
    }

    #endregion

    #region ANIMATIONS_HELPERS

    /// <summary> Переход в игровое меню </summary>
    public void AnimMenuToGameSwitch()
    {
        _menuCanvas.SetActive(false);
        _gameCanvas.SetActive(true);
    }

    /// <summary> Задержка стартового меню </summary>
    private IEnumerator HideStartDescriptMenu()
    {
        yield return new WaitForSeconds(10f);

        _mainAnimator.SetBool("StartGameDescript", false);

    }

    #endregion
}