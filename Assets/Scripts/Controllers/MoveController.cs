using UnityEngine;

/// <summary>
/// Анимация и переходы
/// </summary>
public class MoveController : MonoBehaviour
{
    #region CONNECTIONS

    /// <summary>
    /// Первая глава сюжета
    /// </summary>
    public GamePart firstPart;

    private TextController _textController;
    private Animator _mainAnimator;
    private PlayerController _playerController;

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

        GameStart();
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

                break;

            case 4:
                // Карта

                break;

            case 5:
                // Персонаж

                break;

            case 6:
                // Заметки

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
                if(_startPart.movePart_1 != null) NextPart(_startPart.movePart_1);
                break;

            case 1:
                _mainAnimator.SetBool("GameButton_2", false);
                if (_startPart.movePart_2 != null) NextPart(_startPart.movePart_2);
                break;

            case 2:
                _mainAnimator.SetBool("GameButton_3", false);
                if (_startPart.movePart_3 != null) NextPart(_startPart.movePart_3);
                break;

            case 3:
                // Инвентарь

                break;

            case 4:
                // Карта

                break;

            case 5:
                // Персонаж

                break;

            case 6:
                // Заметки

                break;
        }
    }

    /// <summary>
    /// Нажатие на кнопку в меню
    /// </summary>
    public void MenuButtonDown(int buttonID)
    {

    }

    /// <summary>
    /// Отпускание кнопки в меню
    /// </summary>
    public void MenuButtonUp (int buttonID)
    {

    }

    #endregion
}