using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Воспоминание")]
public class MemberTime : GameEvent
{
    /// <summary> Воспоминание </summary>
    public Note _note;

    public override bool EventStart()
    {
        if (!DataController.playerData.playerNotes.Contains(_note))
        {
            DataController.playerData.playerNotes.Add(_note);
            DataController.SaveNotes();
        }

        return true;
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(MemberTime))]
    public class MemberTimeGUI_Inspector : Editor
    {
        private MemberTime _memberTime;

        private void OnEnable() => _memberTime = (MemberTime)target;

        public override void OnInspectorGUI() => ShowEventEditor(_memberTime);

        public static void ShowEventEditor(MemberTime memberTime)
        {
            GUILayout.Label("Найдено воспоминание");

            EditorGUILayout.BeginVertical("Box");

            // Код

            EditorGUILayout.EndVertical();
        }
    }
}

#endif