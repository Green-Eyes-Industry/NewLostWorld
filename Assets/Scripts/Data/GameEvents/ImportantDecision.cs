using Data.Characters;
using Helpers;
using UnityEditor;
using UnityEngine;

namespace Data.GameEvents
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

        

#if UNITY_EDITOR
        public int id;
        public override string GetPathToIco() => "Assets/Editor/NodeEditor/Images/EventsIco/ImportantDecision.png";
#endif
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(ImportantDecision))]
    public class ImportantDecisionGUInspector : Editor
    {
        private ImportantDecision _importantDecision;

        private void OnEnable() => _importantDecision = (ImportantDecision)target;

        public override void OnInspectorGUI() => ShowEventEditor(_importantDecision);

        public static void ShowEventEditor(ImportantDecision importantDecision)
        {
            GUILayout.Label("Принять важное решение");

            EditorGUILayout.BeginVertical("Box");

            Object[] allDecisions = Resources.LoadAll("Decisions/", typeof(Decision));

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
                importantDecision.id = EditorGUILayout.Popup(importantDecision.id, names);
                importantDecision.decision = (Decision)allDecisions[importantDecision.id];
            }
            else GUILayout.Label("Нет решений");

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Создать", GUILayout.Width(70))) AssetDatabase.CreateAsset(CreateInstance(typeof(Decision)),
                "Assets/Resources/Decisions/" + allDecisions.Length + "_Decision.asset");

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
            if (importantDecision.decision != null) DecisionGUInspector.ShowItemEditor(importantDecision.decision);

            EditorGUILayout.EndVertical();
        }
    }

#endif
}