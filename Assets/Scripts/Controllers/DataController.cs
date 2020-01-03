using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Data;
using Data.Characters;
using Helpers;
using UnityEngine;

namespace Controllers
{
    /// <summary> Загрузка и сохранение данных </summary>
    public class DataController : ParentController
    {
        #region VARIABLES

        [HideInInspector] public static NonPlayer[] npsSaveList;
        [HideInInspector] public static GameEffect[] effectsSaveList;

        public Player mainPlayer;
        public GameSettings mainSettings;

        private const string _trueVal = "true";
        private const string _root = "data";

        private string _savePathFolder;
        private string _globalFileName;
        private string _gameFileName;

        #endregion

        #region GLOBAL_KEYS

        private string _pathGameParts;
        private string _pathAchivemants;

        private string _eyeColor;
        private string _eyeColorR;
        private string _eyeColorG;
        private string _eyeColorB;

        private string _lastPart;

        private string _settings;
        private string _settingsSound;
        private string _settingsVibration;
        private string _settingsEffects;

        private string _previewText;

        private string _achives;
        private string _achivesData;

        #endregion

        #region GAME_KEYS

        private string _pathItems;
        private string _pathEffects;
        private string _pathDecisions;
        private string _pathNotes;
        private string _pathLocations;
        private string _pathNonPlayers;

        private string _playerData;
        private string _playerDataHealth;
        private string _playerDataMind;

        private string _inventory;
        private string _inventoryData;

        private string _effects;
        private string _effectsData;
        private string _effectsDataDuration;

        private string _decision;
        private string _decisionData;

        private string _notes;
        private string _notesData;

        private string _locations;
        private string _locationsData;

        private string _meet;
        private string _meetData;
        private string _meetDataRation;

        #endregion

        /// <summary> Присвоить имена базовым ключам </summary>
        private void KeyGlobalNames(bool isClear)
        {
            if (isClear)
            {
                _globalFileName = null;
                _pathGameParts = null;
                _pathAchivemants = null;
                _eyeColor = null;
                _eyeColorR = null;
                _eyeColorG = null;
                _eyeColorB = null;
                _lastPart = null;
                _settings = null;
                _settingsSound = null;
                _settingsVibration = null;
                _settingsEffects = null;
                _previewText = null;
                _achives = null;
                _achivesData = null;
                return;
            }

            _globalFileName = "/GlobalSettings.xml";

            _pathGameParts = "GameParts/";
            _pathAchivemants = "Achivemants/";

            _eyeColor = "eye_color";
            _eyeColorR = "r";
            _eyeColorG = "g";
            _eyeColorB = "b";

            _lastPart = "last_part";

            _settings = "settings";
            _settingsSound = "sound";
            _settingsVibration = "vibration";
            _settingsEffects = "effects";

            _previewText = "preview_text";

            _achives = "achives";
            _achivesData = "ach";
        }

        /// <summary> Присвоить имена игровым ключам </summary>
        private void KeyGameNames(bool isClear)
        {
            if (isClear)
            {
                _gameFileName = null;
                _pathItems = null;
                _pathDecisions = null;
                _pathNotes = null;
                _pathLocations = null;
                _pathNonPlayers = null;
                _playerData = null;
                _playerDataHealth = null;
                _playerDataMind = null;
                _inventory = null;
                _inventoryData = null;
                _effects = null;
                _effectsData = null;
                _effectsDataDuration = null;
                _decision = null;
                _decisionData = null;
                _notes = null;
                _notesData = null;
                _locations = null;
                _locationsData = null;
                _meet = null;
                _meetData = null;
                _meetDataRation = null;
            }

            _gameFileName = "/GameSettings.xml";

            _pathItems = "GameItems/";
            _pathEffects = "PlayerEffects/";
            _pathDecisions = "Decisions/";
            _pathNotes = "Notes/";
            _pathLocations = "Locations/";
            _pathNonPlayers = "Players/NonPlayers/";

            _playerData = "player_data";
            _playerDataHealth = "health";
            _playerDataMind = "mind";

            _inventory = "inventory";
            _inventoryData = "item";

            _effects = "effects";
            _effectsData = "effect";
            _effectsDataDuration = "duration";

            _decision = "decision";
            _decisionData = "dec";

            _notes = "notes";
            _notesData = "note";

            _locations = "locations";
            _locationsData = "location";

            _meet = "meet";
            _meetData = "meetData";
            _meetDataRation = "ratio";
        }

        public override void Init()
        {
            _savePathFolder = Application.persistentDataPath;

#if UNITY_EDITOR
            _savePathFolder = Application.dataPath + "/Editor/Reserve";
#endif

            LoadGlobalSettings();
        }

        #region SAVE

        public void SaveGlobalSettings()
        {
            // Присвоение значений

            KeyGlobalNames(false); // Загрузка ключей

            XElement el_eyeColor = new XElement(_eyeColor,
                new XAttribute(_eyeColorR, mainSettings.eyeColor.r),
                new XAttribute(_eyeColorG, mainSettings.eyeColor.g),
                new XAttribute(_eyeColorB, mainSettings.eyeColor.b));

            XElement el_lastPart = new XElement(_lastPart,
                mainSettings.lastPart.name);

            XElement settings = new XElement(_settings,
                new XAttribute(_settingsSound, mainSettings.isSoundCheck),
                new XAttribute(_settingsVibration, mainSettings.isVibrationCheck),
                new XAttribute(_settingsEffects, mainSettings.isEffectCheck));

            XElement previewText = new XElement(_previewText, mainSettings.previewText);

            XElement achives = new XElement(_achives);

            foreach (Achivemants achive in mainSettings.gameAchivemants) achives.Add(new XElement(_achivesData, achive.name));

            // Сохранение значений

            XElement global_data = new XElement(_root);

            global_data.Add(el_eyeColor);
            global_data.Add(el_lastPart);
            global_data.Add(settings);
            global_data.Add(previewText);
            global_data.Add(achives);
            
            XDocument save_global_data = new XDocument(global_data);
            File.WriteAllText(_savePathFolder + _globalFileName, save_global_data.ToString());

            KeyGlobalNames(true); // Отгрузка ключей
        }

        /// <summary> Сохранить игровые настройки </summary>
        public void SaveGameSettings()
        {
            KeyGameNames(false); // Загрузка значений

            XElement el_player = new XElement(_playerData,
                new XAttribute(_playerDataHealth, mainPlayer.playerHealth),
                new XAttribute(_playerDataMind, mainPlayer.playerMind));

            XElement el_inventory = new XElement(_inventory);
            XElement el_playerEffect = new XElement(_effects);
            XElement el_playerDecisions = new XElement(_decision);
            XElement el_playerNotes = new XElement(_notes);
            XElement el_playerLocations = new XElement(_locations);
            XElement el_meet = new XElement(_meet);

            foreach (GameItem item in mainPlayer.playerInventory) el_inventory.Add(new XElement(_inventoryData, item.name));
            foreach (GameEffect effect in mainPlayer.playerEffects) el_playerEffect.Add(new XElement(_effectsData, effect.name,
                new XAttribute(_effectsDataDuration, effect.durationEffect)));
            foreach (Decision dec in mainPlayer.playerDecisions) el_playerDecisions.Add(new XElement(_decisionData, dec.name));
            foreach (Note note in mainPlayer.playerNotes) el_playerNotes.Add(new XElement(_notesData, note.name));
            foreach (MapMark mark in mainPlayer.playerMap) el_playerLocations.Add(new XElement(_locationsData, mark.name));
            foreach (NonPlayer nPlayer in mainPlayer.playerMeet) el_meet.Add(new XElement(_meetData, nPlayer.name,
                new XAttribute(_meetDataRation, nPlayer.npToPlayerRatio)));

            // Сохранение значений

            XElement global_data = new XElement(_root);

            global_data.Add(el_player);
            global_data.Add(el_inventory);
            global_data.Add(el_playerEffect);
            global_data.Add(el_playerDecisions);
            global_data.Add(el_playerNotes);
            global_data.Add(el_playerLocations);
            global_data.Add(el_meet);

            XDocument save_global_data = new XDocument(global_data);
            File.WriteAllText(_savePathFolder + _gameFileName, save_global_data.ToString());

            KeyGameNames(true); // Отгрузка значений
        }

        #endregion

        #region LOAD

        /// <summary> Загрузить глобальные настройки </summary>
        private void LoadGlobalSettings()
        {
            KeyGlobalNames(false); // Загрузка ключей

            if (File.Exists(_savePathFolder + _globalFileName))
            {
                XElement global_data = XDocument.Parse(File.ReadAllText(_savePathFolder + _globalFileName)).Element(_root);

                if (global_data == null) return;

                mainSettings.eyeColor.r = float.Parse(global_data.Element(_eyeColor).Attribute(_eyeColorR).Value);
                mainSettings.eyeColor.g = float.Parse(global_data.Element(_eyeColor).Attribute(_eyeColorG).Value);
                mainSettings.eyeColor.b = float.Parse(global_data.Element(_eyeColor).Attribute(_eyeColorB).Value);

                mainSettings.lastPart = Resources.Load<GamePart>(_pathGameParts + global_data.Element(_lastPart).Value);

                mainSettings.isSoundCheck = (global_data.Element(_settings).Attribute(_settingsSound).Value.Equals(_trueVal)) ? true : false;
                mainSettings.isVibrationCheck = (global_data.Element(_settings).Attribute(_settingsVibration).Value.Equals(_trueVal)) ? true : false;
                mainSettings.isEffectCheck = (global_data.Element(_settings).Attribute(_settingsEffects).Value.Equals(_trueVal)) ? true : false;

                mainSettings.previewText = global_data.Element(_previewText).Value;

                mainSettings.gameAchivemants = new List<Achivemants>();

                foreach (XElement element in global_data.Element(_achives).Elements(_achivesData))
                {
                    mainSettings.gameAchivemants.Add(Resources.Load<Achivemants>(_pathAchivemants + element.Value));
                }
            }

            KeyGlobalNames(true); // Отгрузка ключей
        }

        public void LoadGameSettings()
        {
            KeyGameNames(false); // Загрузка ключей

            if (File.Exists(_savePathFolder + _gameFileName))
            {
                XElement global_data = XDocument.Parse(File.ReadAllText(_savePathFolder + _gameFileName)).Element(_root);

                if (global_data == null) return;

                mainPlayer.playerHealth = int.Parse(global_data.Element(_playerData).Attribute(_playerDataHealth).Value);
                mainPlayer.playerMind = int.Parse(global_data.Element(_playerData).Attribute(_playerDataMind).Value);

                mainPlayer.playerInventory = new List<GameItem>();

                foreach (XElement element in global_data.Element(_inventory).Elements(_inventoryData))
                {
                    mainPlayer.playerInventory.Add(Resources.Load<GameItem>(_pathItems + element.Value));
                }

                GameEffect nEffect;
                mainPlayer.playerEffects = new List<GameEffect>();

                foreach (XElement element in global_data.Element(_effects).Elements(_effectsData))
                {
                    nEffect = Resources.Load<GameEffect>(_pathEffects + element.Value);
                    nEffect.durationEffect = int.Parse(element.Attribute(_effectsDataDuration).Value);

                    mainPlayer.playerEffects.Add(nEffect);
                }

                nEffect = null;

                mainPlayer.playerDecisions = new List<Decision>();

                foreach (XElement element in global_data.Element(_decision).Elements(_decisionData))
                {
                    mainPlayer.playerDecisions.Add(Resources.Load<Decision>(_pathDecisions + element.Value));
                }

                mainPlayer.playerNotes = new List<Note>();

                foreach (XElement element in global_data.Element(_notes).Elements(_notesData))
                {
                    mainPlayer.playerNotes.Add(Resources.Load<Note>(_pathNotes + element.Value));
                }

                mainPlayer.playerMap = new List<MapMark>();

                foreach (XElement element in global_data.Element(_locations).Elements(_locationsData))
                {
                    mainPlayer.playerMap.Add(Resources.Load<MapMark>(_pathLocations + element.Value));
                }

                NonPlayer nPlayer;
                mainPlayer.playerMeet = new List<NonPlayer>();

                foreach (XElement element in global_data.Element(_meet).Elements(_meetData))
                {
                    nPlayer = Resources.Load<NonPlayer>(_pathNonPlayers + element.Value);
                    nPlayer.npToPlayerRatio = int.Parse(element.Attribute(_meetDataRation).Value);

                    mainPlayer.playerMeet.Add(nPlayer);
                }

                nPlayer = null;
            }

            KeyGameNames(true); // Отгрузка ключей
        }

        #endregion
    }
}