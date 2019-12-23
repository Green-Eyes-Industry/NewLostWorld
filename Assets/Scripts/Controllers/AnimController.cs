using System.Collections;
using UnityEngine;
using NLW.Parts;

namespace NLW
{
    /// <summary> Анимация и переходы </summary>
    public class AnimController : ParentController
    {
        #region VARIABLES

        /// <summary> Глава запускаемая по нажатию "Начало" </summary>
        [HideInInspector] public GamePart thisPart;
        public GamePart startPart;

        public GameObject menuCanvas;
        public GameObject gameCanvas;

        private SubEventPart _subPart;
        private Animator _mainAnimator;

        private bool _isAchiveDetail; // Вы в меню описания достижения

        private int _lastPartType; // Тип последней главы
        private int _achiveSelectId; // Глава для вывода описания
        private int _achiveDisplayPage; // Отображаемая страница достижений
        private int _pageInvent; // Страница инвентаря

        #endregion

        public override void Init()
        {
            _mainAnimator = GetComponent<Animator>();

            _mainAnimator.SetBool("Settings_1_St", MainController.instance.dataController.mainSettings.isSoundCheck);
            _mainAnimator.SetBool("Settings_2_St", MainController.instance.dataController.mainSettings.isVibrationCheck);
            _mainAnimator.SetBool("Settings_3_St", MainController.instance.dataController.mainSettings.isEffectCheck);

            MainController.instance.uIController.MenuOpenMain();

            _isAchiveDetail = false;
        }

        /// <summary> Начало игры </summary>
        private void GameStart()
        {
            if (thisPart == null && MainController.instance.dataController.mainSettings.lastPart != null)
                thisPart = MainController.instance.dataController.mainSettings.lastPart;
            else if (thisPart == null) thisPart = startPart;
            NextPart(thisPart);
        }

        #region PART_START

        /// <summary> Запуск следующей главы </summary>
        public void NextPart(GamePart nextPart)
        {
            _mainAnimator.SetTrigger("SwitchGameText");

            switch (nextPart)
            {
                case TextPart textPart:
                    ShowPart(textPart);
                    MainController.instance.gameController.EventsStart(nextPart.mainEvents);
                    break;
                case ChangePart changePart:
                    ShowPart(changePart);
                    MainController.instance.gameController.EventsStart(nextPart.mainEvents);
                    break;
                case BattlePart battlePart:
                    ShowPart(battlePart);
                    MainController.instance.gameController.EventsStart(nextPart.mainEvents);
                    break;
                case FinalPart finalPart:
                    ShowPart(finalPart);
                    break;
                case EventPart eventPart:
                    {
                        if (_subPart == null)
                        {
                            MainController.instance.uIController.TimeEvent(true, eventPart.timeToEvent);
                            _subPart = eventPart.eventParts[0];
                        }
                        ShowPart(_subPart);
                        break;
                    }
            }

            thisPart = nextPart;
        }

        /// <summary> Запуск текстовой главы </summary>
        private void ShowPart(TextPart part)
        {
            _mainAnimator.SetInteger("GameStady", 0);
            MainController.instance.uIController.GameMain(part.mainText);
            MainController.instance.uIController.GameButton(0,part.buttonText);
        }

        /// <summary> Запуск главы выбора </summary>
        private void ShowPart(ChangePart part)
        {
            _mainAnimator.SetInteger("GameStady", 1);
            MainController.instance.uIController.GameMain(part.mainText);
            MainController.instance.uIController.GameButton(0, part.buttonText[0]);
            MainController.instance.uIController.GameButton(1, part.buttonText[1]);
        }

        /// <summary> Запуск главы боя </summary>
        private void ShowPart(BattlePart part)
        {
            _mainAnimator.SetInteger("GameStady", 2);
            MainController.instance.uIController.GameMain(part.mainText);
            MainController.instance.uIController.GameButton(0, part.buttonText[0]);
            MainController.instance.uIController.GameButton(1, part.buttonText[1]);
            MainController.instance.uIController.GameButton(2, part.buttonText[2]);
        }

        /// <summary> Запуск финальной главы </summary>
        private void ShowPart(FinalPart part)
        {
            _mainAnimator.SetInteger("GameStady", 10);
            MainController.instance.uIController.GameMain(part.mainText);
            MainController.instance.uIController.GameButton(0, part.backButtonText);
            MainController.instance.uIController.GameButton(1,"Решения других игроков");
            MainController.instance.uIController.ShowFinalAchiveIco(part.newAchive);

            if (!MainController.instance.dataController.mainSettings.gameAchivemants.Contains(part.newAchive))
                MainController.instance.dataController.mainSettings.gameAchivemants.Add(part.newAchive);
            MainController.instance.dataController.SaveAchivesData();
        }

        /// <summary> Запуск временного евента </summary>
        private void ShowPart(SubEventPart part)
        {
            _mainAnimator.SetInteger("GameStady", 3);
            MainController.instance.uIController.GameMain(part.mainText);
        }

        #endregion

        #region MAIN_MENU

        /// <summary> Главное меню </summary>
        public void MainMenuPress(int id, bool press)
        {
            switch (id)
            {
                case 1:
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

                                switch (thisPart)
                                {
                                    case TextPart _: partType = 0; break;
                                    case ChangePart _: partType = 1; break;
                                    case BattlePart _: partType = 2; break;
                                    case EventPart _: partType = 3; break;
                                    default: partType = 4; break;
                                }

                                _mainAnimator.SetInteger("GameStady", partType);
                                StartCoroutine(HideStartDescriptMenu());

                                break;

                            case 1:
                                _mainAnimator.SetBool("Settings_1_St", !_mainAnimator.GetBool("Settings_1_St"));
                                MainController.instance.dataController.mainSettings.isSoundCheck = _mainAnimator.GetBool("Settings_1_St");
                                MainController.instance.dataController.SaveSettingsData();
                                break;
                        }
                    }
                    break;

                case 2:
                    _mainAnimator.SetBool("Menu_2_Button", press);

                    if (!press)
                    {
                        switch (_mainAnimator.GetInteger("MenuStady"))
                        {
                            case 0:
                                MainController.instance.uIController.MenuOpenSettings();
                                _mainAnimator.SetTrigger("SwitchTextToSettings");
                                _mainAnimator.SetInteger("MenuStady", 1);
                                break;

                            case 1:
                                _mainAnimator.SetBool("Settings_2_St", !_mainAnimator.GetBool("Settings_2_St"));
                                MainController.instance.dataController.mainSettings.isVibrationCheck = _mainAnimator.GetBool("Settings_2_St");
                                MainController.instance.dataController.SaveSettingsData();
                                break;
                        }
                    }
                    break;

                case 3:
                    _mainAnimator.SetBool("Menu_3_Button", press);

                    if (!press)
                    {
                        switch (_mainAnimator.GetInteger("MenuStady"))
                        {
                            case 0:
                                MainController.instance.uIController.MenuOpenAbout();
                                _mainAnimator.SetTrigger("SwitchTextToAbout");
                                _mainAnimator.SetInteger("MenuStady", 2);
                                break;

                            case 1:
                                _mainAnimator.SetBool("Settings_3_St", !_mainAnimator.GetBool("Settings_3_St"));
                                MainController.instance.dataController.mainSettings.isEffectCheck = _mainAnimator.GetBool("Settings_3_St");
                                MainController.instance.dataController.SaveSettingsData();
                                break;
                        }
                    }
                    break;

                case 4:
                    if (press)
                    {
                        _achiveDisplayPage = 0;

                        switch (_mainAnimator.GetInteger("MenuStady"))
                        {
                            case 0:
                            case 1:
                            case 2:
                                _mainAnimator.SetBool("Menu_4_Button", true);
                                MainController.instance.uIController.ShowAchive(_achiveDisplayPage);
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
                                MainController.instance.uIController.MenuOpenMain();
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
                    break;
            }
        }

        /// <summary> Ячейки Достижений </summary>
        public void AchiveCaseMenuPress(int id, bool press)
        {
            if ((_achiveDisplayPage * 5) + id <= MainController.instance.dataController.mainSettings.gameAchivemants.Count)
            {
                _mainAnimator.SetBool("AchiveCase_" + id, press);

                if (!press)
                {
                    if (!_isAchiveDetail)
                    {
                        _mainAnimator.SetBool("AchiveDescript", true);
                        _isAchiveDetail = true;

                        MainController.instance.uIController.ShowAchiveDescript(
                            MainController.instance.dataController.mainSettings.gameAchivemants[(_achiveDisplayPage * 5) + (id - 1)]);
                    }
                    else
                    {
                        _mainAnimator.SetBool("AchiveDescript", false);
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
                            MainController.instance.uIController.ShowAchive(_achiveDisplayPage);
                            _mainAnimator.SetBool("Achives_Left", true);
                        }
                        break;

                    case 2:
                        if (!_isAchiveDetail && MainController.instance.dataController.mainSettings.gameAchivemants.Count > 5 * (_achiveDisplayPage + 1))
                        {
                            _achiveDisplayPage++;
                            MainController.instance.uIController.ShowAchive(_achiveDisplayPage);
                            _mainAnimator.SetBool("Achives_Right", true);
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
                            _mainAnimator.SetBool("Achives_Left", false);
                            _mainAnimator.SetTrigger("AchiveSlidePage");
                        }
                        break;

                    case 2:
                        if (!_isAchiveDetail && 5 * (_achiveDisplayPage + 1) < MainController.instance.dataController.mainSettings.gameAchivemants.Count)
                        {
                            _mainAnimator.SetBool("Achives_Right", false);
                            _mainAnimator.SetTrigger("AchiveSlidePage");
                        }
                        break;

                    case 3:
                        _mainAnimator.SetBool("AchiveDescript", false);
                        _isAchiveDetail = false;
                        break;
                }
            }
        }

        /// <summary> Выход из первоначальной вставки </summary>
        public void PreviewExit(bool press)
        {
            if (!press) _mainAnimator.SetBool("StartGameDescript", false);
        }

        #endregion

        #region GAME_MENU

        /// <summary> Базовая игровая глава </summary>
        public void GamePartPress(int id, bool press)
        {
            _mainAnimator.SetBool("GameButton_" + (id + 1), press);

            if (thisPart is FinalPart && !press)
            {
                switch (id)
                {
                    case 0:
                        _mainAnimator.SetInteger("MenuStady", 0);
                        _mainAnimator.SetInteger("GameStady", 20);
                        _mainAnimator.SetBool("StartGameDescript", true);
                        break;

                    case 1:
                        // TODO : Статистика решений других игроков
                        break;
                }
                thisPart = startPart;
                return;
            }

            
            if (thisPart.movePart[id] != null && !press) NextPart(thisPart.movePart[id]);
        }

        /// <summary> Глава эвента </summary>
        public void EventPartPress(int id, bool press)
        {
            switch (id)
            {
                case 1:
                    _mainAnimator.SetBool("GameButton_EvLeft", press);
                    if (!press)
                    {
                        // Влево
                        if (_subPart.moveLeft.isFail && thisPart.movePart[2] != null)
                        {
                            MainController.instance.uIController.TimeEvent(false, 0f);
                            FinalEvent(false);
                        }
                        else if (_subPart.moveLeft.isFinal && thisPart.movePart[0] != null)
                        {
                            MainController.instance.uIController.TimeEvent(false, 0f);
                            FinalEvent(true);
                        }
                        else if (_subPart.moveLeft != null)
                        {
                            _subPart = _subPart.moveLeft;
                            NextPart(thisPart);
                        }
                    }
                    break;

                case 2:
                    _mainAnimator.SetBool("GameButton_EvRight", press);
                    if (!press)
                    {
                        // Вправо
                        if (_subPart.moveRight.isFail && thisPart.movePart[2] != null)
                        {
                            MainController.instance.uIController.TimeEvent(false, 0f);
                            FinalEvent(false);
                        }
                        else if (_subPart.moveRight.isFinal && thisPart.movePart[0] != null)
                        {
                            MainController.instance.uIController.TimeEvent(false, 0f);
                            FinalEvent(true);
                        }
                        else if (_subPart.moveRight != null)
                        {
                            _subPart = _subPart.moveRight;
                            NextPart(thisPart);
                        }
                    }
                    break;
            }
        }

        /// <summary> Завершение эвента </summary>
        public void FinalEvent(bool isWin)
        {
            NextPart(isWin ? thisPart.movePart[0] : thisPart.movePart[2]);
        }

        /// <summary> Глава загадки </summary>
        public void MazePartPress(int id, bool press)
        {
            // TODO : Разработка
        }

        #endregion

        #region GAME_HELP_MENU

        /// <summary> Меню инвентаря </summary>
        public void InventoryMenuPress(int id, bool press)
        {
            switch (id)
            {
                case 0:
                    _mainAnimator.SetBool("Btb_Inventory", press);
                    if (!press)
                    {
                        if (_mainAnimator.GetInteger("GameStady") != 5)
                        {
                            _pageInvent = 0;
                            MainController.instance.uIController.ShowInventory(_pageInvent);
                            _lastPartType = _mainAnimator.GetInteger("GameStady");
                            _mainAnimator.SetInteger("GameStady", 5);
                        }
                        else _mainAnimator.SetInteger("GameStady", _lastPartType);
                    }
                    break;

                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    _mainAnimator.SetBool("GameInvent_Case_" + id, press);
                    if (!press)
                    {
                        // TODO : Логика "Выбор предмета в ячейке"
                    }
                    break;

                case 9:
                    _mainAnimator.SetBool("GameInvent_Bt_Info", press);
                    if (!press)
                    {
                        // TODO : Логика "Подробности о предмете"
                    }
                    break;

                case 10:
                    _mainAnimator.SetBool("GameInvent_Bt_Use", press);
                    if (!press)
                    {
                        // TODO : Логика "Использовать предмет"
                    }
                    break;

                case 11:
                    _mainAnimator.SetBool("GameInvent_Bt_Remove", press);
                    if (!press)
                    {
                        // TODO : Логика "Выбросить предмет"
                    }
                    break;

                case 12:
                    _mainAnimator.SetBool("GameInvent_Bt_Left", press);
                    if (!press)
                    {
                        // TODO : Логика "Страница влево"
                        if (_pageInvent > 0) _pageInvent--;
                        MainController.instance.uIController.ShowInventory(_pageInvent);
                    }
                    break;

                case 13:
                    _mainAnimator.SetBool("GameInvent_Bt_Right", press);
                    if (!press)
                    {
                        // TODO : Логика "Страница вправо"
                        _pageInvent++;
                        MainController.instance.uIController.ShowInventory(_pageInvent);
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
                    _mainAnimator.SetBool("Btb_Player", press);
                    if (!press)
                    {
                        if (_mainAnimator.GetInteger("GameStady") != 6)
                        {
                            _lastPartType = _mainAnimator.GetInteger("GameStady");
                            _mainAnimator.SetInteger("GameStady", 6);
                        }
                        else _mainAnimator.SetInteger("GameStady", _lastPartType);
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
                    _mainAnimator.SetBool("Btb_Map", press);
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
                    _mainAnimator.SetBool("Btb_Notes", press);
                    if (!press)
                    {
                        // TODO : Логика перехода в меню заметок
                    }
                    break;
            }
        }

        #endregion

        #region HELPERS

        /// <summary> Переход в игровое меню </summary>
        public void AnimMenuToGameSwitch()
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            gameCanvas.SetActive(!gameCanvas.activeSelf);
        }

        /// <summary> Задержка стартового меню </summary>
        private IEnumerator HideStartDescriptMenu()
        {
            MainController.instance.dataController.LoadGameData();

            yield return new WaitForSeconds(10f);

            _mainAnimator.SetBool("StartGameDescript", false);

        }

        #endregion
    }
}