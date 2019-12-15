using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New game settings", menuName = "Игровые обьекты/Игровые параметры")]
public class GameSettings : ScriptableObject
{
    /// <summary> Последняя глава </summary>
    public GamePart lastPart;

    /// <summary> Настройка звука </summary>
    public bool isSoundCheck;

    /// <summary> Настройка вибрации </summary>
    public bool isVibrationCheck;

    /// <summary> Настройка еффектов </summary>
    public bool isEffectCheck;

    /// <summary> Цвет глаза </summary>
    public Color eyeColor;

    /// <summary> Полученные достижения </summary>
    public List<Achivemants> gameAchivemants;
}

#if UNITY_EDITOR

namespace GUIInspector
{

    [CustomEditor(typeof(GameSettings))]
    public class GameSettingsGUI_Inspector : Editor
    {
        private GameSettings _gameSettings;
        private List<Achivemants> _achivemants;

        private string[] _newAchiveNames;
        private Achivemants[] _newAchiveFiles;

        private int _newAchiveIndex = 0;

        private void OnEnable()
        {
            _gameSettings = (GameSettings)target;
            _achivemants = new List<Achivemants>();
            ReloadAchives();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Игровые параметры");

            GUILayout.BeginVertical("Box");

            _gameSettings.eyeColor = EditorGUILayout.ColorField("Цвет глаза :", _gameSettings.eyeColor);

            EditorGUILayout.Space();

            _gameSettings.isSoundCheck = EditorGUILayout.Toggle("Звук : ", _gameSettings.isSoundCheck);
            _gameSettings.isVibrationCheck = EditorGUILayout.Toggle("Вибрация : ", _gameSettings.isVibrationCheck);
            _gameSettings.isEffectCheck = EditorGUILayout.Toggle("Эффекты : ", _gameSettings.isEffectCheck);

            EditorGUILayout.Space();

            if (_gameSettings.lastPart == null) GUI.backgroundColor = Color.red;
            _gameSettings.lastPart = (GamePart)EditorGUILayout.ObjectField(_gameSettings.lastPart, typeof(GamePart), true);
            GUI.backgroundColor = Color.white;

            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            GUILayout.Label("Достижения [Получено " + _gameSettings.gameAchivemants.Count + " достижений]");
            EditorGUILayout.Space();

            for (int i = 0; i < _gameSettings.gameAchivemants.Count; i++)
            {
                EditorGUILayout.BeginHorizontal("Box");
                EditorGUILayout.LabelField(_gameSettings.gameAchivemants[i].achiveName);
                if (GUILayout.Button("Удалить"))
                {
                    _gameSettings.gameAchivemants.RemoveAt(i);
                    ReloadAchives();
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (_newAchiveNames.Length > 0)
            {
                _newAchiveIndex = EditorGUILayout.Popup(_newAchiveIndex, _newAchiveNames);
                if (GUILayout.Button("Добавить достижение"))
                {
                    _gameSettings.gameAchivemants.Add(_newAchiveFiles[_newAchiveIndex]);
                    ReloadAchives();
                }
            }
            else GUILayout.Label("Нет доступных для получения достижений");

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            if (GUILayout.Button("Сохранить настройки", GUILayout.Height(20))) EditorUtility.SetDirty(_gameSettings);
        }

        /// <summary> Перезагрузить список достижений </summary>
        private void ReloadAchives()
        {
            Object[] obj = Resources.LoadAll("Achivemants", typeof(Achivemants));

            List<string> _newAchiveList = new List<string>();
            List<Achivemants> _newAchiveFileList = new List<Achivemants>();

            for (int i = 0; i < obj.Length; i++)
            {
                _achivemants.Add((Achivemants)obj[i]);
            }

            for (int i = 0; i < _achivemants.Count; i++)
            {
                if (!_gameSettings.gameAchivemants.Contains(_achivemants[i]))
                {
                    _newAchiveList.Add(_achivemants[i].achiveName);
                    _newAchiveFileList.Add(_achivemants[i]);
                }
            }

            _newAchiveNames = _newAchiveList.ToArray();
            _newAchiveFiles = _newAchiveFileList.ToArray();
        }
    }
}

#endif