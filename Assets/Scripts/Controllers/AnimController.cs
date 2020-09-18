using System.Collections;
using Data;
using Data.GameParts;
using Helpers;
using UnityEngine;

namespace Controllers
{
    /// <summary> Анимация и переходы </summary>
    [RequireComponent(typeof(Animator))]
    public class AnimController : ParentController
    {
        #region VARIABLES

        /// <summary> Глава запускаемая по нажатию "Начало" </summary>
        [HideInInspector] public GamePart thisPart;
        public GamePart startPart;

        public GameObject menuCanvas;
        public GameObject gameCanvas;

        private AnimatorParameters _aParam;
        private SubEventPart _subPart;
        private Animator _mainAnimator;

        private bool _isAchiveDetail;   // Вы в меню описания достижения

        private int _lastPartType;      // Тип последней главы
        private int _achiveSelectId;    // Глава для вывода описания
        private int _achiveDisplayPage; // Отображаемая страница достижений
        private int _pageInvent;        // Страница инвентаря
        private GameItem _selectedItem; // Выбранный предмет в инвентаре

        public enum MessangeType
        {
            ITEM_MS,
            EFFECT_MS,
            NOTE_MS,
            MAP_MS
        }

        private enum ItemInteractType
        {
            INFO_ITEM,
            USE_ITEM,
            REMOVE_ITEM
        }
        private ItemInteractType _selectedMenuInvent; // Текущее окно взаимодействия с предметом

        #endregion

        public override void Init()
        {
            _mainAnimator = GetComponent<Animator>();
            _aParam = new AnimatorParameters();
            _aParam.InitParam();

            _mainAnimator.SetBool(_aParam.stadySettings_1, MainController.instance.dataController.mainSettings.isSoundCheck);
            _mainAnimator.SetBool(_aParam.stadySettings_2, MainController.instance.dataController.mainSettings.isVibrationCheck);
            _mainAnimator.SetBool(_aParam.stadySettings_3, MainController.instance.dataController.mainSettings.isEffectCheck);

            MainController.instance.uIController.MenuOpenMain();

            _isAchiveDetail = false;
        }

        /// <summary> Начало игры </summary>
        private void GameStart()
        {
            if (thisPart == null && MainController.instance.dataController.mainSettings.lastPart != null) thisPart = MainController.instance.dataController.mainSettings.lastPart;
            else if (thisPart == null) thisPart = startPart;
            NextPart(thisPart);
        }

        #region PART_START

        /// <summary> Запуск следующей главы </summary>
        public void NextPart(GamePart nextPart)
        {
            _mainAnimator.SetTrigger(_aParam.switchGameText);

            thisPart = nextPart;

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
        }

        /// <summary> Запуск текстовой главы </summary>
        private void ShowPart(TextPart part)
        {
            _mainAnimator.SetInteger(_aParam.gStady, 0);
            MainController.instance.uIController.GameMain(part.mainText);
            MainController.instance.uIController.GameButton(0, part.buttonText);
        }

        /// <summary> Запуск главы выбора </summary>
        private void ShowPart(ChangePart part)
        {
            _mainAnimator.SetInteger(_aParam.gStady, 1);
            MainController.instance.uIController.GameMain(part.mainText);
            MainController.instance.uIController.GameButton(0, part.buttonText[0]);
            MainController.instance.uIController.GameButton(1, part.buttonText[1]);
        }

        /// <summary> Запуск главы боя </summary>
        private void ShowPart(BattlePart part)
        {
            _mainAnimator.SetInteger(_aParam.gStady, 2);
            MainController.instance.uIController.GameMain(part.mainText);
            MainController.instance.uIController.GameButton(0, part.buttonText[0]);
            MainController.instance.uIController.GameButton(1, part.buttonText[1]);
            MainController.instance.uIController.GameButton(2, part.buttonText[2]);
        }

        /// <summary> Запуск финальной главы </summary>
        private void ShowPart(FinalPart part)
        {
            _mainAnimator.SetInteger(_aParam.gStady, 10);
            MainController.instance.uIController.GameMain(part.mainText);
            MainController.instance.uIController.GameButton(0, part.backButtonText);
            MainController.instance.uIController.GameButton(1, "Решения других игроков");
            MainController.instance.uIController.ShowFinalAchiveIco(part.newAchive);

            if (!MainController.instance.dataController.mainSettings.gameAchivemants.Contains(part.newAchive)) MainController.instance.dataController.mainSettings.gameAchivemants.Add(part.newAchive);
            MainController.instance.dataController.SaveGlobalSettings();
        }

        /// <summary> Запуск временного евента </summary>
        private void ShowPart(SubEventPart part)
        {
            _mainAnimator.SetInteger(_aParam.gStady, 3);
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
                    _mainAnimator.SetBool(_aParam.buttonMenu_1, press);

                    if (!press)
                    {
                        switch (_mainAnimator.GetInteger(_aParam.mStady))
                        {
                            case 0:
                                _mainAnimator.SetBool(_aParam.startGameDeskript, true);
                                _mainAnimator.SetInteger(_aParam.mStady, 9);
                                StartCoroutine(HideStartDescriptMenu());

                                break;

                            case 1:
                                _mainAnimator.SetBool(_aParam.stadySettings_1, !_mainAnimator.GetBool(_aParam.stadySettings_1));
                                MainController.instance.dataController.mainSettings.isSoundCheck = _mainAnimator.GetBool(_aParam.stadySettings_1);
                                break;
                        }
                    }
                    break;

                case 2:
                    _mainAnimator.SetBool(_aParam.buttonMenu_2, press);

                    if (!press)
                    {
                        switch (_mainAnimator.GetInteger(_aParam.mStady))
                        {
                            case 0:
                                MainController.instance.uIController.MenuOpenSettings();
                                _mainAnimator.SetTrigger(_aParam.returnToMenu);
                                _mainAnimator.SetInteger(_aParam.mStady, 1);
                                break;

                            case 1:
                                _mainAnimator.SetBool(_aParam.stadySettings_2, !_mainAnimator.GetBool(_aParam.stadySettings_2));
                                MainController.instance.dataController.mainSettings.isVibrationCheck = _mainAnimator.GetBool(_aParam.stadySettings_2);
                                break;
                        }
                    }
                    break;

                case 3:
                    _mainAnimator.SetBool(_aParam.buttonMenu_3, press);

                    if (!press)
                    {
                        switch (_mainAnimator.GetInteger(_aParam.mStady))
                        {
                            case 0:
                                MainController.instance.uIController.MenuOpenAbout();
                                _mainAnimator.SetTrigger(_aParam.switchTextToAbout);
                                _mainAnimator.SetInteger(_aParam.mStady, 2);
                                break;

                            case 1:
                                _mainAnimator.SetBool(_aParam.stadySettings_3, !_mainAnimator.GetBool(_aParam.stadySettings_3));
                                MainController.instance.dataController.mainSettings.isEffectCheck = _mainAnimator.GetBool(_aParam.stadySettings_3);
                                break;
                        }
                    }
                    break;

                case 4:
                    if (press)
                    {
                        _achiveDisplayPage = 0;

                        switch (_mainAnimator.GetInteger(_aParam.mStady))
                        {
                            case 0:
                            case 1:
                            case 2:
                                _mainAnimator.SetBool(_aParam.buttonMenu_4, true);
                                MainController.instance.uIController.ShowAchive(_achiveDisplayPage);
                                break;

                            default: _mainAnimator.SetBool(_aParam.buttonAchiveBack, true); break;
                        }
                    }
                    else
                    {
                        switch (_mainAnimator.GetInteger(_aParam.mStady))
                        {
                            case 0:
                                _mainAnimator.SetBool(_aParam.buttonMenu_4, press);
                                _mainAnimator.SetInteger(_aParam.mStady, 3);
                                break;

                            case 1:
                            case 2:
                                MainController.instance.uIController.MenuOpenMain();
                                _mainAnimator.SetBool(_aParam.buttonMenu_4, press);
                                _mainAnimator.SetInteger(_aParam.mStady, 0);
                                _mainAnimator.SetTrigger(_aParam.returnToMenu);
                                MainController.instance.dataController.SaveGlobalSettings();
                                break;

                            default:
                                _mainAnimator.SetBool(_aParam.buttonAchiveBack, press);

                                if (!_isAchiveDetail) _mainAnimator.SetInteger(_aParam.mStady, 0);
                                else
                                {
                                    _mainAnimator.SetBool(_aParam.achiveDescript, false);
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
                if (!_isAchiveDetail) _mainAnimator.SetBool(_aParam.buttonAchiveCases + id, press);

                if (!press)
                {
                    if (!_isAchiveDetail)
                    {
                        _mainAnimator.SetBool(_aParam.achiveDescript, true);
                        _isAchiveDetail = true;

                        MainController.instance.uIController.ShowAchiveDescript(
                            MainController.instance.dataController.mainSettings.gameAchivemants[(_achiveDisplayPage * 5) + (id - 1)]);
                    }
                    else
                    {
                        _mainAnimator.SetBool(_aParam.achiveDescript, false);
                        _isAchiveDetail = false;
                    }
                }
            }
            else if (!_isAchiveDetail) _mainAnimator.SetBool(_aParam.buttonAchiveCloseCases + id, press);
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
                            _mainAnimator.SetBool(_aParam.buttonAchiveLeft, true);
                        }
                        break;

                    case 2:
                        if (!_isAchiveDetail && MainController.instance.dataController.mainSettings.gameAchivemants.Count > 5 * (_achiveDisplayPage + 1))
                        {
                            _achiveDisplayPage++;
                            MainController.instance.uIController.ShowAchive(_achiveDisplayPage);
                            _mainAnimator.SetBool(_aParam.buttonAchiveRight, true);
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
                            _mainAnimator.SetBool(_aParam.buttonAchiveLeft, false);
                            _mainAnimator.SetTrigger(_aParam.achiveSlidePage);
                        }
                        break;

                    case 2:
                        if (!_isAchiveDetail && 5 * (_achiveDisplayPage + 1) < MainController.instance.dataController.mainSettings.gameAchivemants.Count)
                        {
                            _mainAnimator.SetBool(_aParam.buttonAchiveRight, false);
                            _mainAnimator.SetTrigger(_aParam.achiveSlidePage);
                        }
                        break;

                    case 3:
                        _mainAnimator.SetBool(_aParam.achiveDescript, false);
                        _isAchiveDetail = false;
                        break;
                }
            }
        }

        /// <summary> Выход из первоначальной вставки </summary>
        public void PreviewExit(bool press)
        {
            if (!press) _mainAnimator.SetBool(_aParam.startGameDeskript, false);
        }

        #endregion

        #region GAME_MENU

        /// <summary> Базовая игровая глава </summary>
        public void GamePartPress(int id, bool press)
        {
            _mainAnimator.SetBool(_aParam.buttonGame + (id + 1), press);

            if (thisPart is FinalPart && !press)
            {
                switch (id)
                {
                    case 0:
                        _mainAnimator.SetInteger(_aParam.mStady, 0);
                        _mainAnimator.SetInteger(_aParam.gStady, 20);
                        _mainAnimator.SetBool(_aParam.startGameDeskript, true);
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
                    _mainAnimator.SetBool(_aParam.buttonEventLeft, press);
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
                    _mainAnimator.SetBool(_aParam.buttonEventRight, press);
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

                    // Открытие меню инвентаря
                    _mainAnimator.SetBool(_aParam.buttonInventory, press);
                    if (!press)
                    {
                        if (_mainAnimator.GetInteger(_aParam.gStady) != 5)
                        {
                            _pageInvent = 0;
                            if (MainController.instance.dataController.mainPlayer.playerInventory.Count > 0)
                            {
                                _selectedItem = MainController.instance.dataController.mainPlayer.playerInventory[0];
                                MainController.instance.uIController.GameMessageInventory(_selectedItem.itemName);
                            }
                            else MainController.instance.uIController.GameMessageInventory("Пусто");
                            MainController.instance.uIController.ShowInventory(_pageInvent);
                            _lastPartType = _mainAnimator.GetInteger(_aParam.gStady);

                            _mainAnimator.SetInteger(_aParam.gameInventPage, 1);
                            _mainAnimator.SetInteger(_aParam.gStady, 5);
                        }
                        else
                        {
                            _mainAnimator.SetInteger(_aParam.gStady, _lastPartType);
                            _mainAnimator.SetBool(_aParam.gameInventDescript, false);
                        }
                    }
                    break;

                case 1: case 2: case 3: case 4: case 5: case 6: case 7: case 8:

                    // Выбор предмета

                    if (_mainAnimator.GetBool(_aParam.gameInventDescript)) return; // отключение ложной смены предмета

                    _mainAnimator.SetBool(_aParam.buttonGameInventCases + id, press);
                    if (!press)
                    {
                        if (MainController.instance.dataController.mainPlayer.playerInventory.Count > id + (_pageInvent * 8) - 1)
                        {
                            _selectedItem = MainController.instance.dataController.mainPlayer.playerInventory[id + (_pageInvent * 8) - 1];
                            MainController.instance.uIController.GameMessageInventory(_selectedItem.itemName);
                        }
                        else
                        {
                            MainController.instance.uIController.GameMessageInventory("Пусто");
                            _selectedItem = null;
                        }
                        _mainAnimator.SetInteger(_aParam.inventCaseSelected, id);
                    }
                    break;

                case 9:

                    // Подробности
                    if (_selectedItem != null)
                    {
                        _mainAnimator.SetBool(_aParam.buttonGameInventInfo, press);
                        if (!press) ItemInteract(ItemInteractType.INFO_ITEM);
                    }
                    else _mainAnimator.SetBool(_aParam.buttonGameInventNullInfo, press);
                    break;

                case 10:

                    // Исспользовать
                    if (_selectedItem != null)
                    {
                        _mainAnimator.SetBool(_aParam.buttonGameInventUse, press);
                        if (!press) ItemInteract(ItemInteractType.USE_ITEM);
                    }
                    else _mainAnimator.SetBool(_aParam.buttonGameInventNullUse, press);
                    break;

                case 11:

                    // Выбросить
                    if (_selectedItem != null)
                    {
                        _mainAnimator.SetBool(_aParam.buttonGameInventRemove, press);
                        if (!press) ItemInteract(ItemInteractType.REMOVE_ITEM);
                    }
                    else _mainAnimator.SetBool(_aParam.buttonGameInventNullRemove, press);
                    break;

                case 12:

                    // Страница влево
                    if(_pageInvent > 0)
                    {
                        _mainAnimator.SetBool(_aParam.buttonGameInventLeft, press);

                        if (!press)
                        {
                            _pageInvent--;
                            _mainAnimator.SetInteger(_aParam.gameInventPage, _pageInvent + 1);
                            MainController.instance.uIController.ShowInventory(_pageInvent);
                        }
                    }
                    break;

                case 13:

                    // Страница вправо
                    if (_pageInvent < 2)
                    {
                        _mainAnimator.SetBool(_aParam.buttonGameInventRight, press);

                        if (!press)
                        {
                            _pageInvent++;
                            _mainAnimator.SetInteger(_aParam.gameInventPage, _pageInvent + 1);
                            MainController.instance.uIController.ShowInventory(_pageInvent);
                        }
                    }
                    break;

                case 14:

                    // Да в вопросе о взаимодействии
                    _mainAnimator.SetBool(_aParam.buttonInventoryIYes, press);
                    if (!press)
                    {
                        _mainAnimator.SetBool(_aParam.gameInventDescript, false);

                        switch (_selectedMenuInvent)
                        {
                            case ItemInteractType.USE_ITEM:

                                Data.GameItems.UsableItem uItem = (Data.GameItems.UsableItem)_selectedItem;
                                uItem.UseThisItem();
                                break;
                        }

                        MainController.instance.dataController.mainPlayer.playerInventory.Remove(_selectedItem);
                        MainController.instance.uIController.GameMessageInventory("Пусто");
                        _selectedItem = null;
                        MainController.instance.uIController.ShowInventory(_pageInvent);
                    }
                    break;
                case 15:

                    // Нет в вопросе о взаимодействии
                    _mainAnimator.SetBool(_aParam.buttonInventoryINo, press);
                    if (!press) _mainAnimator.SetBool(_aParam.gameInventDescript, false);
                    break;
            }
        }

        /// <summary> Меню персонажа </summary>
        public void PlayerMenuPress(int id, bool press)
        {
            switch (id)
            {
                case 0:
                    _mainAnimator.SetBool(_aParam.buttonPlayer, press);
                    if (!press)
                    {
                        if (_mainAnimator.GetInteger(_aParam.gStady) != 6)
                        {
                            MainController.instance.uIController.ShowEffects();
                            _lastPartType = _mainAnimator.GetInteger(_aParam.gStady);
                            _mainAnimator.SetInteger(_aParam.gStady, 6);
                        }
                        else
                        {
                            _mainAnimator.SetInteger(_aParam.gStady, _lastPartType);
                            _mainAnimator.SetBool(_aParam.gamePlayerDescript, false);
                        }
                    }
                    break;

                case 7:
                    _mainAnimator.SetBool(_aParam.buttonPlayerBack, press);
                    if (!press) _mainAnimator.SetBool(_aParam.gamePlayerDescript, false);
                    break;

                default:
                    _mainAnimator.SetBool(_aParam.buttonPlayerCases + id, press);
                    if (!press && (id - 1) < MainController.instance.dataController.mainPlayer.playerEffects.Count)
                    {
                        MainController.instance.uIController.ShowEffectDescript(id - 1);
                        _mainAnimator.SetBool(_aParam.gamePlayerDescript, true);
                    }
                    break;

            }
        }

        /// <summary> Меню карты </summary>
        public void MapMenuPress(int id, bool press)
        {
            switch (id)
            {
                case 0:
                    _mainAnimator.SetBool(_aParam.buttonMap, press);
                    if (!press)
                    {
                        if (_mainAnimator.GetInteger(_aParam.gStady) != 7)
                        {
                            _lastPartType = _mainAnimator.GetInteger(_aParam.gStady);
                            _mainAnimator.SetInteger(_aParam.gStady, 7);
                        }
                        else _mainAnimator.SetInteger(_aParam.gStady, _lastPartType);
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
                    _mainAnimator.SetBool(_aParam.buttonNote, press);
                    if (!press)
                    {
                        if (_mainAnimator.GetInteger(_aParam.gStady) != 8)
                        {
                            _lastPartType = _mainAnimator.GetInteger(_aParam.gStady);
                            _mainAnimator.SetInteger(_aParam.gStady, 8);
                        }
                        else _mainAnimator.SetInteger(_aParam.gStady, _lastPartType);
                    }
                    break;
            }
        }

        #endregion

        #region EFFECTS_ANIMATION

        /// <summary> Анимация сообщения </summary>
        public void GameMessangeEffect(MessangeType mt)
        {
            switch (mt)
            {
                case MessangeType.ITEM_MS: _mainAnimator.SetTrigger(_aParam.messangeInventory); break;
                case MessangeType.EFFECT_MS: _mainAnimator.SetTrigger(_aParam.messangePlayer); break;
                case MessangeType.NOTE_MS: _mainAnimator.SetTrigger(_aParam.messangeNote); break;
                case MessangeType.MAP_MS: _mainAnimator.SetTrigger(_aParam.messangeMap); break;
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
            MainController.instance.dataController.LoadGameSettings();

            GameStart();

            int partType;

            switch (thisPart)
            {
                case TextPart _: partType = 0; break;
                case ChangePart _: partType = 1; break;
                case BattlePart _: partType = 2; break;
                case EventPart _: partType = 3; break;
                default: partType = 4; break;
            }

            _mainAnimator.SetInteger(_aParam.gStady, partType);

            yield return new WaitForSeconds(10f);

            _mainAnimator.SetBool(_aParam.startGameDeskript, false);

        }

        /// <summary> Сообщение о взаимодействии с предметом </summary>
        private void ItemInteract(ItemInteractType interT)
        {
            switch (interT)
            {
                case ItemInteractType.INFO_ITEM:
                    MainController.instance.uIController.InventDescriptMenu(_selectedItem.itemDescript);
                    MainController.instance.uIController.DeactivateInventButton(false);
                    break;

                case ItemInteractType.USE_ITEM:
                    if (_selectedItem is Data.GameItems.UsableItem)
                    {
                        MainController.instance.uIController.InventDescriptMenu(
                            "Вы хотите исспользовать\n\n" +
                            _selectedItem.itemName);
                        MainController.instance.uIController.DeactivateInventButton(true);
                    }
                    else
                    {
                        MainController.instance.uIController.InventDescriptMenu(
                            "Предмет\n\n" +
                            _selectedItem.itemName +
                            "\n\nк сожалению невозможно исспользовать");
                        MainController.instance.uIController.DeactivateInventButton(false);
                    }
                    break;

                case ItemInteractType.REMOVE_ITEM:
                    MainController.instance.uIController.InventDescriptMenu(
                        "Действительно выбросить\n\n" +
                        _selectedItem.itemName +
                        "\n\nОн может вам еще пригодится");
                    MainController.instance.uIController.DeactivateInventButton(true);
                    break;
            }

            _selectedMenuInvent = interT;
            _mainAnimator.SetBool(_aParam.gameInventDescript, true);
        }

        /// <summary> Сообщение о сохранении игры </summary>
        public void SaveGameMessange() => _mainAnimator.SetTrigger(_aParam.effectSave);

        #endregion
    }
}