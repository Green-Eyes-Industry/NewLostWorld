using UnityEngine;

/// <summary> Загрузка и сохранение данных </summary>
public class DataController : MonoBehaviour
{
    // Методы загрузки и выгрузки данных

    public static Player playerData;
    public static GameSettings gameSettingsData;

    /// <summary> Удаление данных и выход </summary>
    public static void DellAllData()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    /// <summary> Сохранить игровые данные </summary>
    public static void SaveGameData()
    {
        PlayerPrefs.SetString("PlayerData", playerData.ToString());
        PlayerPrefs.SetString("GameData", gameSettingsData.ToString());
    }

    /// <summary> Загрузить все данные </summary>
    public static void LoadGameData()
    {
        
    }

    #region GLOBAL_SAVE_METHODS

    /// <summary> Сохранить данные настроек </summary>
    public static void SaveSettingsData()
    {

    }

    #endregion
}