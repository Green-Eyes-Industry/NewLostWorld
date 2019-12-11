using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Контрольная точка")]
public class CheckPoint : GameEvent
{
    /// <summary> Перезаписывает сохраненные данные </summary>
    public override bool EventStart()
    {
        DataController.gameSettingsData.lastPart = MoveController.thisPart;
        DataController.SaveLastPart();

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

            EditorGUILayout.BeginVertical("Box");

            // Код

            EditorGUILayout.EndVertical();
        }
    }
}

#endif