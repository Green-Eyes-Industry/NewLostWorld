﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary> Замена текста на кнопках и контроль UI </summary>
public class TextController : MonoBehaviour
{
    #region CONNECTED_VAR

    [SerializeField] private float _gameWaitToSwitch;

    [SerializeField] private RectTransform _eyeCenter;

    [SerializeField] private Text _menuButton_1;
    [SerializeField] private Text _menuButton_2;
    [SerializeField] private Text _menuButton_3;
    [SerializeField] private Text _menuButton_4;

    [SerializeField] private Text _gameMain_Txt;
    [SerializeField] private Text _gameButton_1_Txt;
    [SerializeField] private Text _gameButton_2_Txt;
    [SerializeField] private Text _gameButton_3_Txt;

    [SerializeField] private Text _gameMessageInventoryTxt;
    [SerializeField] private Text _gameMessageCharacterTxt;
    [SerializeField] private Text _gameMessageMapTxt;
    [SerializeField] private Text _gameMessageNotesTxt;

    private Gyroscope _mainGyro;
    private bool _isGyroEnable;
    private Vector2 _eyeFixPosition;

    #endregion

    private void Start() => ConnectGyroscope();

    private void Update() => EyeWatching();

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
    public void GameButton_1(string newTxt) => StartCoroutine(WaitToGameButton_1_Switch(_gameWaitToSwitch, newTxt));

    /// <summary> Замена текста на второй игровой кнопке </summary>
    public void GameButton_2(string newTxt) => StartCoroutine(WaitToGameButton_2_Switch(_gameWaitToSwitch, newTxt));

    /// <summary> Замена текста на третьей игровой кнопке </summary>
    public void GameButton_3(string newTxt) => StartCoroutine(WaitToGameButton_3_Switch(_gameWaitToSwitch, newTxt));

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

    /// <summary> Перерисовать инвентарь </summary>
    public void RepaintInventory()
    {
        if (DataController.playerData.playerInventory.Count != 0)
        {
            for (int i = 0; i < DataController.playerData.playerInventory.Count; i++)
            {
                //  _mainPlayer.playerInventory[i].itemIco;
            }
        }
    }

    #endregion

    #region WAIT_SWITCH

    /// <summary> Задержка при переходе  главное меню </summary>
    private IEnumerator WaitToMenuSwitch(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _menuButton_1.text = "Новая игра";
        _menuButton_2.text = "Настройки";
        _menuButton_3.text = "Об Авторах";
        _menuButton_4.text = "Достижения";
    }

    /// <summary> Задержка при переходе в настройки </summary>
    private IEnumerator WaitToSettingsSwitch(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _menuButton_1.text = "Звук";
        _menuButton_2.text = "Вибриция";
        _menuButton_3.text = "Эффекты";
        _menuButton_4.text = "Назад";
    }

    /// <summary> Задержка при переходе в меню автора </summary>
    private IEnumerator WaitToAboutSwitch(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _menuButton_4.text = "Назад";
    }

    /// <summary> Задержка замена основного текста в игре </summary>
    private IEnumerator WaitToMainTextSwitch(float waitTime, string newTxt)
    {
        yield return new WaitForSeconds(waitTime);

        _gameMain_Txt.text = newTxt;
    }

    /// <summary> Задержка при замене текста на первой кнопке </summary>
    private IEnumerator WaitToGameButton_1_Switch(float waitTime, string newTxt)
    {
        yield return new WaitForSeconds(waitTime);

        _gameButton_1_Txt.text = newTxt;
    }

    /// <summary> Задержка при замене текста на второй кнопке </summary>
    private IEnumerator WaitToGameButton_2_Switch(float waitTime, string newTxt)
    {
        yield return new WaitForSeconds(waitTime);

        _gameButton_2_Txt.text = newTxt;
    }

    /// <summary> Задержка при замене текста на третей кнопке </summary>
    private IEnumerator WaitToGameButton_3_Switch(float waitTime, string newTxt)
    {
        yield return new WaitForSeconds(waitTime);

        _gameButton_3_Txt.text = newTxt;
    }

    #endregion
}