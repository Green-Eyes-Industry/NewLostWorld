using UnityEditor;
using UnityEngine;

namespace Data.GameEvents
{
    public class CheckPoint : GameEvent
    {
        /// <summary> Перезаписывает сохраненные данные </summary>
        public override bool EventStart()
        {
            MainController.instance.dataController.mainSettings.lastPart = MainController.instance.animController.thisPart;
            MainController.instance.animController.SaveGameMessange();
            MainController.instance.dataController.SaveGameSettings();
            MainController.instance.dataController.SaveGlobalSettings();
            return true;
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(CheckPoint))]
    public class CheckPointGUInspector : Editor
    {
        private CheckPoint _checkPoint;

        private void OnEnable() => _checkPoint = (CheckPoint)target;

        public override void OnInspectorGUI() => ShowEventEditor(_checkPoint);

        public static void ShowEventEditor(CheckPoint checkPoint)
        {
            GUILayout.Label("Контрольная точка");
        }
    }

#endif
}