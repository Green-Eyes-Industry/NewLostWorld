using UnityEngine;
using System.Collections;
using NLW.Data;
using NLW.Parts;

namespace NLW
{
    /// <summary> Загрузка и сохранение данных </summary>
    public class DataController : ParentController
    {
        public static NonPlayer[] npsSaveList;
        [HideInInspector] public Player mainPlayer;
        [HideInInspector] public GameSettings mainSettings;

        #region RESOURCES_PATH

        private const string PlayerPath = "Players/MainPlayer";
        private const string SettingsPath = "MainSettings";

        private const string PartsPath = "GameParts/";
        private const string AchivesPath = "Achivemants/";
        private const string InventoryPath = "GameItems/";
        private const string EffectsPath = "PlayerEffects/";
        private const string LocationsPath = "Locations/";
        private const string NotesPath = "Notes/";
        private const string DecisionsPath = "Decisions/";

        #endregion

        #region SAVE_KEYS

        private const string AchivesKey = "Achive_";
        private const string LastPartKey = "LastPart";
        private const string SoundSettingsKey = "Sound";
        private const string VibrationSettingsKey = "Vibration";
        private const string EffectsSettingsKey = "Effects";

        private const string PlayerHealthKey = "Health";
        private const string PlayerMindKey = "Mind";

        private const string PlayerInventoryKey = "Invent_";
        private const string PlayerEffectsKey = "Effect_";
        private const string PlayerLocationsKey = "Location_";
        private const string PlayerNotesKey = "Note_";
        private const string PlayerDecisionsKey = "Decision_";

        #endregion

        public override void Init()
        {
            LoadData();
        }

        #region LOAD_METHODS

        /// <summary> Загрузить базовые данные </summary>
        private void LoadData()
        {
            npsSaveList = new NonPlayer[20];

            mainPlayer = (Player)Resources.Load(PlayerPath, typeof(Player));
            mainSettings = (GameSettings)Resources.Load(SettingsPath, typeof(GameSettings));

            if (PlayerPrefs.HasKey(LastPartKey))
            {
                // Глава на которой закончили
                if ((GamePart)Resources.Load(PartsPath + PlayerPrefs.GetString(LastPartKey), typeof(GamePart)) != null)
                {
                    mainSettings.lastPart = (GamePart)Resources.Load(PartsPath + PlayerPrefs.GetString(LastPartKey), typeof(GamePart));
                }
            }

            // Загрузка настроек

            mainSettings.isSoundCheck = PlayerPrefs.HasKey(SoundSettingsKey);
            mainSettings.isVibrationCheck = PlayerPrefs.HasKey(VibrationSettingsKey);
            mainSettings.isEffectCheck = PlayerPrefs.HasKey(EffectsSettingsKey);

            // Достижения
            if (!PlayerPrefs.HasKey(AchivesKey + 0)) return;
            
            for (int i = 0; i < 100; i++)
            {
                if (PlayerPrefs.HasKey(AchivesKey + i))
                {
                    mainSettings.gameAchivemants.Add((Achivemants)Resources.Load(
                        AchivesPath + PlayerPrefs.GetString(AchivesKey + i),
                        typeof(Achivemants)));
                }
                else break;
            }
        }

        /// <summary> Загрузка данных для игры </summary>
        public void LoadGameData()
        {
            // Характеристики игрока
            if (PlayerPrefs.HasKey(PlayerHealthKey)) mainPlayer.playerHealth = PlayerPrefs.GetInt(PlayerHealthKey);
            if (PlayerPrefs.HasKey(PlayerMindKey)) mainPlayer.playerMind = PlayerPrefs.GetInt(PlayerMindKey);

            // Инвентарь
            if (PlayerPrefs.HasKey(PlayerInventoryKey + 0))
            {
                for (int i = 0; i < 100; i++)
                {
                    if (PlayerPrefs.HasKey(PlayerInventoryKey + i))
                    {
                        mainPlayer.playerInventory.Add((GameItem)Resources.Load(
                            InventoryPath + PlayerPrefs.GetString(PlayerInventoryKey + i),
                            typeof(GameItem)));
                    }
                    else break;
                }
            }

            // Эффекты
            if (PlayerPrefs.HasKey(PlayerEffectsKey + 0))
            {
                for (int i = 0; i < 100; i++)
                {
                    if (PlayerPrefs.HasKey(PlayerEffectsKey + i))
                    {
                        mainPlayer.playerEffects.Add((GameEffect)Resources.Load(
                            EffectsPath + PlayerPrefs.GetString(PlayerEffectsKey + i),
                            typeof(GameEffect)));
                    }
                    else break;
                }
            }

            // Карта
            if (PlayerPrefs.HasKey(PlayerLocationsKey + 0))
            {
                for (int i = 0; i < 100; i++)
                {
                    if (PlayerPrefs.HasKey(PlayerLocationsKey + i))
                    {
                        mainPlayer.playerMap.Add((MapMark)Resources.Load(
                            LocationsPath + PlayerPrefs.GetString(PlayerLocationsKey + i),
                            typeof(MapMark)));
                    }
                    else break;
                }
            }

            // Заметки
            if (PlayerPrefs.HasKey(PlayerNotesKey + 0))
            {
                for (int i = 0; i < 100; i++)
                {
                    if (PlayerPrefs.HasKey(PlayerNotesKey + i))
                    {
                        mainPlayer.playerNotes.Add((Note)Resources.Load(
                            NotesPath + PlayerPrefs.GetString(PlayerNotesKey + i),
                            typeof(Note)));
                    }
                    else break;
                }
            }

            // Решения
            if (!PlayerPrefs.HasKey(PlayerDecisionsKey + 0)) return;
            {
                for (int i = 0; i < 100; i++)
                {
                    if (PlayerPrefs.HasKey(PlayerDecisionsKey + i))
                    {
                        mainPlayer.playerDecisions.Add((Decision)Resources.Load(
                            DecisionsPath + PlayerPrefs.GetString(PlayerDecisionsKey + i),
                            typeof(Decision)));
                    }
                    else break;
                }
            }
        }

        /// <summary> Загрузить отношение к игроку </summary>
        public void LoadNonPlayerRatio(NonPlayer n)
        {
            if (PlayerPrefs.HasKey(n.name)) n.npToPlayerRatio = PlayerPrefs.GetInt(n.name);
        }

        #endregion

        #region SAVE_METHODS

        /// <summary> Сохранить последнюю главу </summary>
        private void SaveLastPart()
        {
            PlayerPrefs.SetString(LastPartKey, MainController.instance.animController.thisPart.name);
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить характеристики персонажа </summary>
        private void SaveCharacteristic()
        {
            PlayerPrefs.SetInt(PlayerHealthKey, mainPlayer.playerHealth);
            PlayerPrefs.SetInt(PlayerMindKey, mainPlayer.playerMind);
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить инвентарь персонажа </summary>
        private void SaveInventory()
        {
            if (mainPlayer.playerInventory != null)
            {
                // Сохранение
                for (int i = 0; i < mainPlayer.playerInventory.Count; i++)
                {
                    PlayerPrefs.SetString(PlayerInventoryKey + i, mainPlayer.playerInventory[i].name);
                }

                // Очистка от лишнего
                if (mainPlayer.playerInventory.Count > 0)
                {
                    for (int i = mainPlayer.playerInventory.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey(PlayerInventoryKey + i)) PlayerPrefs.DeleteKey(PlayerInventoryKey + i);
                        else break;
                    }
                }
            }
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить эффекты на персонаже </summary>
        private void SaveEffects()
        {
            if (mainPlayer.playerEffects != null)
            {
                // Сохранение
                for (int i = 0; i < mainPlayer.playerEffects.Count; i++)
                {
                    PlayerPrefs.SetString(PlayerEffectsKey + i, mainPlayer.playerEffects[i].name);
                }

                // Очистка от лишнего
                if (mainPlayer.playerEffects.Count > 0)
                {
                    for (int i = mainPlayer.playerEffects.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey(PlayerEffectsKey + i)) PlayerPrefs.DeleteKey(PlayerEffectsKey + i);
                        else break;
                    }
                }
            }
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить карту </summary>
        private void SaveMap()
        {
            if (mainPlayer.playerMap != null)
            {
                // Сохранение
                for (int i = 0; i < mainPlayer.playerMap.Count; i++)
                {
                    PlayerPrefs.SetString(PlayerLocationsKey + i, mainPlayer.playerMap[i].name);
                }

                // Очистка от лишнего
                if (mainPlayer.playerMap.Count > 0)
                {
                    for (int i = mainPlayer.playerMap.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey(PlayerLocationsKey + i)) PlayerPrefs.DeleteKey(PlayerLocationsKey + i);
                        else break;
                    }
                }
            }
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить заметки </summary>
        private void SaveNotes()
        {
            if (mainPlayer.playerDecisions != null)
            {
                // Сохранение
                for (int i = 0; i < mainPlayer.playerNotes.Count; i++)
                {
                    PlayerPrefs.SetString(PlayerNotesKey + i, mainPlayer.playerNotes[i].name);
                }

                // Очистка от лишнего
                if (mainPlayer.playerNotes.Count > 0)
                {
                    for (int i = mainPlayer.playerNotes.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey(PlayerNotesKey + i)) PlayerPrefs.DeleteKey(PlayerNotesKey + i);
                        else break;
                    }
                }
            }
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить решения </summary>
        private void SaveDecisons()
        {
            if (mainPlayer.playerNotes != null)
            {
                // Сохранение
                for (int i = 0; i < mainPlayer.playerDecisions.Count; i++)
                {
                    PlayerPrefs.SetString(PlayerDecisionsKey + i, mainPlayer.playerDecisions[i].name);
                }

                // Очистка от лишнего
                if (mainPlayer.playerDecisions.Count > 0)
                {
                    for (int i = mainPlayer.playerDecisions.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey(PlayerDecisionsKey + i)) PlayerPrefs.DeleteKey(PlayerDecisionsKey + i);
                        else break;
                    }
                }
            }
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить данные настроек </summary>
        public void SaveSettingsData()
        {
            if (mainSettings.isSoundCheck) PlayerPrefs.SetInt(SoundSettingsKey, 1);
            else PlayerPrefs.DeleteKey(SoundSettingsKey);

            if (mainSettings.isVibrationCheck) PlayerPrefs.SetInt(VibrationSettingsKey, 1);
            else PlayerPrefs.DeleteKey(VibrationSettingsKey);

            if (mainSettings.isEffectCheck) PlayerPrefs.SetInt(EffectsSettingsKey, 1);
            else PlayerPrefs.DeleteKey(EffectsSettingsKey);

            PlayerPrefs.Save();
        }

        /// <summary> Сохранить данные достижений </summary>
        public void SaveAchivesData()
        {
            if (mainSettings.gameAchivemants != null)
            {
                // Сохранение
                for (int i = 0; i < mainSettings.gameAchivemants.Count; i++)
                {
                    PlayerPrefs.SetString(AchivesKey + i, mainSettings.gameAchivemants[i].name);
                }

                // Очистка от лишнего
                if (mainSettings.gameAchivemants.Count > 0)
                {
                    for (int i = mainSettings.gameAchivemants.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey(AchivesKey + i)) PlayerPrefs.DeleteKey(AchivesKey + i);
                        else break;
                    }
                }
            }

            PlayerPrefs.Save();
        }

        /// <summary> Добавить НПС к измененным </summary>
        public void SaveNonPlayerRatio(NonPlayer n)
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
        private void SaveAllRatio()
        {
            for (int i = 0; i < npsSaveList.Length; i++)
            {
                if (npsSaveList[i] != null) PlayerPrefs.SetInt(npsSaveList[i].name, npsSaveList[i].npToPlayerRatio);
            }
        }

        /// <summary> Сохранение на контрольной точке </summary>
        public void CheckPointSave() => StartCoroutine(SaveWait());

        /// <summary> Задержка полного сохранения данных </summary>
        private IEnumerator SaveWait()
        {
            SaveLastPart();
            SaveCharacteristic();
            yield return new WaitForSeconds(1f);
            SaveInventory();
            SaveMap();
            yield return new WaitForSeconds(1f);
            SaveNotes();
            SaveEffects();
            yield return new WaitForSeconds(1f);
            SaveDecisons();
            SaveAllRatio();
        }

        #endregion

        /// <summary> Удаление данных и выход </summary>
        public void DellAllData(bool exit)
        {
            PlayerPrefs.DeleteAll();
            if (exit) Application.Quit();
        }
    }
}