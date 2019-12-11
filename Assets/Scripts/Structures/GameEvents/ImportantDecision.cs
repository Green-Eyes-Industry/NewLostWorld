using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Важное решение")]
public class ImportantDecision : GameEvent
{
    /// <summary> Решение </summary>
    public Decision decision;

    /// <summary> Принять решение </summary>
    public override bool EventStart()
    {
        if (DataController.playerData.playerDecisions.Contains(decision))
        {
            DataController.playerData.playerDecisions.Add(decision);
            DataController.SaveDecisons();
        }
        return true;
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(ImportantDecision))]
    public class ImportantDecisionGUI_Inspector : Editor
    {
        private ImportantDecision _importantDecision;

        private void OnEnable() => _importantDecision = (ImportantDecision)target;

        public override void OnInspectorGUI() => ShowEventEditor(_importantDecision);

        public static void ShowEventEditor(ImportantDecision importantDecision)
        {
            GUILayout.Label("Принять важное решение");

            EditorGUILayout.BeginVertical("Box");

            // Код

            EditorGUILayout.EndVertical();
        }
    }
}

#endif