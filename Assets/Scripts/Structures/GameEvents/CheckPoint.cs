﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Data;
#endif

namespace NLW.Data
{
    public class CheckPoint : GameEvent
    {
        /// <summary> Перезаписывает сохраненные данные </summary>
        public override bool EventStart()
        {
            DataController dController = MainController.instance.dataController;
            MainController.instance.dataController.mainSettings.lastPart = MainController.instance.animController.thisPart;
            dController.CheckPointSave();
            return true;
        }
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(CheckPoint))]
    public class CheckPointGInspector : Editor
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