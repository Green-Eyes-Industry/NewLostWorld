using UnityEngine;

/// <summary> Загрузка и сохранение данных </summary>
public class DataController : MonoBehaviour
{
    public static Player playerData;
    public static GameSettings gameSettingsData;
    public static NonPlayer[] npsSaveList;

    #region LOAD_METHODS

    /// <summary> Загрузить базовые данные </summary>
    public static void LoadData()
    {
        npsSaveList = new NonPlayer[20];

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

        // Достижения
        if (PlayerPrefs.HasKey("Achive_0"))
        {
            for (int i = 0; i < 100; i++)
            {
                if (PlayerPrefs.HasKey("Achive_" + i))
                {
                    gameSettingsData.gameAchivemants.Add((Achivemants)Resources.Load(
                        "Achivemants/" + PlayerPrefs.GetString("Achive_" + i),
                        typeof(Achivemants)));
                }
                else break;
            }
        }
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
                    playerData.playerInventory.Add((GameItem)Resources.Load(
                        "GameItems/" + PlayerPrefs.GetString("Invent_" + i),
                        typeof(GameItem)));
                }
                else break;
            }
        }

        // Эффекты
        if (PlayerPrefs.HasKey("Effect_0"))
        {
            for (int i = 0; i < 100; i++)
            {
                if (PlayerPrefs.HasKey("Effect_" + i))
                {
                    playerData.playerEffects.Add((GameEffect)Resources.Load(
                        "PlayerEffects/" + PlayerPrefs.GetString("Effect_" + i),
                        typeof(GameEffect)));
                }
                else break;
            }
        }

        // Карта
        if (PlayerPrefs.HasKey("Location_0"))
        {
            for (int i = 0; i < 100; i++)
            {
                if (PlayerPrefs.HasKey("Location_" + i))
                {
                    playerData.playerMap.Add((MapMark)Resources.Load(
                        "Locations/" + PlayerPrefs.GetString("Location_" + i),
                        typeof(MapMark)));
                }
                else break;
            }
        }

        // Заметки
        if (PlayerPrefs.HasKey("Note_0"))
        {
            for (int i = 0; i < 100; i++)
            {
                if (PlayerPrefs.HasKey("Note_" + i))
                {
                    playerData.playerNotes.Add((Note)Resources.Load(
                        "Notes/" + PlayerPrefs.GetString("Note_" + i),
                        typeof(Note)));
                }
                else break;
            }
        }

        // Решения
        if (PlayerPrefs.HasKey("Decision_0"))
        {
            for (int i = 0; i < 100; i++)
            {
                if (PlayerPrefs.HasKey("Decision_" + i))
                {
                    playerData.playerDecisions.Add((Decision)Resources.Load(
                        "Decisions/" + PlayerPrefs.GetString("Decision_" + i),
                        typeof(Decision)));
                }
                else break;
            }
        }
    }

    /// <summary> Загрузить отношение к игроку </summary>
    public static void LoadNonPlayerRatio(NonPlayer n)
    {
        if (PlayerPrefs.HasKey(n.name)) n.npToPlayerRatio = PlayerPrefs.GetInt(n.name);
    }

    #endregion

    #region SAVE_METHODS

    /// <summary> Сохранить последнюю главу </summary>
    public static void SaveLastPart()
    {
        PlayerPrefs.SetString("LastPart", AnimController.thisPart.name);
        PlayerPrefs.Save();
    }

    /// <summary> Сохранить характеристики персонажа </summary>
    public static void SaveCharacteristic()
    {
        PlayerPrefs.SetInt("Health", playerData.playerHealth);
        PlayerPrefs.SetInt("Mind", playerData.playerMind);
        PlayerPrefs.Save();
    }

    /// <summary> Сохранить инвентарь персонажа </summary>
    public static void SaveInventory()
    {
        if (playerData.playerInventory != null)
        {
            // Сохранение
            for (int i = 0; i < playerData.playerInventory.Count; i++)
            {
                PlayerPrefs.SetString("Invent_" + i, playerData.playerInventory[i].name);
            }

            // Очистка от лишнего
            if (playerData.playerInventory.Count > 0)
            {
                for (int i = playerData.playerInventory.Count - 1; i < 50; i++)
                {
                    if (PlayerPrefs.HasKey("Invent_" + i)) PlayerPrefs.DeleteKey("Invent_" + i);
                    else break;
                }
            }
        }
        PlayerPrefs.Save();
    }

    /// <summary> Сохранить эффекты на персонаже </summary>
    public static void SaveEffects()
    {
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
        PlayerPrefs.Save();
    }

    /// <summary> Сохранить карту </summary>
    public static void SaveMap()
    {
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
        PlayerPrefs.Save();
    }

    /// <summary> Сохранить заметки </summary>
    public static void SaveNotes()
    {
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

    /// <summary> Сохранить решения </summary>
    public static void SaveDecisons()
    {
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
        if (gameSettingsData.gameAchivemants != null)
        {
            // Сохранение
            for (int i = 0; i < gameSettingsData.gameAchivemants.Count; i++)
            {
                PlayerPrefs.SetString("Achive_" + i, gameSettingsData.gameAchivemants[i].name);
            }

            // Очистка от лишнего
            if (gameSettingsData.gameAchivemants.Count > 0)
            {
                for (int i = gameSettingsData.gameAchivemants.Count - 1; i < 50; i++)
                {
                    if (PlayerPrefs.HasKey("Achive_" + i)) PlayerPrefs.DeleteKey("Achive_" + i);
                    else break;
                }
            }
        }

        PlayerPrefs.Save();
    }

    /// <summary> Добавить НПС к измененным </summary>
    public static void SaveNonPlayerRatio(NonPlayer n)
    {
        int id = 0;

        for (int i = 0; i < npsSaveList.Length; i++)
        {
            if (npsSaveList[i] == null) id = i;
            if (npsSaveList[i] == n) return;
        }

        npsSaveList[id] = n;
    }

    /// <summary> Сохранить влияние игрока </summary>
    public static void SaveAllRatio()
    {
        for (int i = 0; i < npsSaveList.Length; i++)
        {
            if (npsSaveList[i] != null) PlayerPrefs.SetInt(npsSaveList[i].name, npsSaveList[i].npToPlayerRatio);
        }
    }

    #endregion

    /// <summary> Удаление данных и выход </summary>
    public static void DellAllData(bool exit)
    {
        PlayerPrefs.DeleteAll();
        if (exit) Application.Quit();
    }
}