using System.Collections;
using UnityEngine;
using NLW.Parts;

namespace NLW
{
    /// <summary> Анимация и переходы </summary>
    public class AnimController : MainController
    {
        #region VARIABLES

        /// <summary> Глава запускаемая по нажатию "Начало" </summary>
        [HideInInspector] public GamePart thisPart;
        public GamePart startPart;

        public GameObject menuCanvas;
        public GameObject gameCanvas;

        private SubEventPart subPart;
        private Animator mainAnimator;

        private bool _isAchiveDetail; // Вы в меню описания достижения

        private int _lastPartType; // Тип последней главы
        private int _achiveSelectId; // Глава для вывода описания
        private int _achiveDisplayPage; // Отображаемая страница достижений
        private int _pageInvent; // Страница инвентаря

        #endregion

        protected override void Init()
        {
            mainAnimator = GetComponent<Animator>();

            mainAnimator.SetBool("Settings_1_St", dataController.mainSettings.isSoundCheck);
            mainAnimator.SetBool("Settings_2_St", dataController.mainSettings.isVibrationCheck);
            mainAnimator.SetBool("Settings_3_St", dataController.mainSettings.isEffectCheck);

            uIController.MenuOpenMain();

            _isAchiveDetail = false;
        }

        /// <summary> Начало игры </summary>
        private void GameStart()
        {
            if (thisPart == null && dataController.mainSettings.lastPart != null) thisPart = dataController.mainSettings.lastPart;
            else if (thisPart == null) thisPart = startPart;
            NextPart(thisPart);
        }

        #region PART_START

        /// <summary> Запуск следующей главы </summary>
        public void NextPart(GamePart nextPart)
        {
            mainAnimator.SetTrigger("SwitchGameText");

            if (nextPart is TextPart textPart)
            {
                ShowPart(textPart);
                gameController.EventsStart(nextPart.mainEvents);
            }
            else if (nextPart is ChangePart changePart)
            {
                ShowPart(changePart);
                gameController.EventsStart(nextPart.mainEvents);
            }
            else if (nextPart is BattlePart battlePart)
            {
                ShowPart(battlePart);
                gameController.EventsStart(nextPart.mainEvents);
            }
            else if (nextPart is FinalPart finalPart) ShowPart(finalPart);
            else if (nextPart is EventPart eventPart)
            {
                if (subPart == null)
                {
                    uIController.TimeEvent(true, eventPart.timeToEvent);
                    subPart = eventPart.eventParts[0];
                }
                ShowPart(subPart);
            }

            thisPart = nextPart;
        }

        /// <summary> Запуск текстовой главы </summary>
        private void ShowPart(TextPart part)
        {
            mainAnimator.SetInteger("GameStady", 0);
            uIController.GameMain(part.mainText);
            uIController.GameButton(0,part.buttonText);
        }

        /// <summary> Запуск главы выбора </summary>
        private void ShowPart(ChangePart part)
        {
            mainAnimator.SetInteger("GameStady", 1);
            uIController.GameMain(part.mainText);
            uIController.GameButton(0, part.buttonText[0]);
            uIController.GameButton(1, part.buttonText[1]);
        }

        /// <summary> Запуск главы боя </summary>
        private void ShowPart(BattlePart part)
        {
            mainAnimator.SetInteger("GameStady", 2);
            uIController.GameMain(part.mainText);
            uIController.GameButton(0, part.buttonText[0]);
            uIController.GameButton(1, part.buttonText[1]);
            uIController.GameButton(2, part.buttonText[2]);
        }

        /// <summary> Запуск финальной главы </summary>
        private void ShowPart(FinalPart part)
        {
            mainAnimator.SetInteger("GameStady", 10);
            uIController.GameMain(part.mainText);
            uIController.GameButton(0, part.backButtonText);
            uIController.GameButton(1,"Решения других игроков");
            uIController.ShowFinalAchiveIco(part.newAchive);

            if (!dataController.mainSettings.gameAchivemants.Contains(part.newAchive)) dataController.mainSettings.gameAchivemants.Add(part.newAchive);
            dataController.SaveAchivesData();
        }

        /// <summary> Запуск временного евента </summary>
        private void ShowPart(SubEventPart part)
        {
            mainAnimator.SetInteger("GameStady", 3);
            uIController.GameMain(part.mainText);
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
                                dataController.mainSettings.isSoundCheck = mainAnimator.GetBool("Settings_1_St");
                                dataController.SaveSettingsData();
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
                                uIController.MenuOpenSettings();
                                mainAnimator.SetTrigger("SwitchTextToSettings");
                                mainAnimator.SetInteger("MenuStady", 1);
                                break;

                            case 1:
                                mainAnimator.SetBool("Settings_2_St", !mainAnimator.GetBool("Settings_2_St"));
                                dataController.mainSettings.isVibrationCheck = mainAnimator.GetBool("Settings_2_St");
                                dataController.SaveSettingsData();
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
                                uIController.MenuOpenAbout();
                                mainAnimator.SetTrigger("SwitchTextToAbout");
                                mainAnimator.SetInteger("MenuStady", 2);
                                break;

                            case 1:
                                mainAnimator.SetBool("Settings_3_St", !mainAnimator.GetBool("Settings_3_St"));
                                dataController.mainSettings.isEffectCheck = mainAnimator.GetBool("Settings_3_St");
                                dataController.SaveSettingsData();
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
                                uIController.ShowAchive(_achiveDisplayPage);
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
                                uIController.MenuOpenMain();
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
            if ((_achiveDisplayPage * 5) + id <= dataController.mainSettings.gameAchivemants.Count)
            {
                mainAnimator.SetBool("AchiveCase_" + id, press);

                if (!press)
                {
                    if (!_isAchiveDetail)
                    {
                        mainAnimator.SetBool("AchiveDescript", true);
                        _isAchiveDetail = true;

                        uIController.ShowAchiveDescript(dataController.mainSettings.gameAchivemants[(_achiveDisplayPage * 5) + (id - 1)]);
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
                            uIController.ShowAchive(_achiveDisplayPage);
                            mainAnimator.SetBool("Achives_Left", true);
                        }
                        break;

                    case 2:
                        if (!_isAchiveDetail && dataController.mainSettings.gameAchivemants.Count > 5 * (_achiveDisplayPage + 1))
                        {
                            _achiveDisplayPage++;
                            uIController.ShowAchive(_achiveDisplayPage);
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
                        if (!_isAchiveDetail && 5 * (_achiveDisplayPage + 1) < dataController.mainSettings.gameAchivemants.Count)
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
            if (!press) mainAnimator.SetBool("StartGameDescript", false);
        }

        #endregion

        #region GAME_MENU

        /// <summary> Базовая игровая глава </summary>
        public void GamePartPress(int id, bool press)
        {
            mainAnimator.SetBool("GameButton_" + (id + 1), press);

            if (thisPart is FinalPart && !press)
            {
                switch (id)
                {
                    case 0:
                        mainAnimator.SetInteger("MenuStady", 0);
                        mainAnimator.SetInteger("GameStady", 20);
                        mainAnimator.SetBool("StartGameDescript", true);
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
                    mainAnimator.SetBool("GameButton_EvLeft", press);
                    if (!press)
                    {
                        // Влево
                        if (subPart.moveLeft.isFail && thisPart.movePart[2] != null)
                        {
                            uIController.TimeEvent(false, 0f);
                            FinalEvent(false);
                        }
                        else if (subPart.moveLeft.isFinal && thisPart.movePart[0] != null)
                        {
                            uIController.TimeEvent(false, 0f);
                            FinalEvent(true);
                        }
                        else if (subPart.moveLeft != null)
                        {
                            subPart = subPart.moveLeft;
                            NextPart(thisPart);
                        }
                    }
                    break;

                case 2:
                    mainAnimator.SetBool("GameButton_EvRight", press);
                    if (!press)
                    {
                        // Вправо
                        if (subPart.moveRight.isFail && thisPart.movePart[2] != null)
                        {
                            uIController.TimeEvent(false, 0f);
                            FinalEvent(false);
                        }
                        else if (subPart.moveRight.isFinal && thisPart.movePart[0] != null)
                        {
                            uIController.TimeEvent(false, 0f);
                            FinalEvent(true);
                        }
                        else if (subPart.moveRight != null)
                        {
                            subPart = subPart.moveRight;
                            NextPart(thisPart);
                        }
                    }
                    break;
            }
        }

        /// <summary> Завершение эвента </summary>
        public void FinalEvent(bool isWin)
        {
            if (isWin) NextPart(thisPart.movePart[0]);
            else NextPart(thisPart.movePart[2]);
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
                    mainAnimator.SetBool("Btb_Inventory", press);
                    if (!press)
                    {
                        if (mainAnimator.GetInteger("GameStady") != 5)
                        {
                            _pageInvent = 0;
                            uIController.ShowInventory(_pageInvent);
                            _lastPartType = mainAnimator.GetInteger("GameStady");
                            mainAnimator.SetInteger("GameStady", 5);
                        }
                        else mainAnimator.SetInteger("GameStady", _lastPartType);
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
                        if (_pageInvent > 0) _pageInvent--;
                        uIController.ShowInventory(_pageInvent);
                    }
                    break;

                case 13:
                    mainAnimator.SetBool("GameInvent_Bt_Right", press);
                    if (!press)
                    {
                        // TODO : Логика "Страница вправо"
                        _pageInvent++;
                        uIController.ShowInventory(_pageInvent);
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
            dataController.LoadGameData();

            yield return new WaitForSeconds(10f);

            mainAnimator.SetBool("StartGameDescript", false);

        }

        #endregion
    }
}