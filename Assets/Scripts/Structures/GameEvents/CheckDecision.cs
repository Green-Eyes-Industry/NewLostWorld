using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Проверка решения")]
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

        private void OnEnable() => _checkDecision = (CheckDecision)target;

        public override void OnInspectorGUI() => ShowEventEditor(_checkDecision);

        public static void ShowEventEditor(CheckDecision checkDecision)
        {
            GUILayout.Label("Проверка решения персонажа");

            EditorGUILayout.BeginVertical("Box");

            // Код

            EditorGUILayout.EndVertical();
        }
    }
}

#endif