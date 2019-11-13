using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Замена текста на кнопках и контроль UI
/// </summary>
public class TextController : MonoBehaviour
{
    #region CONNECTED_VAR

    [SerializeField] private Text _menuStartButtonTxt;
    [SerializeField] private Text _gameMain_Txt;
    [SerializeField] private Text _gameButton_1_Txt;
    [SerializeField] private Text _gameButton_2_Txt;
    [SerializeField] private Text _gameButton_3_Txt;

    [SerializeField] private Text _gameMessageInventoryTxt;
    [SerializeField] private Text _gameMessageCharacterTxt;
    [SerializeField] private Text _gameMessageMapTxt;
    [SerializeField] private Text _gameMessageNotesTxt;

    private Player _mainPlayer;

    #endregion

    private void Start()
    {
        _mainPlayer = GetComponent<DataController>().playerData;
    }

    #region TEXT_CONTROL

    /// <summary>
    /// Замена текста на первой кнопке меню
    /// </summary>
    public void MenuStartButton(string newTxt)
    {
        _menuStartButtonTxt.text = newTxt;
    }

    /// <summary>
    /// Замена основного текста в игре
    /// </summary>
    public void GameMain (string newTxt)
    {
        _gameMain_Txt.text = newTxt;
    }

    /// <summary>
    /// Замена текста на первой игровой кнопке
    /// </summary>
    public void GameButton_1(string newTxt)
    {
        _gameButton_1_Txt.text = newTxt;
    }

    /// <summary>
    /// Замена текста на второй игровой кнопке
    /// </summary>
    public void GameButton_2(string newTxt)
    {
        _gameButton_2_Txt.text = newTxt;
    }

    /// <summary>
    /// Замена текста на третьей игровой кнопке
    /// </summary>
    public void GameButton_3(string newTxt)
    {
        _gameButton_3_Txt.text = newTxt;
    }

    /// <summary>
    /// Текст сообщения инвентаря
    /// </summary>
    public void GameMessageInventory(string newTxt)
    {
        _gameMessageInventoryTxt.text = newTxt;
    }

    /// <summary>
    /// Текст сообщения персонажа
    /// </summary>
    public void GameMessageCharacter(string newTxt)
    {
        _gameMessageCharacterTxt.text = newTxt;
    }

    /// <summary>
    /// Текст сообщения карты
    /// </summary>
    public void GameMessageMap(string newTxt)
    {
        _gameMessageMapTxt.text = newTxt;
    }

    /// <summary>
    /// Текст сообщения заметок
    /// </summary>
    public void GameMessageNotes(string newTxt)
    {
        _gameMessageNotesTxt.text = newTxt;
    }

    #endregion

    #region UI_CONTROL

    public void RepaintInventory()
    {
        if (_mainPlayer.playerInventory.Count != 0)
        {
            for (int i = 0; i < _mainPlayer.playerInventory.Count; i++)
            {
                //  _mainPlayer.playerInventory[i].itemIco;
            }
        }
    }

    #endregion
}