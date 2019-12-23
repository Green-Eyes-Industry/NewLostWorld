using UnityEngine;
using System.Collections;
using NLW.Data;
using NLW.Parts;

namespace NLW
{
    /// <summary> Загрузка и сохранение данных </summary>
    public class DataController : MainController
    {
        public static NonPlayer[] npsSaveList;
        [HideInInspector] public Player mainPlayer;
        [HideInInspector] public GameSettings mainSettings;

        #region RESOURCES_PATH

        private readonly string _playerPath = "Players/MainPlayer";
        private readonly string _settingsPath = "MainSettings";

        private readonly string _partsPath = "GameParts/";
        private readonly string _achivesPath = "Achivemants/";
        private readonly string _inventoryPath = "GameItems/";
        private readonly string _effectsPath = "PlayerEffects/";
        private readonly string _locationsPath = "Locations/";
        private readonly string _notesPath = "Notes/";
        private readonly string _decisionsPath = "Decisions/";

        #endregion

        #region SAVE_KEYS

        private readonly string _achivesKey = "Achive_";
        private readonly string _lastPartKey = "LastPart";
        private readonly string _soundSettingsKey = "Sound";
        private readonly string _vibrationSettingsKey = "Vibration";
        private readonly string _effectsSettingsKey = "Effects";

        private readonly string _playerHealthKey = "Health";
        private readonly string _playerMindKey = "Mind";

        private readonly string _playerInventoryKey = "Invent_";
        private readonly string _playerEffectsKey = "Effect_";
        private readonly string _playerLocationsKey = "Location_";
        private readonly string _playerNotesKey = "Note_";
        private readonly string _playerDecisionsKey = "Decision_";

        #endregion

        protected override void Init() => LoadData();

        #region LOAD_METHODS

        /// <summary> Загрузить базовые данные </summary>
        private void LoadData()
        {
            npsSaveList = new NonPlayer[20];

            mainPlayer = (Player)Resources.Load(_playerPath, typeof(Player));
            mainSettings = (GameSettings)Resources.Load(_settingsPath, typeof(GameSettings));

            if (PlayerPrefs.HasKey(_lastPartKey))
            {
                // Глава на которой закончили
                if ((GamePart)Resources.Load(_partsPath + PlayerPrefs.GetString(_lastPartKey), typeof(GamePart)) != null)
                {
                    mainSettings.lastPart = (GamePart)Resources.Load(_partsPath + PlayerPrefs.GetString(_lastPartKey), typeof(GamePart));
                }
            }

            // Загрузка настроек

            if (PlayerPrefs.HasKey(_soundSettingsKey)) mainSettings.isSoundCheck = true;
            else mainSettings.isSoundCheck = false;

            if (PlayerPrefs.HasKey(_vibrationSettingsKey)) mainSettings.isVibrationCheck = true;
            else mainSettings.isVibrationCheck = false;

            if (PlayerPrefs.HasKey(_effectsSettingsKey)) mainSettings.isEffectCheck = true;
            else mainSettings.isEffectCheck = false;

            // Достижения
            if (PlayerPrefs.HasKey(_achivesKey + 0))
            {
                for (int i = 0; i < 100; i++)
                {
                    if (PlayerPrefs.HasKey(_achivesKey + i))
                    {
                        mainSettings.gameAchivemants.Add((Achivemants)Resources.Load(
                            _achivesPath + PlayerPrefs.GetString(_achivesKey + i),
                            typeof(Achivemants)));
                    }
                    else break;
                }
            }
        }

        /// <summary> Загрузка данных для игры </summary>
        public void LoadGameData()
        {
            // Характеристики игрока
            if (PlayerPrefs.HasKey(_playerHealthKey)) mainPlayer.playerHealth = PlayerPrefs.GetInt(_playerHealthKey);
            if (PlayerPrefs.HasKey(_playerMindKey)) mainPlayer.playerMind = PlayerPrefs.GetInt(_playerMindKey);

            // Инвентарь
            if (PlayerPrefs.HasKey(_playerInventoryKey + 0))
            {
                for (int i = 0; i < 100; i++)
                {
                    if (PlayerPrefs.HasKey(_playerInventoryKey + i))
                    {
                        mainPlayer.playerInventory.Add((GameItem)Resources.Load(
                            _inventoryPath + PlayerPrefs.GetString(_playerInventoryKey + i),
                            typeof(GameItem)));
                    }
                    else break;
                }
            }

            // Эффекты
            if (PlayerPrefs.HasKey(_playerEffectsKey + 0))
            {
                for (int i = 0; i < 100; i++)
                {
                    if (PlayerPrefs.HasKey(_playerEffectsKey + i))
                    {
                        mainPlayer.playerEffects.Add((GameEffect)Resources.Load(
                            _effectsPath + PlayerPrefs.GetString(_playerEffectsKey + i),
                            typeof(GameEffect)));
                    }
                    else break;
                }
            }

            // Карта
            if (PlayerPrefs.HasKey(_playerLocationsKey + 0))
            {
                for (int i = 0; i < 100; i++)
                {
                    if (PlayerPrefs.HasKey(_playerLocationsKey + i))
                    {
                        mainPlayer.playerMap.Add((MapMark)Resources.Load(
                            _locationsPath + PlayerPrefs.GetString(_playerLocationsKey + i),
                            typeof(MapMark)));
                    }
                    else break;
                }
            }

            // Заметки
            if (PlayerPrefs.HasKey(_playerNotesKey + 0))
            {
                for (int i = 0; i < 100; i++)
                {
                    if (PlayerPrefs.HasKey(_playerNotesKey + i))
                    {
                        mainPlayer.playerNotes.Add((Note)Resources.Load(
                            _notesPath + PlayerPrefs.GetString(_playerNotesKey + i),
                            typeof(Note)));
                    }
                    else break;
                }
            }

            // Решения
            if (PlayerPrefs.HasKey(_playerDecisionsKey + 0))
            {
                for (int i = 0; i < 100; i++)
                {
                    if (PlayerPrefs.HasKey(_playerDecisionsKey + i))
                    {
                        mainPlayer.playerDecisions.Add((Decision)Resources.Load(
                            _decisionsPath + PlayerPrefs.GetString(_playerDecisionsKey + i),
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
        public void SaveLastPart()
        {
            PlayerPrefs.SetString(_lastPartKey, animController.thisPart.name);
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить характеристики персонажа </summary>
        public void SaveCharacteristic()
        {
            PlayerPrefs.SetInt(_playerHealthKey, mainPlayer.playerHealth);
            PlayerPrefs.SetInt(_playerMindKey, mainPlayer.playerMind);
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить инвентарь персонажа </summary>
        public void SaveInventory()
        {
            if (mainPlayer.playerInventory != null)
            {
                // Сохранение
                for (int i = 0; i < mainPlayer.playerInventory.Count; i++)
                {
                    PlayerPrefs.SetString(_playerInventoryKey + i, mainPlayer.playerInventory[i].name);
                }

                // Очистка от лишнего
                if (mainPlayer.playerInventory.Count > 0)
                {
                    for (int i = mainPlayer.playerInventory.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey(_playerInventoryKey + i)) PlayerPrefs.DeleteKey(_playerInventoryKey + i);
                        else break;
                    }
                }
            }
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить эффекты на персонаже </summary>
        public void SaveEffects()
        {
            if (mainPlayer.playerEffects != null)
            {
                // Сохранение
                for (int i = 0; i < mainPlayer.playerEffects.Count; i++)
                {
                    PlayerPrefs.SetString(_playerEffectsKey + i, mainPlayer.playerEffects[i].name);
                }

                // Очистка от лишнего
                if (mainPlayer.playerEffects.Count > 0)
                {
                    for (int i = mainPlayer.playerEffects.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey(_playerEffectsKey + i)) PlayerPrefs.DeleteKey(_playerEffectsKey + i);
                        else break;
                    }
                }
            }
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить карту </summary>
        public void SaveMap()
        {
            if (mainPlayer.playerMap != null)
            {
                // Сохранение
                for (int i = 0; i < mainPlayer.playerMap.Count; i++)
                {
                    PlayerPrefs.SetString(_playerLocationsKey + i, mainPlayer.playerMap[i].name);
                }

                // Очистка от лишнего
                if (mainPlayer.playerMap.Count > 0)
                {
                    for (int i = mainPlayer.playerMap.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey(_playerLocationsKey + i)) PlayerPrefs.DeleteKey(_playerLocationsKey + i);
                        else break;
                    }
                }
            }
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить заметки </summary>
        public void SaveNotes()
        {
            if (mainPlayer.playerDecisions != null)
            {
                // Сохранение
                for (int i = 0; i < mainPlayer.playerNotes.Count; i++)
                {
                    PlayerPrefs.SetString(_playerNotesKey + i, mainPlayer.playerNotes[i].name);
                }

                // Очистка от лишнего
                if (mainPlayer.playerNotes.Count > 0)
                {
                    for (int i = mainPlayer.playerNotes.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey(_playerNotesKey + i)) PlayerPrefs.DeleteKey(_playerNotesKey + i);
                        else break;
                    }
                }
            }
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить решения </summary>
        public void SaveDecisons()
        {
            if (mainPlayer.playerNotes != null)
            {
                // Сохранение
                for (int i = 0; i < mainPlayer.playerDecisions.Count; i++)
                {
                    PlayerPrefs.SetString(_playerDecisionsKey + i, mainPlayer.playerDecisions[i].name);
                }

                // Очистка от лишнего
                if (mainPlayer.playerDecisions.Count > 0)
                {
                    for (int i = mainPlayer.playerDecisions.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey(_playerDecisionsKey + i)) PlayerPrefs.DeleteKey(_playerDecisionsKey + i);
                        else break;
                    }
                }
            }
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить данные настроек </summary>
        public void SaveSettingsData()
        {
            if (mainSettings.isSoundCheck) PlayerPrefs.SetInt(_soundSettingsKey, 1);
            else PlayerPrefs.DeleteKey(_soundSettingsKey);

            if (mainSettings.isVibrationCheck) PlayerPrefs.SetInt(_vibrationSettingsKey, 1);
            else PlayerPrefs.DeleteKey(_vibrationSettingsKey);

            if (mainSettings.isEffectCheck) PlayerPrefs.SetInt(_effectsSettingsKey, 1);
            else PlayerPrefs.DeleteKey(_effectsSettingsKey);

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
                    PlayerPrefs.SetString(_achivesKey + i, mainSettings.gameAchivemants[i].name);
                }

                // Очистка от лишнего
                if (mainSettings.gameAchivemants.Count > 0)
                {
                    for (int i = mainSettings.gameAchivemants.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey(_achivesKey + i)) PlayerPrefs.DeleteKey(_achivesKey + i);
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
        public void SaveAllRatio()
        {
            for (int i = 0; i < npsSaveList.Length; i++)
            {
                if (npsSaveList[i] != null) PlayerPrefs.SetInt(npsSaveList[i].name, npsSaveList[i].npToPlayerRatio);
            }
        }

        /// <summary> Сохранение на чек-поинте </summary>
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