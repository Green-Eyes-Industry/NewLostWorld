using System.Collections;
using UnityEngine;

/// <summary> Анимация и переходы </summary>
public class MoveController : MonoBehaviour
{
    #region CONNECTIONS

    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private GameObject _gameCanvas;

    /// <summary> Первая глава сюжета </summary>
    public GamePart firstPart;
    public static MoveController moveC;

    private TextController _textController;
    [HideInInspector] public static Animator _mainAnimator;
    private PlayerController _playerController;

    private bool _isAchiveDetail; // Вы в меню описания достижения
    private int _lastPartType;

    #endregion

    #region VARIABLES

    /// <summary> Глава запускаемая по нажатию "Начало" </summary>
    [HideInInspector] public static GamePart thisPart;

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
        _textController = GetComponent<TextController>();
        _playerController = GetComponent<PlayerController>();

        _isAchiveDetail = false;
    }

    #region PART_START

    /// <summary> Запуск следующей главы </summary>
    public static void NextPart(GamePart nextPart)
    {
        _mainAnimator.SetTrigger("SwitchGameText");
        if (nextPart is TextPart) moveC.ShowTextPart((TextPart)nextPart);
        else if (nextPart is ChangePart) moveC.ShowChangePart((ChangePart)nextPart);
        else if (nextPart is BattlePart) moveC.ShowBattlePart((BattlePart)nextPart);
        else moveC.ShowFinalPart((FinalPart)nextPart);

        moveC._playerController.EventsStart(nextPart.mainEvents);
        
        thisPart = nextPart;
    }

    /// <summary> Запуск текстовой главы </summary>
    private void ShowTextPart(TextPart textPart)
    {
        _mainAnimator.SetInteger("GameStady", 0);
        _textController.GameMain(textPart.mainText);
        _textController.GameButton_1(textPart.buttonText_1);
    }

    /// <summary> Запуск главы выбора </summary>
    private void ShowChangePart(ChangePart changePart)
    {
        // Код
        _mainAnimator.SetInteger("GameStady", 1);
        _textController.GameMain(changePart.mainText);
        _textController.GameButton_1(changePart.buttonText_1);
        _textController.GameButton_2(changePart.buttonText_2);
    }

    /// <summary> Запуск главы боя </summary>
    private void ShowBattlePart(BattlePart battlePart)
    {
        // Код
        _mainAnimator.SetInteger("GameStady", 2);
        _textController.GameMain(battlePart.mainText);
        _textController.GameButton_1(battlePart.buttonText_1);
        _textController.GameButton_2(battlePart.buttonText_2);
        _textController.GameButton_3(battlePart.buttonText_3);
    }

    /// <summary> Запуск финальной главы </summary>
    private void ShowFinalPart(FinalPart finalPart)
    {
        // Код
        Debug.Log("Это не текстовая глава а финальная");
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

    }

    /// <summary> Взаимодействие с кнопкой в меню карты </summary>
    public void GameHelpMapButton(bool btStady, int btType)
    {

    }

    /// <summary> Взаимодействие с кнопкой в меню заметок </summary>
    public void GameHelpNotesButton(bool btStady, int btType)
    {

    }


    #endregion

    #region MENU_BUTTONS

    /// <summary> Нажатие на кнопок в меню </summary>
    public void MenuButtonDown(int buttonID)
    {
        switch (buttonID)
        {
            case 0: _mainAnimator.SetBool("Menu_1_Button", true); break;    // Меню 1 кнопка
            case 1: _mainAnimator.SetBool("Menu_2_Button", true); break;    // Меню 2 кнопка
            case 2: _mainAnimator.SetBool("Menu_3_Button", true); break;    // Меню 3 кнопка

            case 3:
                // Меню 4 кнопка

                switch (_mainAnimator.GetInteger("MenuStady"))
                {
                    case 0:
                    case 1:
                    case 2: _mainAnimator.SetBool("Menu_4_Button", true); break;
                    default: _mainAnimator.SetBool("Achives_Back", true); break;
                }

                break;

            case 4: if (!_isAchiveDetail) _mainAnimator.SetBool("Achives_Left", true); break; // Влево в меню достижений
            case 5: if (!_isAchiveDetail) _mainAnimator.SetBool("Achives_Right", true); break; // Вправо в меню достижений

            case 7: if (!_isAchiveDetail) _mainAnimator.SetBool("AchiveCase_1", true); break; // Ячейка достижения 1
            case 8: if (!_isAchiveDetail) _mainAnimator.SetBool("AchiveCase_2", true); break; // Ячейка достижения 2
            case 9: if (!_isAchiveDetail) _mainAnimator.SetBool("AchiveCase_3", true); break; // Ячейка достижения 3
            case 10: if (!_isAchiveDetail) _mainAnimator.SetBool("AchiveCase_4", true); break; // Ячейка достижения 4
            case 11: if (!_isAchiveDetail) _mainAnimator.SetBool("AchiveCase_5", true); break; // Ячейка достижения 5
        }
    }

    /// <summary> Отпускание кнопок в меню </summary>
    public void MenuButtonUp (int buttonID)
    {
        switch (buttonID)
        {
            case 0:
                // Начать игру

                _mainAnimator.SetBool("Menu_1_Button", false);

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

                break;

            case 1:
                // Настройки

                _mainAnimator.SetBool("Menu_2_Button", false);

                switch (_mainAnimator.GetInteger("MenuStady"))
                {
                    case 0:
                        _textController.MenuOpenSettings();
                        _mainAnimator.SetTrigger("SwitchTextToSettings");
                        _mainAnimator.SetInteger("MenuStady", 1);
                        break;

                    case 1:
                        _mainAnimator.SetBool("Settings_2_St", !_mainAnimator.GetBool("Settings_2_St"));
                        DataController.gameSettingsData.isVibrationCheck = _mainAnimator.GetBool("Settings_2_St");
                        DataController.SaveSettingsData();
                        break;
                }

                break;

            case 2:
                // Об авторе

                _mainAnimator.SetBool("Menu_3_Button", false);

                switch (_mainAnimator.GetInteger("MenuStady"))
                {
                    case 0:
                        _textController.MenuOpenAbout();
                        _mainAnimator.SetTrigger("SwitchTextToAbout");
                        _mainAnimator.SetInteger("MenuStady", 2);
                        break;

                    case 1:
                        _mainAnimator.SetBool("Settings_3_St", !_mainAnimator.GetBool("Settings_3_St"));
                        DataController.gameSettingsData.isEffectCheck = _mainAnimator.GetBool("Settings_3_St");
                        DataController.SaveSettingsData();
                        break;
                }

                break;

            case 3:
                // Достижения
                
                switch (_mainAnimator.GetInteger("MenuStady"))
                {
                    case 0:
                        _mainAnimator.SetBool("Menu_4_Button", false);
                        _mainAnimator.SetInteger("MenuStady", 3);
                        break;

                    case 1:
                    case 2:
                        _textController.MenuOpenMain();
                        _mainAnimator.SetBool("Menu_4_Button", false);
                        _mainAnimator.SetInteger("MenuStady", 0);
                        _mainAnimator.SetTrigger("ReturnToMenu");
                        break;

                    default:
                        _mainAnimator.SetBool("Achives_Back", false);

                        if (!_isAchiveDetail) _mainAnimator.SetInteger("MenuStady", 0);
                        else
                        {
                            _mainAnimator.SetBool("AchiveDescript", false);
                            _isAchiveDetail = false;
                        }
                            
                        break;
                }

                break;

            case 4:
                // Влево в меню достижений

                if (!_isAchiveDetail)
                {
                    _mainAnimator.SetBool("Achives_Left", false);
                    _mainAnimator.SetTrigger("AchiveSlidePage");
                }
                break;

            case 5:
                // Вправо в меню достижений

                if (!_isAchiveDetail)
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

            case 7:
                // Ячейка достижения 1

                if (!_isAchiveDetail)
                {
                    _mainAnimator.SetBool("AchiveCase_1", false);

                    // Проверка на полученность

                    _mainAnimator.SetBool("AchiveDescript", true);
                    _isAchiveDetail = true;

                    // Смена текста описания
                }
                else
                {
                    _mainAnimator.SetBool("AchiveDescript", false);
                    _isAchiveDetail = false;
                }
                break;

            case 8:
                // Ячейка достижения 2

                if (!_isAchiveDetail)
                {
                    _mainAnimator.SetBool("AchiveCase_2", false);

                    // Проверка на полученность

                    _mainAnimator.SetBool("AchiveDescript", true);
                    _isAchiveDetail = true;

                    // Смена текста описания
                }
                else
                {
                    _mainAnimator.SetBool("AchiveDescript", false);
                    _isAchiveDetail = false;
                }
                break;

            case 9:
                // Ячейка достижения 3

                if (!_isAchiveDetail)
                {
                    _mainAnimator.SetBool("AchiveCase_3", false);

                    // Проверка на полученность

                    _mainAnimator.SetBool("AchiveDescript", true);
                    _isAchiveDetail = true;

                    // Смена текста описания
                }
                else
                {
                    _mainAnimator.SetBool("AchiveDescript", false);
                    _isAchiveDetail = false;
                }
                break;

            case 10:
                // Ячейка достижения 4

                if (!_isAchiveDetail)
                {
                    _mainAnimator.SetBool("AchiveCase_4", false);

                    // Проверка на полученность

                    _mainAnimator.SetBool("AchiveDescript", true);
                    _isAchiveDetail = true;

                    // Смена текста описания
                }
                else
                {
                    _mainAnimator.SetBool("AchiveDescript", false);
                    _isAchiveDetail = false;
                }
                break;

            case 11:
                // Ячейка достижения 5

                if (!_isAchiveDetail)
                {
                    _mainAnimator.SetBool("AchiveCase_5", false);

                    // Проверка на полученность

                    _mainAnimator.SetBool("AchiveDescript", true);
                    _isAchiveDetail = true;

                    // Смена текста описания
                }
                else
                {
                    _mainAnimator.SetBool("AchiveDescript", false);
                    _isAchiveDetail = false;
                }
                break;

            case 12:
                // Выход из стартового превью

                _mainAnimator.SetBool("StartGameDescript", false);
                break;
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