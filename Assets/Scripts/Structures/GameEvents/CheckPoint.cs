using UnityEngine;

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
            DataController DController = MainController.Instance.dataController;
            MainController.Instance.dataController.mainSettings.lastPart = MainController.Instance.animController.thisPart;
            DController.CheckPointSave();
            return true;
        }
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