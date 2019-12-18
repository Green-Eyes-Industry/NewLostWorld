﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CheckPoint : GameEvent
{
    /// <summary> Перезаписывает сохраненные данные </summary>
    public override bool EventStart()
    {
        DataController.gameSettingsData.lastPart = AnimController.thisPart;
        DataController.SaveEffects();
        DataController.SaveDecisons();
        DataController.SaveInventory();
        DataController.SaveMap();
        DataController.SaveNotes();
        DataController.SaveLastPart();
        DataController.SaveCharacteristic();
        DataController.SaveAllRatio();
        return true;
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(CheckPoint))]
    public class CheckPointGUI_Inspector : Editor
    {
        private CheckPoint _checkPoint;

        private void OnEnable() => _checkPoint = (CheckPoint)target;

        public override void OnInspectorGUI() => ShowEventEditor(_checkPoint);

        public static void ShowEventEditor(CheckPoint checkPoint)
        {
            GUILayout.Label("Контрольная точка");
        }
    }
}

#endif