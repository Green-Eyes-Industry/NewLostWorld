using UnityEngine;

/// <summary> Загрузка и сохранение данных </summary>
public class DataController : MonoBehaviour
{
    public static Player playerData;
    public static GameSettings gameSettingsData;

    /// <summary> Загрузить базовые данные </summary>
    public static void LoadData()
    {
        if (PlayerPrefs.HasKey("LastPart"))
        {
            // Глава на которой закончили
            gameSettingsData.lastPart = (GamePart)Resources.Load("GameParts/" + PlayerPrefs.GetString("LastPart"), typeof(GamePart));
        }

        // Загрузка настроек

        if (PlayerPrefs.HasKey("Sound")) gameSettingsData.isSoundCheck = true;
        else gameSettingsData.isSoundCheck = false;

        if (PlayerPrefs.HasKey("Vibration")) gameSettingsData.isVibrationCheck = true;
        else gameSettingsData.isVibrationCheck = false;

        if (PlayerPrefs.HasKey("Effects")) gameSettingsData.isEffectCheck = true;
        else gameSettingsData.isEffectCheck = false;
    }

    /// <summary> Загрузка данных для игры </summary>
    public static void LoadGameData()
    {
        // Характеристики игрока
        if (PlayerPrefs.HasKey("Health")) playerData.playerHealth = PlayerPrefs.GetInt("Health");
        if (PlayerPrefs.HasKey("Mind")) playerData.playerMind = PlayerPrefs.GetInt("Mind");

        // Инвентарь
        if (PlayerPrefs.HasKey("Invent_0"))
        {
            for (int i = 0; i < 100; i++)
            {
                if (PlayerPrefs.HasKey("Invent_" + i))
                {
                    playerData.playerInventory.Add((GameItem)Resources.Load("GameItems/" + PlayerPrefs.GetString("Invent_" + i), typeof(GameItem)));
                }
                else break;
            }
        }
    }

    #region SAVE_METHODS

    /// <summary> Сохранить игровые данные </summary>
    public static void SaveGameData()
    {
        // TODO : Сохранения в процессе игры

        // Глава на которой закончили
        PlayerPrefs.SetString("LastPart", MoveController.thisPart.name);

        // Характеристики игрока
        PlayerPrefs.SetInt("Health", playerData.playerHealth);
        PlayerPrefs.SetInt("Mind", playerData.playerMind);

        // Инвентарь
        if(playerData.playerInventory != null)
        {
            // Сохранение
            for (int i = 0; i < playerData.playerInventory.Count; i++)
            {
                PlayerPrefs.SetString("Invent_" + i, playerData.playerInventory[i].name);
            }

            // Очистка от лишнего
            if(playerData.playerInventory.Count > 0)
            {
                for (int i = playerData.playerInventory.Count - 1; i < 50; i++)
                {
                    if (PlayerPrefs.HasKey("Invent_" + i)) PlayerPrefs.DeleteKey("Invent_" + i);
                    else break;
                }
            }
        }

        // Эффекты
        if (playerData.playerEffects != null)
        {
            // Сохранение
            for (int i = 0; i < playerData.playerEffects.Count; i++)
            {
                PlayerPrefs.SetString("Effect_" + i, playerData.playerEffects[i].name);
            }

            // Очистка от лишнего
            if (playerData.playerEffects.Count > 0)
            {
                for (int i = playerData.playerEffects.Count - 1; i < 50; i++)
                {
                    if (PlayerPrefs.HasKey("Effect_" + i)) PlayerPrefs.DeleteKey("Effect_" + i);
                    else break;
                }
            }
        }

        // Карта
        if (playerData.playerMap != null)
        {
            // Сохранение
            for (int i = 0; i < playerData.playerMap.Count; i++)
            {
                PlayerPrefs.SetString("Location_" + i, playerData.playerMap[i].name);
            }

            // Очистка от лишнего
            if (playerData.playerMap.Count > 0)
            {
                for (int i = playerData.playerMap.Count - 1; i < 50; i++)
                {
                    if (PlayerPrefs.HasKey("Location_" + i)) PlayerPrefs.DeleteKey("Location_" + i);
                    else break;
                }
            }
        }

        // Заметки
        if (playerData.playerNotes != null)
        {
            // Сохранение
            for (int i = 0; i < playerData.playerNotes.Count; i++)
            {
                PlayerPrefs.SetString("Note_" + i, playerData.playerNotes[i].name);
            }

            // Очистка от лишнего
            if (playerData.playerNotes.Count > 0)
            {
                for (int i = playerData.playerNotes.Count - 1; i < 50; i++)
                {
                    if (PlayerPrefs.HasKey("Note_" + i)) PlayerPrefs.DeleteKey("Note_" + i);
                    else break;
                }
            }
        }

        // Решения
        if (playerData.playerDecisions != null)
        {
            // Сохранение
            for (int i = 0; i < playerData.playerDecisions.Count; i++)
            {
                PlayerPrefs.SetString("Decision_" + i, playerData.playerDecisions[i].name);
            }

            // Очистка от лишнего
            if (playerData.playerDecisions.Count > 0)
            {
                for (int i = playerData.playerDecisions.Count - 1; i < 50; i++)
                {
                    if (PlayerPrefs.HasKey("Decision_" + i)) PlayerPrefs.DeleteKey("Decision_" + i);
                    else break;
                }
            }
        }

        PlayerPrefs.Save();
    }

    /// <summary> Сохранить данные настроек </summary>
    public static void SaveSettingsData()
    {
        if (gameSettingsData.isSoundCheck) PlayerPrefs.SetInt("Sound", 1);
        else PlayerPrefs.DeleteKey("Sound");

        if (gameSettingsData.isVibrationCheck) PlayerPrefs.SetInt("Vibration", 1);
        else PlayerPrefs.DeleteKey("Vibration");

        if (gameSettingsData.isEffectCheck) PlayerPrefs.SetInt("Effects", 1);
        else PlayerPrefs.DeleteKey("Effects");

        PlayerPrefs.Save();
    }

    /// <summary> Сохранить данные достижений </summary>
    public static void SaveAchivesData()
    {
        // TODO : Сохранение достижений
    }

    #endregion

    /// <summary> Удаление данных и выход </summary>
    public static void DellAllData(bool exit)
    {
        PlayerPrefs.DeleteAll();
        if (exit) Application.Quit();
    }
}