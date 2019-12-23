using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Data;
#endif

namespace NLW.Data
{
    public class ImportantDecision : GameEvent
    {
        /// <summary> Решение </summary>
        public Decision decision;

        /// <summary> Принять решение </summary>
        public override bool EventStart()
        {
            Player mPlayer = MainController.instance.dataController.mainPlayer;

            if (!mPlayer.playerDecisions.Contains(decision)) mPlayer.playerDecisions.Add(decision);
            return true;
        }
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(ImportantDecision))]
    public class ImportantDecisionGInspector : Editor
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

            for (int i = 0; i < names.Length; i++)
            {
                Decision nameConvert = (Decision)allDecisions[i];

                if (nameConvert.nameDecision == "") names[i] = nameConvert.name;
                else names[i] = nameConvert.nameDecision;
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
            if (importantDecision.decision != null) DecisionGInspector.ShowItemEditor(importantDecision.decision);

            EditorGUILayout.EndVertical();
        }
    }
}

#endif