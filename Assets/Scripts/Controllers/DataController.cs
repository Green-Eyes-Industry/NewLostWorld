using UnityEngine;

/// <summary>
/// Загрузка и сохранение данных
/// </summary>
public class DataController : MonoBehaviour
{
    // Методы загрузки и выгрузки данных

    public Player playerData;
    public GameSettings gameSettingsData;

    #region SAVE_DATA

    /// <summary>
    /// Сохранение общих данных игрока
    /// </summary>
    public void SaveGamePreferences()
    {
        // Отношение персонажей к игроку
        // Принятые важные решения
        // Метки на карте
        // Заметки

        // Сохранение последней главы
        PlayerPrefs.SetString("LastPart", "GameParts/" + gameSettingsData.lastPart);

        // Сохранение цвета глаза
        PlayerPrefs.SetFloat("EyeColor", gameSettingsData.eyeColor);

        // Сохранение здоровья игрока
        PlayerPrefs.SetInt("PL_Health", playerData.playerHealth);

        // Сохранение рассудка игрока
        PlayerPrefs.SetInt("PL_Mind", playerData.playerHealth);
    }

    /// <summary>
    /// Сохранение инвентаря
    /// </summary>
    public void SavePlayerInventory()
    {
        // Сохранение инвентаря
        for (int i = 0; i < playerData.playerInventory.Count; i++)
        {
            PlayerPrefs.SetString("PL_Invent_" + i, "GameItems/" + playerData.playerInventory[i].name);
        }

        // Очистка остального инвентаря
        for (int i = playerData.playerInventory.Count; i < 100; i++)
        {
            if (PlayerPrefs.HasKey("PL_Invent_" + i)) PlayerPrefs.DeleteKey("PL_Invent_" + i);
            else break;
        }
    }

    /// <summary>
    /// Сохранение еффектов
    /// </summary>
    public void SavePlayerEffects()
    {
        // Сохранение эффектов
        for (int i = 0; i < playerData.playerEffects.Count; i++)
        {
            PlayerPrefs.SetString("PL_Effects_" + i, "PlayerEffects/" + playerData.playerEffects[i].name);
        }

        // Очистка остальных еффектов
        for (int i = playerData.playerEffects.Count; i < 100; i++)
        {
            if (PlayerPrefs.HasKey("PL_Effects_" + i)) PlayerPrefs.DeleteKey("PL_Effects_" + i);
            else break;
        }
    }

    /// <summary>
    /// Сохранение глобальных настроек
    /// </summary>
    public void SaveGlobalPreferences()
    {
        if (gameSettingsData.soundCheck)
        {
            if (gameSettingsData.vibrationCheck)
            {
                if (gameSettingsData.effectCheck) PlayerPrefs.SetInt("GameData", 111); 
                else PlayerPrefs.SetInt("GameData", 112);
            }
            else
            {
                if (gameSettingsData.effectCheck)PlayerPrefs.SetInt("GameData", 121);
                else PlayerPrefs.SetInt("GameData", 122);
            }
        }
        else
        {
            if (gameSettingsData.vibrationCheck)
            {
                if (gameSettingsData.effectCheck) PlayerPrefs.SetInt("GameData", 211);
                else PlayerPrefs.SetInt("GameData", 212);
            }
            else
            {
                if (gameSettingsData.effectCheck) PlayerPrefs.SetInt("GameData", 221);
                else PlayerPrefs.SetInt("GameData", 222);
            }
        }
    }

    #endregion

    #region LOAD_DATA

    /// <summary>
    /// Загрузка игровых данных
    /// </summary>
    public void LoadGamePreferences()
    {
        // Загрузка последней главы
        if (PlayerPrefs.HasKey("LastPart")) gameSettingsData.lastPart = (GamePart)Resources.Load(PlayerPrefs.GetString("LastPart"));

        // Загрузка здоровья игрока
        if (PlayerPrefs.HasKey("PL_Health")) playerData.playerHealth = PlayerPrefs.GetInt("PL_Health");

        // Загрузка рассудка игрока
        if (PlayerPrefs.HasKey("PL_Mind")) playerData.playerMind = PlayerPrefs.GetInt("PL_Mind");
    }


    /// <summary>
    /// Загрузка глобальных данных
    /// </summary>
    public void LoadGlobalPreferences()
    {
        // Загрузка игровых настроек
        int globalData = 111;
        if (PlayerPrefs.HasKey("GameData")) globalData = PlayerPrefs.GetInt("GameData");
        switch (globalData)
        {
            case 112:
                SwitchGameSettings(true, true, false);
                break;

            case 122:
                SwitchGameSettings(true, false, false);
                break;

            case 222:
                SwitchGameSettings(false, false, false);
                break;

            case 221:
                SwitchGameSettings(false, false, true);
                break;

            case 211:
                SwitchGameSettings(false, true, true);
                break;

            case 212:
                SwitchGameSettings(false, true, false);
                break;

            case 121:
                SwitchGameSettings(true, false, true);
                break;

            default:
                SwitchGameSettings(true, true, true);
                break;
        }

        // Загрузка цвета глаза
        if (PlayerPrefs.HasKey("EyeColor")) gameSettingsData.eyeColor = PlayerPrefs.GetFloat("EyeColor");
    }

    /// <summary>
    /// Смена настроек игры
    /// </summary>
    private void SwitchGameSettings(bool soundSwitch, bool vibrationSwitch, bool effectsSwitch)
    {
        gameSettingsData.soundCheck = soundSwitch;
        gameSettingsData.vibrationCheck = vibrationSwitch;
        gameSettingsData.effectCheck = effectsSwitch;
    }

    #endregion

    /// <summary>
    /// Удаление данных и выход
    /// </summary>
    public void DellAllSaves()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}