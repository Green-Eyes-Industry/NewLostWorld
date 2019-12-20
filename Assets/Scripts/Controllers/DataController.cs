using UnityEngine;
using System.Collections;
using NLW.Data;

namespace NLW
{
    /// <summary> Загрузка и сохранение данных </summary>
    public class DataController : MainController
    {
        public static NonPlayer[] npsSaveList;

        #region LOAD_METHODS

        protected override void Init()
        {
            LoadData();
        }

        /// <summary> Загрузить базовые данные </summary>
        private void LoadData()
        {
            npsSaveList = new NonPlayer[20];

            if (PlayerPrefs.HasKey("LastPart"))
            {
                // Глава на которой закончили
                if ((Parts.GamePart)Resources.Load("GameParts/" + PlayerPrefs.GetString("LastPart"), typeof(Parts.GamePart)) != null)
                {
                    Instance.mainSettings.lastPart = (Parts.GamePart)Resources.Load("GameParts/" + PlayerPrefs.GetString("LastPart"), typeof(Parts.GamePart));
                }
            }

            // Загрузка настроек

            if (PlayerPrefs.HasKey("Sound")) Instance.mainSettings.isSoundCheck = true;
            else Instance.mainSettings.isSoundCheck = false;

            if (PlayerPrefs.HasKey("Vibration")) Instance.mainSettings.isVibrationCheck = true;
            else Instance.mainSettings.isVibrationCheck = false;

            if (PlayerPrefs.HasKey("Effects")) Instance.mainSettings.isEffectCheck = true;
            else Instance.mainSettings.isEffectCheck = false;

            // Достижения
            if (PlayerPrefs.HasKey("Achive_0"))
            {
                for (int i = 0; i < 100; i++)
                {
                    if (PlayerPrefs.HasKey("Achive_" + i))
                    {
                        Instance.mainSettings.gameAchivemants.Add((Achivemants)Resources.Load(
                            "Achivemants/" + PlayerPrefs.GetString("Achive_" + i),
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
            if (PlayerPrefs.HasKey("Health")) Instance.mainPlayer.playerHealth = PlayerPrefs.GetInt("Health");
            if (PlayerPrefs.HasKey("Mind")) Instance.mainPlayer.playerMind = PlayerPrefs.GetInt("Mind");

            // Инвентарь
            if (PlayerPrefs.HasKey("Invent_0"))
            {
                for (int i = 0; i < 100; i++)
                {
                    if (PlayerPrefs.HasKey("Invent_" + i))
                    {
                        Instance.mainPlayer.playerInventory.Add((GameItem)Resources.Load(
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
                        Instance.mainPlayer.playerEffects.Add((GameEffect)Resources.Load(
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
                        Instance.mainPlayer.playerMap.Add((MapMark)Resources.Load(
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
                        Instance.mainPlayer.playerNotes.Add((Note)Resources.Load(
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
                        Instance.mainPlayer.playerDecisions.Add((Decision)Resources.Load(
                            "Decisions/" + PlayerPrefs.GetString("Decision_" + i),
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
            PlayerPrefs.SetString("LastPart", Instance.animController.thisPart.name);
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить характеристики персонажа </summary>
        public void SaveCharacteristic()
        {
            PlayerPrefs.SetInt("Health", Instance.mainPlayer.playerHealth);
            PlayerPrefs.SetInt("Mind", Instance.mainPlayer.playerMind);
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить инвентарь персонажа </summary>
        public void SaveInventory()
        {
            if (Instance.mainPlayer.playerInventory != null)
            {
                // Сохранение
                for (int i = 0; i < Instance.mainPlayer.playerInventory.Count; i++)
                {
                    PlayerPrefs.SetString("Invent_" + i, Instance.mainPlayer.playerInventory[i].name);
                }

                // Очистка от лишнего
                if (Instance.mainPlayer.playerInventory.Count > 0)
                {
                    for (int i = Instance.mainPlayer.playerInventory.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey("Invent_" + i)) PlayerPrefs.DeleteKey("Invent_" + i);
                        else break;
                    }
                }
            }
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить эффекты на персонаже </summary>
        public void SaveEffects()
        {
            if (Instance.mainPlayer.playerEffects != null)
            {
                // Сохранение
                for (int i = 0; i < Instance.mainPlayer.playerEffects.Count; i++)
                {
                    PlayerPrefs.SetString("Effect_" + i, Instance.mainPlayer.playerEffects[i].name);
                }

                // Очистка от лишнего
                if (Instance.mainPlayer.playerEffects.Count > 0)
                {
                    for (int i = Instance.mainPlayer.playerEffects.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey("Effect_" + i)) PlayerPrefs.DeleteKey("Effect_" + i);
                        else break;
                    }
                }
            }
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить карту </summary>
        public void SaveMap()
        {
            if (Instance.mainPlayer.playerMap != null)
            {
                // Сохранение
                for (int i = 0; i < Instance.mainPlayer.playerMap.Count; i++)
                {
                    PlayerPrefs.SetString("Location_" + i, Instance.mainPlayer.playerMap[i].name);
                }

                // Очистка от лишнего
                if (Instance.mainPlayer.playerMap.Count > 0)
                {
                    for (int i = Instance.mainPlayer.playerMap.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey("Location_" + i)) PlayerPrefs.DeleteKey("Location_" + i);
                        else break;
                    }
                }
            }
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить заметки </summary>
        public void SaveNotes()
        {
            if (Instance.mainPlayer.playerDecisions != null)
            {
                // Сохранение
                for (int i = 0; i < Instance.mainPlayer.playerDecisions.Count; i++)
                {
                    PlayerPrefs.SetString("Decision_" + i, Instance.mainPlayer.playerDecisions[i].name);
                }

                // Очистка от лишнего
                if (Instance.mainPlayer.playerDecisions.Count > 0)
                {
                    for (int i = Instance.mainPlayer.playerDecisions.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey("Decision_" + i)) PlayerPrefs.DeleteKey("Decision_" + i);
                        else break;
                    }
                }
            }
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить решения </summary>
        public void SaveDecisons()
        {
            if (Instance.mainPlayer.playerNotes != null)
            {
                // Сохранение
                for (int i = 0; i < Instance.mainPlayer.playerNotes.Count; i++)
                {
                    PlayerPrefs.SetString("Note_" + i, Instance.mainPlayer.playerNotes[i].name);
                }

                // Очистка от лишнего
                if (Instance.mainPlayer.playerNotes.Count > 0)
                {
                    for (int i = Instance.mainPlayer.playerNotes.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey("Note_" + i)) PlayerPrefs.DeleteKey("Note_" + i);
                        else break;
                    }
                }
            }
            PlayerPrefs.Save();
        }

        /// <summary> Сохранить данные настроек </summary>
        public void SaveSettingsData()
        {
            if (Instance.mainSettings.isSoundCheck) PlayerPrefs.SetInt("Sound", 1);
            else PlayerPrefs.DeleteKey("Sound");

            if (Instance.mainSettings.isVibrationCheck) PlayerPrefs.SetInt("Vibration", 1);
            else PlayerPrefs.DeleteKey("Vibration");

            if (Instance.mainSettings.isEffectCheck) PlayerPrefs.SetInt("Effects", 1);
            else PlayerPrefs.DeleteKey("Effects");

            PlayerPrefs.Save();
        }

        /// <summary> Сохранить данные достижений </summary>
        public void SaveAchivesData()
        {
            if (Instance.mainSettings.gameAchivemants != null)
            {
                // Сохранение
                for (int i = 0; i < Instance.mainSettings.gameAchivemants.Count; i++)
                {
                    PlayerPrefs.SetString("Achive_" + i, Instance.mainSettings.gameAchivemants[i].name);
                }

                // Очистка от лишнего
                if (Instance.mainSettings.gameAchivemants.Count > 0)
                {
                    for (int i = Instance.mainSettings.gameAchivemants.Count - 1; i < 50; i++)
                    {
                        if (PlayerPrefs.HasKey("Achive_" + i)) PlayerPrefs.DeleteKey("Achive_" + i);
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