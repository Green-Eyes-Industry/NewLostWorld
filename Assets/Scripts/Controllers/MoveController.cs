using System.Collections;
using UnityEngine;

/// <summary>
/// Анимация и переходы
/// </summary>
public class MoveController : MonoBehaviour
{
    #region CONNECTIONS

    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private GameObject _gameCanvas;

    /// <summary>
    /// Первая глава сюжета
    /// </summary>
    public GamePart firstPart;

    private TextController _textController;
    private Animator _mainAnimator;
    private PlayerController _playerController;

    private bool _isAchiveDetail; // Вы в меню описания достижения

    #endregion

    #region VARIABLE

    /// <summary>
    /// Глава запускаемая по нажатию "Начало"
    /// </summary>
    private GamePart _startPart;

    #endregion

    /// <summary>
    /// Начало игры
    /// </summary>
    public void GameStart()
    {
        if (_startPart == null) _startPart = firstPart;
        NextPart(_startPart);
    }

    /// <summary>
    /// Привязка компонентов
    /// </summary>
    public void Init()
    {
        _mainAnimator = GetComponent<Animator>();
        _textController = GetComponent<TextController>();
        _playerController = GetComponent<PlayerController>();

        _isAchiveDetail = false;
    }

    #region PART_START

    /// <summary>
    /// Запуск следующей главы
    /// </summary>
    private void NextPart(GamePart nextPart)
    {
        if (nextPart is TextPart) ShowTextPart((TextPart)nextPart);
        else if (nextPart is ChangePart) ShowChangePart((ChangePart)nextPart);
        else if (nextPart is BattlePart) ShowBattlePart((BattlePart)nextPart);
        else ShowFinalPart((FinalPart)nextPart);

        _playerController.EventsStart(nextPart.mainEvents);
        
        _startPart = nextPart;
    }

    /// <summary>
    /// Запуск текстовой главы
    /// </summary>
    private void ShowTextPart(TextPart textPart)
    {
        _mainAnimator.SetInteger("PartType", 0);
        _textController.GameMain(textPart.mainText);
        _textController.GameButton_1(textPart.buttonText_1);
    }

    /// <summary>
    /// Запуск главы выбора
    /// </summary>
    private void ShowChangePart(ChangePart changePart)
    {
        // Код
        _mainAnimator.SetInteger("PartType", 1);
        _textController.GameMain(changePart.mainText);
        _textController.GameButton_1(changePart.buttonText_1);
        _textController.GameButton_2(changePart.buttonText_2);
    }

    /// <summary>
    /// Запуск главы боя
    /// </summary>
    private void ShowBattlePart(BattlePart battlePart)
    {
        // Код
        _mainAnimator.SetInteger("PartType", 2);
        _textController.GameMain(battlePart.mainText);
        _textController.GameButton_1(battlePart.buttonText_1);
        _textController.GameButton_2(battlePart.buttonText_2);
        _textController.GameButton_3(battlePart.buttonText_3);
    }

    /// <summary>
    /// Запуск финальной главы
    /// </summary>
    private void ShowFinalPart(FinalPart finalPart)
    {
        // Код
        Debug.Log("Это не текстовая глава а финальная");
    }

    #endregion

    #region BUTTON_CONTROL

    /// <summary>
    /// Нажатие на кнопку в игре
    /// </summary>
    public void GameButtonDown(int buttonID)
    {
        switch (buttonID)
        {
            case 0:
                _mainAnimator.SetBool("GameButton_1", true);
                break;

            case 1:
                _mainAnimator.SetBool("GameButton_2", true);
                break;

            case 2:
                _mainAnimator.SetBool("GameButton_3", true);
                break;

            case 3:
                // Инвентарь
                _mainAnimator.SetBool("Btb_Inventory", true);
                break;

            case 4:
                // Карта
                _mainAnimator.SetBool("Btb_Map", true);
                break;

            case 5:
                // Персонаж
                _mainAnimator.SetBool("Btb_Player", true);
                break;

            case 6:
                // Заметки
                _mainAnimator.SetBool("Btb_Notes", true);
                break;
        }
    }

    /// <summary>
    /// Отпускание кнопки в игре
    /// </summary>
    public void GameButtonUp(int buttonID)
    {
        switch (buttonID)
        {
            case 0:
                _mainAnimator.SetBool("GameButton_1", false);
                // if(_startPart.movePart_1 != null) NextPart(_startPart.movePart_1);
                break;

            case 1:
                _mainAnimator.SetBool("GameButton_2", false);
                // if (_startPart.movePart_2 != null) NextPart(_startPart.movePart_2);
                break;

            case 2:
                _mainAnimator.SetBool("GameButton_3", false);
                // if (_startPart.movePart_3 != null) NextPart(_startPart.movePart_3);
                break;

            case 3:
                // Инвентарь
                _mainAnimator.SetBool("Btb_Inventory", false);
                if (_mainAnimator.GetInteger("GameStady") != 5) _mainAnimator.SetInteger("GameStady", 5);
                else _mainAnimator.SetInteger("GameStady", 0); // Заменить на последнюю главу
                break;

            case 4:
                // Карта
                _mainAnimator.SetBool("Btb_Map", false);
                break;

            case 5:
                // Персонаж
                _mainAnimator.SetBool("Btb_Player", false);
                break;

            case 6:
                // Заметки
                _mainAnimator.SetBool("Btb_Notes", false);
                break;
        }
    }

    /// <summary>
    /// Нажатие на кнопку в меню
    /// </summary>
    public void MenuButtonDown(int buttonID)
    {
        switch (buttonID)
        {
            case 0:
                // Начать игру
                _mainAnimator.SetBool("Menu_1_Button", true);
                break;

            case 1:
                // Настройки
                _mainAnimator.SetBool("Menu_2_Button", true);
                break;

            case 2:
                // Об авторе
                _mainAnimator.SetBool("Menu_3_Button", true);
                break;

            case 3:
                // Достижения

                switch (_mainAnimator.GetInteger("MenuStady"))
                {
                    case 0:
                    case 1:
                    case 2:
                        _mainAnimator.SetBool("Menu_4_Button", true);
                        break;

                    default:
                        _mainAnimator.SetBool("Achives_Back", true);
                        break;
                }

                break;

            case 4:
                // Влево в меню достижений
                if (!_isAchiveDetail) _mainAnimator.SetBool("Achives_Left", true);
                break;

            case 5:
                // Вправо в меню достижений
                if (!_isAchiveDetail) _mainAnimator.SetBool("Achives_Right", true);
                break;

            case 7:
                // Ячейка достижения 1
                if (!_isAchiveDetail) _mainAnimator.SetBool("AchiveCase_1", true);
                break;

            case 8:
                // Ячейка достижения 2
                if (!_isAchiveDetail) _mainAnimator.SetBool("AchiveCase_2", true);
                break;

            case 9:
                // Ячейка достижения 3
                if (!_isAchiveDetail) _mainAnimator.SetBool("AchiveCase_3", true);
                break;

            case 10:
                // Ячейка достижения 4
                if (!_isAchiveDetail) _mainAnimator.SetBool("AchiveCase_4", true);
                break;

            case 11:
                // Ячейка достижения 5
                if (!_isAchiveDetail) _mainAnimator.SetBool("AchiveCase_5", true);
                break;
        }
    }

    /// <summary>
    /// Отпускание кнопки в меню
    /// </summary>
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

                        // TODO : Начать игру
                        _mainAnimator.SetBool("StartGameDescript", true);
                        _mainAnimator.SetInteger("MenuStady", 9);
                        _mainAnimator.SetInteger("GameStady", 0); // Переделать под сохраненную главу

                        StartCoroutine(HideStartDescriptMenu());

                        break;

                    case 1:
                        if (_mainAnimator.GetBool("Settings_1_St")) _mainAnimator.SetBool("Settings_1_St", false);
                        else _mainAnimator.SetBool("Settings_1_St", true);
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
                        if(_mainAnimator.GetBool("Settings_2_St")) _mainAnimator.SetBool("Settings_2_St", false);
                        else _mainAnimator.SetBool("Settings_2_St", true);
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
                        if (_mainAnimator.GetBool("Settings_3_St")) _mainAnimator.SetBool("Settings_3_St", false);
                        else _mainAnimator.SetBool("Settings_3_St", true);
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
                if (!_isAchiveDetail) _mainAnimator.SetBool("Achives_Left", false);
                break;

            case 5:
                // Вправо в меню достижений
                if (!_isAchiveDetail) _mainAnimator.SetBool("Achives_Right", false);
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

    #region ANIM_HELPERS

    /// <summary>
    /// Переход в игровое меню
    /// </summary>
    public void AnimMenuToGameSwitch()
    {
        _menuCanvas.SetActive(false);
        _gameCanvas.SetActive(true);
    }

    /// <summary>
    /// Задержка стартового меню
    /// </summary>
    private IEnumerator HideStartDescriptMenu()
    {
        yield return new WaitForSeconds(10f);

        _mainAnimator.SetBool("StartGameDescript", false);

    }

    #endregion
}