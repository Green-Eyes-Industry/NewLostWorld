using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CheckDecision : GameEvent
{
    /// <summary> Решение </summary>
    public Decision decision;

    /// <summary> Глава при проверке </summary>
    public GamePart _failPart;

    /// <summary> Проверка на принятое ранее решение </summary>
    /// <returns> Вернет False при провале проверки </returns>
    public override bool EventStart()
    {
        for (int i = 0; i < DataController.playerData.playerDecisions.Count; i++)
        {
            if (DataController.playerData.playerDecisions[i].Equals(decision)) return true;
        }

        return false;
    }

    /// <summary> Вернуть главу провала </summary>
    public override GamePart FailPart() { return _failPart; }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(CheckDecision))]
    public class CheckDecisionGUI_Inspector : Editor
    {
        private CheckDecision _checkDecision;
        public static int id = 0;

        private void OnEnable() => _checkDecision = (CheckDecision)target;

        public override void OnInspectorGUI() => ShowEventEditor(_checkDecision);

        public static void ShowEventEditor(CheckDecision checkDecision)
        {
            GUILayout.Label("Проверка решения персонажа");

            EditorGUILayout.BeginVertical("Box");

            checkDecision._failPart = (GamePart)EditorGUILayout.ObjectField("Глава провала : ", checkDecision._failPart, typeof(GamePart), true);

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
            if (checkDecision.decision != null) DecisionGUI_Inspector.ShowItemEditor(checkDecision.decision);

            EditorGUILayout.EndVertical();
        }
    }
}

#endif