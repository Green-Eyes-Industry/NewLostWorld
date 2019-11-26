using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsGUI_Inspector : Editor
{
    private GameSettings _gameSettings;
    private Color _eyeColor;

    private void OnEnable() => _gameSettings = (GameSettings)target;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Игровые параметры");

        GUILayout.BeginVertical("Box");

        _eyeColor = EditorGUILayout.ColorField("Цвет глаза :", _eyeColor);
        _gameSettings.eyeColor = _eyeColor.b;

        EditorGUILayout.Space();

        _gameSettings.soundCheck = EditorGUILayout.Toggle("Настройка звука : ",_gameSettings.soundCheck);
        _gameSettings.vibrationCheck = EditorGUILayout.Toggle("Настройка звука : ", _gameSettings.vibrationCheck);
        _gameSettings.effectCheck = EditorGUILayout.Toggle("Настройка звука : ", _gameSettings.effectCheck);

        EditorGUILayout.Space();

        _gameSettings.lastPart = (GamePart)EditorGUILayout.ObjectField(_gameSettings.lastPart, typeof(GamePart), true);
        if (_gameSettings.lastPart == null) Resources.Load("GameParts/000_Start");

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Сохранить настройки", GUILayout.Height(30))) EditorUtility.SetDirty(_gameSettings);
    }
}