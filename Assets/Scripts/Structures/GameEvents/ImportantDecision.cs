using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
        public static int id = 0;

        private void OnEnable() => _importantDecision = (ImportantDecision)target;

        public override void OnInspectorGUI() => ShowEventEditor(_importantDecision);

        public static void ShowEventEditor(ImportantDecision importantDecision)
        {
            GUILayout.Label("Принять важное решение");

            EditorGUILayout.BeginVertical("Box");

            object[] allDecisions = Resources.LoadAll("Decisions/", typeof(Decision));

            string[] names = new string[allDecisions.Length];

            Decision nameConvert;

            for (int i = 0; i < names.Length; i++)
            {
                nameConvert = (Decision)allDecisions[i];

                if (nameConvert._nameDecision == "") names[i] = nameConvert.name;
                else names[i] = nameConvert._nameDecision;
            }

            EditorGUILayout.BeginHorizontal();

            if (allDecisions.Length > 0)
            {
                id = EditorGUILayout.Popup(id, names);
                importantDecision.decision = (Decision)allDecisions[id];
            }
            else GUILayout.Label("Нет решений");

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Создать", GUILayout.Width(70))) AssetDatabase.CreateAsset(CreateInstance(typeof(Decision)),
                  "Assets/Resources/Decisions/" + allDecisions.Length + "_Decision.asset");

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
            if (importantDecision.decision != null) DecisionGUI_Inspector.ShowItemEditor(importantDecision.decision);

            EditorGUILayout.EndVertical();
        }
    }
}

#endif