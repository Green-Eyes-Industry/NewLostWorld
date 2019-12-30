using Helpers;
using UnityEditor;
using UnityEngine;

namespace Data.GameEvents
{
    public class CheckDecision : GameEvent
    {
        /// <summary> Решение </summary>
        public Decision decision;

        /// <summary> Глава при проверке </summary>
        public GamePart failPart;

        /// <summary> Проверка на принятое ранее решение </summary>
        /// <returns> Вернет False при провале проверки </returns>
        public override bool EventStart()
        {
            foreach (Decision dec in MainController.instance.dataController.mainPlayer.playerDecisions)
            {
                if (dec.Equals(decision)) return true;
            }

            return false;
        }

        /// <summary> Вернуть главу провала </summary>
        public override GamePart FailPart() { return failPart; }

#if UNITY_EDITOR
        public int id;
#endif
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(CheckDecision))]
    public class CheckDecisionGInspector : Editor
    {
        private CheckDecision _checkDecision;

        private void OnEnable() => _checkDecision = (CheckDecision)target;

        public override void OnInspectorGUI() => ShowEventEditor(_checkDecision);

        public static void ShowEventEditor(CheckDecision checkDecision)
        {
            GUILayout.Label("Проверка решения персонажа");

            EditorGUILayout.BeginVertical("Box");

            checkDecision.failPart = (GamePart)EditorGUILayout.ObjectField("Глава провала : ", checkDecision.failPart, typeof(GamePart), true);

            Object[] allDecisions = Resources.LoadAll("Decisions/", typeof(Decision));

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
                checkDecision.id = EditorGUILayout.Popup(checkDecision.id, names);
                checkDecision.decision = (Decision)allDecisions[checkDecision.id];
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

#endif
}