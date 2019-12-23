using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
using NLW.Data;
#endif

namespace NLW.Data
{
    public class CheckDecision : GameEvent
    {
        /// <summary> Решение </summary>
        public Decision decision;

        /// <summary> Глава при проверке </summary>
        public Parts.GamePart failPart;

        /// <summary> Проверка на принятое ранее решение </summary>
        /// <returns> Вернет False при провале проверки </returns>
        public override bool EventStart()
        {
            for (int i = 0; i < MainController.instance.dataController.mainPlayer.playerDecisions.Count; i++)
            {
                if (MainController.instance.dataController.mainPlayer.playerDecisions[i].Equals(decision)) return true;
            }

            return false;
        }

        /// <summary> Вернуть главу провала </summary>
        public override Parts.GamePart FailPart() { return failPart; }
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(CheckDecision))]
    public class CheckDecisionGInspector : Editor
    {
        private CheckDecision _checkDecision;
        public static int id = 0;

        private void OnEnable() => _checkDecision = (CheckDecision)target;

        public override void OnInspectorGUI() => ShowEventEditor(_checkDecision);

        public static void ShowEventEditor(CheckDecision checkDecision)
        {
            GUILayout.Label("Проверка решения персонажа");

            EditorGUILayout.BeginVertical("Box");

            checkDecision.failPart = (NLW.Parts.GamePart)EditorGUILayout.ObjectField("Глава провала : ", checkDecision.failPart, typeof(NLW.Parts.GamePart), true);

            object[] allDecisions = Resources.LoadAll("Decisions/", typeof(Decision));

            string[] names = new string[allDecisions.Length];

            for (int i = 0; i < names.Length; i++)
            {
                Decision nameConvert = (Decision)allDecisions[i];

                if (nameConvert.nameDecision == "") names[i] = nameConvert.name;
                else names[i] = nameConvert.nameDecision;
            }

            EditorGUILayout.BeginHorizontal();

            if(allDecisions.Length > 0)
            {
                id = EditorGUILayout.Popup(id, names);
                checkDecision.decision = (Decision)allDecisions[id];
            }
            else GUILayout.Label("Нет решений");

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Создать",GUILayout.Width(70))) AssetDatabase.CreateAsset(CreateInstance(typeof(Decision)),
                 "Assets/Resources/Decisions/" + allDecisions.Length + "_Decision.asset");

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
            if (checkDecision.decision != null) DecisionGInspector.ShowItemEditor(checkDecision.decision);

            EditorGUILayout.EndVertical();
        }
    }
}

#endif