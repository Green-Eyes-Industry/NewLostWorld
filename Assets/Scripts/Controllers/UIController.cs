using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NLW
{
    /// <summary> Контроль UI </summary>
    public class UIController : MainController
    {
        #region VARIABLES

        private bool _isEventStarted; // Вы в главе евента
        private float _timeForEvent; // Время на эвент

        // Базовые

        private float _gameWaitToSwitch;
        public RectTransform _eyeCenter;

        // Достижения

        public Sprite _achiveClose;
        public Sprite _achiveOpen;

        public Image[] _achiveCase;

        public Text _achiveDescript_txt;

        // Кнопки главного меню

        public Text[] _menuButton; 

        // Игровая глава

        public Text _gameMain_Txt;
        public Text[] _gameButton;
        public Image _timerImage, finalAchiveIco;
        
        // Сообщения в игре

        public Text _gameMessageInventoryTxt;
        public Text _gameMessageCharacterTxt;
        public Text _gameMessageMapTxt;
        public Text _gameMessageNotesTxt;

        // Инвентарь

        public Image[] _inventCase;

        // Эффекты в меню персонажа

        public Image[] _effectCase;

        private Gyroscope _mainGyro;
        private bool _isGyroEnable;
        private Vector2 _eyeFixPosition;

        #endregion

        protected override void Init()
        {
            _gameWaitToSwitch = 0.2f;

            // Start

            TimeEvent(false, 0f);
            ConnectGyroscope();
        }

        private void Update()
        {
            if (_isEventStarted) TimerEventVision();
            EyeWatching();
        }

        #region VISUAL_EFFECTS

        /// <summary> Старт евента </summary>
        public void TimeEvent(bool isStart, float timeSec)
        {
            _isEventStarted = isStart;
            if (isStart) _timeForEvent = (1 / timeSec);
            else _timeForEvent = 0f;
        }

        /// <summary> Таймер в главе евента </summary>
        private void TimerEventVision()
        {
            if (_timerImage.fillAmount > 0) _timerImage.fillAmount -= _timeForEvent * Time.deltaTime;
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
                _eyeCenter.anchoredPosition = new Vector2(0, 40);
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

                _eyeCenter.anchoredPosition = _eyeFixPosition;
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
        public void GameMessageInventory(string newTxt) => _gameMessageInventoryTxt.text = newTxt;

        /// <summary> Текст сообщения персонажа </summary>
        public void GameMessageCharacter(string newTxt) => _gameMessageCharacterTxt.text = newTxt;

        /// <summary> Текст сообщения карты </summary>
        public void GameMessageMap(string newTxt) => _gameMessageMapTxt.text = newTxt;

        /// <summary> Текст сообщения заметок </summary>
        public void GameMessageNotes(string newTxt) => _gameMessageNotesTxt.text = newTxt;

        #endregion

        #region UI_CONTROL

        /// <summary> Отобразить инвентарь </summary>
        public void ShowInventory(int page)
        {
            int itemCount = mainPlayer.playerInventory.Count;

            page *= _inventCase.Length;

            for (int i = 0; i < _inventCase.Length; i++)
            {
                if (i + page < itemCount)
                {
                    _inventCase[i].gameObject.SetActive(true);
                    _inventCase[i].sprite = mainPlayer.playerInventory[i + page].itemIco;
                }
                else _inventCase[i].gameObject.SetActive(false);
            }
        }

        /// <summary> Отобразить еффекты на персонаже </summary>
        public void ShowEffects()
        {
            int effectsCount = mainPlayer.playerEffects.Count;

            for (int i = 0; i < _effectCase.Length; i++)
            {
                if (i < effectsCount)
                {
                    _effectCase[i].gameObject.SetActive(true);
                    _effectCase[i].sprite = mainPlayer.playerEffects[i].icoEffect;
                }
                else _effectCase[i].gameObject.SetActive(false);
            }
        }

        /// <summary> Отобразить достижения </summary>
        public void ShowAchive(int page)
        {
            int achivesLendth = mainSettings.gameAchivemants.Count;

            page *= _achiveCase.Length;

            for (int i = 0; i < _achiveCase.Length; i++)
            {
                if (i + page < achivesLendth)
                {
                    _achiveCase[i].gameObject.transform.parent.GetComponent<Image>().sprite = _achiveOpen;
                    _achiveCase[i].gameObject.SetActive(true);
                    _achiveCase[i].sprite = mainSettings.gameAchivemants[i + page].achiveIco;
                }
                else
                {
                    _achiveCase[i].gameObject.transform.parent.GetComponent<Image>().sprite = _achiveClose;
                    _achiveCase[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary> Показать подробности о достижении </summary>
        public void ShowAchiveDescript(Data.Achivemants achive) => _achiveDescript_txt.text = achive.achiveDescript;

        /// <summary> Отобразить значек получаемого достижения </summary>
        public void ShowFinalAchiveIco(Data.Achivemants achive)
        {
            finalAchiveIco.sprite = achive.achiveIco;
        }

        #endregion

        #region WAIT_SWITCH

        /// <summary> Задержка при переходе  главное меню </summary>
        private IEnumerator WaitToMenuSwitch(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            string mainText;

            if (mainSettings.lastPart != null) mainText = "Продолжить";
            else mainText = "Новая игра";

            _menuButton[0].text = mainText;
            _menuButton[1].text = "Настройки";
            _menuButton[2].text = "Об Авторах";
            _menuButton[3].text = "Достижения";
        }

        /// <summary> Задержка при переходе в настройки </summary>
        private IEnumerator WaitToSettingsSwitch(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            _menuButton[0].text = "Звук";
            _menuButton[1].text = "Вибрация";
            _menuButton[2].text = "Эффекты";
            _menuButton[3].text = "Назад";
        }

        /// <summary> Задержка при переходе в меню автора </summary>
        private IEnumerator WaitToAboutSwitch(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            _menuButton[3].text = "Назад";
        }

        /// <summary> Задержка замена основного текста в игре </summary>
        private IEnumerator WaitToMainTextSwitch(float waitTime, string newTxt)
        {
            yield return new WaitForSeconds(waitTime);

            _gameMain_Txt.text = newTxt;
        }

        /// <summary> Задержка при замене текста на кнопке </summary>
        private IEnumerator WaitToGameButton_Switch(int id, float waitTime, string newTxt)
        {
            yield return new WaitForSeconds(waitTime);

            _gameButton[id].text = newTxt;
        }

        #endregion
    }
}