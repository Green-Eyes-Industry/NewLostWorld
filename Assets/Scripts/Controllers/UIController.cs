using System.Collections;
using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    /// <summary> Контроль UI </summary>
    public class UIController : ParentController
    {
        #region VARIABLES

        private bool _isEventStarted; // Вы в главе евента
        private float _timeForEvent; // Время на эвент

        // Базовые

        private float _gameWaitToSwitch;
        public RectTransform eyeCenter;

        // Достижения

        public Sprite achiveClose;
        public Sprite achiveOpen;

        public Image[] achiveCase;

        public Text achiveDescriptText;

        // Кнопки главного меню

        public Text[] menuButton; 

        // Игровая глава

        public Text gameMainText;
        public Text[] gameButton;
        public Image timerImage;
        public Image finalAchiveIco;
        
        // Сообщения в игре

        public Text gameMessageInventoryTxt;
        public Text gameMessageCharacterTxt;
        public Text gameMessageMapTxt;
        public Text gameMessageNotesTxt;

        // Инвентарь

        public Image[] inventCase;
        public Text inventDescriptMenu;
        public Text inventNoButtonText;
        public GameObject inventYesButton;

        // Эффекты в меню персонажа

        public Image[] effectCase;
        public Text effectDescriptText;
        public Text menuPlayerInventoryHelpText;
        public Image playerHealthFill;
        public Image playerMindFill;

        private Gyroscope _mainGyro;
        private bool _isGyroEnable;
        private Vector2 _eyeFixPosition;

        #endregion

        public override void Init()
        {
            _gameWaitToSwitch = 0.2f;

            // Start

            TimeEvent(false, 0f);
            ConnectGyroscope();
        }

        public void IUpdate()
        {
            if (_isEventStarted) TimerEventVision();
            EyeWatching();
        }

        #region VISUAL_EFFECTS

        /// <summary> Старт эвента </summary>
        public void TimeEvent(bool isStart, float timeSec)
        {
            _isEventStarted = isStart;
            if (isStart) _timeForEvent = (1 / timeSec);
            else _timeForEvent = 0f;
        }

        /// <summary> Таймер в главе эвента </summary>
        private void TimerEventVision()
        {
            if (timerImage.fillAmount > 0) timerImage.fillAmount -= _timeForEvent * Time.deltaTime;
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
        public void GameMain(string newTxt) => StartCoroutine(WaitToMainTextSwitch(_gameWaitToSwitch, newTxt));

        /// <summary> Замена текста на первой игровой кнопке </summary>
        public void GameButton(int id, string newTxt) => StartCoroutine(WaitToGameButton_Switch(id,_gameWaitToSwitch, newTxt));

        /// <summary> Текст сообщения инвентаря </summary>
        public void GameMessageInventory(string newTxt) => gameMessageInventoryTxt.text = newTxt;

        /// <summary> Текст сообщения персонажа </summary>
        public void GameMessageCharacter(string newTxt) => gameMessageCharacterTxt.text = newTxt;

        /// <summary> Текст сообщения карты </summary>
        public void GameMessageMap(string newTxt) => gameMessageMapTxt.text = newTxt;

        /// <summary> Текст сообщения заметок </summary>
        public void GameMessageNotes(string newTxt) => gameMessageNotesTxt.text = newTxt;

        /// <summary> Текст подробностей о предмете в инвентаре </summary>
        public void InventDescriptMenu(string newTxt) => inventDescriptMenu.text = newTxt;

        #endregion

        #region UI_CONTROL

        /// <summary> Отобразить инвентарь </summary>
        public void ShowInventory(int page)
        {
            menuPlayerInventoryHelpText.text = "Инвентарь";

            for (int i = 0; i < MainController.instance.dataController.mainPlayer.playerInventory.Count; i++)
            {
                if (MainController.instance.dataController.mainPlayer.playerInventory == null)
                {
                    MainController.instance.dataController.mainPlayer.playerInventory.RemoveAt(i);
                    if (i > 0) i--;
                }
            }

            int itemCount = MainController.instance.dataController.mainPlayer.playerInventory.Count;

            page *= inventCase.Length;

            for (int i = 0; i < inventCase.Length; i++)
            {
                if (i + page < itemCount)
                {
                    inventCase[i].gameObject.SetActive(true);
                    inventCase[i].sprite = MainController.instance.dataController.mainPlayer.playerInventory[i + page].itemIco;
                }
                else inventCase[i].gameObject.SetActive(false);
            }
        }

        /// <summary> Отобразить эффекты на персонаже </summary>
        public void ShowEffects()
        {
            int effectsCount = MainController.instance.dataController.mainPlayer.playerEffects.Count;

            menuPlayerInventoryHelpText.text = "Персонаж";
            GameMessageCharacter("Отображает ваше текущее состояние");

            playerHealthFill.fillAmount = MainController.instance.dataController.mainPlayer.playerHealth / 100f;
            playerMindFill.fillAmount = MainController.instance.dataController.mainPlayer.playerMind / 100f;

            for (int i = 0; i < effectCase.Length; i++)
            {
                if (i < effectsCount)
                {
                    effectCase[i].gameObject.SetActive(true);
                    effectCase[i].sprite = MainController.instance.dataController.mainPlayer.playerEffects[i].icoEffect;
                }
                else effectCase[i].gameObject.SetActive(false);
            }
        }

        /// <summary> Отобразить подробности об эффекте </summary>
        public void ShowEffectDescript(int id)
        {
            Data.GameEffect effect = MainController.instance.dataController.mainPlayer.playerEffects[id];
            string action = (effect is Data.GameEffects.NegativeEffect) ? "Отнимает " : "Дает ";

            effectDescriptText.text = effect.nameEffect + "\n\n" +
                ((effect.healthInfluenceEffect != 0) ? action + effect.healthInfluenceEffect + " здоровья\n" : "") +
                ((effect.mindInfluenceEffect != 0) ? action + effect.mindInfluenceEffect + " рассудка" : "") +
                "\n\n" + "Осталось " + effect.durationEffect + " дней";
        }

        /// <summary> Отобразить достижения </summary>
        public void ShowAchive(int page)
        {
            for (int i = 0; i < MainController.instance.dataController.mainSettings.gameAchivemants.Count; i++)
            {
                if (MainController.instance.dataController.mainSettings.gameAchivemants == null)
                {
                    MainController.instance.dataController.mainSettings.gameAchivemants.RemoveAt(i);
                    if (i > 0) i--;
                }
            }

            int achivesLendth = MainController.instance.dataController.mainSettings.gameAchivemants.Count;

            page *= achiveCase.Length;

            for (int i = 0; i < achiveCase.Length; i++)
            {
                if (i + page < achivesLendth)
                {
                    achiveCase[i].gameObject.transform.parent.GetComponent<Image>().sprite = achiveOpen;
                    achiveCase[i].gameObject.SetActive(true);
                    achiveCase[i].sprite = MainController.instance.dataController.mainSettings.gameAchivemants[i + page].achiveIco;
                }
                else
                {
                    achiveCase[i].gameObject.transform.parent.GetComponent<Image>().sprite = achiveClose;
                    achiveCase[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary> Показать подробности о достижении </summary>
        public void ShowAchiveDescript(Achivemants achive) => achiveDescriptText.text = achive.achiveDescript;

        /// <summary> Отобразить значек получаемого достижения </summary>
        public void ShowFinalAchiveIco(Achivemants achive)
        {
            finalAchiveIco.sprite = achive.achiveIco;
        }

        /// <summary> Переключение кнопок в меню подробностей инвентаря </summary>
        public void DeactivateInventButton(bool isAct)
        {
            if (isAct) inventNoButtonText.text = "Нет";
            else inventNoButtonText.text = "Назад";

            inventYesButton.SetActive(isAct);
        }

        #endregion

        #region CUROTINES

        /// <summary> Задержка при переходе  главное меню </summary>
        private IEnumerator WaitToMenuSwitch(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            string mainText = MainController.instance.dataController.mainSettings.lastPart != null ? "Продолжить" : "Новая игра";

            menuButton[0].text = mainText;
            menuButton[1].text = "Настройки";
            menuButton[2].text = "Об авторах";
            menuButton[3].text = "Достижения";
        }

        /// <summary> Задержка при переходе в настройки </summary>
        private IEnumerator WaitToSettingsSwitch(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            menuButton[0].text = "Звук";
            menuButton[1].text = "Вибрация";
            menuButton[2].text = "Эффекты";
            menuButton[3].text = "Назад";
        }

        /// <summary> Задержка при переходе в меню автора </summary>
        private IEnumerator WaitToAboutSwitch(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            menuButton[3].text = "Назад";
        }

        /// <summary> Задержка замена основного текста в игре </summary>
        private IEnumerator WaitToMainTextSwitch(float waitTime, string newTxt)
        {
            yield return new WaitForSeconds(waitTime);

            gameMainText.text = newTxt;
        }

        /// <summary> Задержка при замене текста на кнопке </summary>
        private IEnumerator WaitToGameButton_Switch(int id, float waitTime, string newTxt)
        {
            yield return new WaitForSeconds(waitTime);

            gameButton[id].text = newTxt;
        }

        #endregion
    }
}