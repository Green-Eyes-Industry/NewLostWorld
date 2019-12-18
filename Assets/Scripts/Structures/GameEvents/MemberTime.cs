using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MemberTime : GameEvent
{
    /// <summary> Заметка </summary>
    public Note _note;

    public override bool EventStart()
    {
        if (!DataController.playerData.playerNotes.Contains(_note)) DataController.playerData.playerNotes.Add(_note);

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
        public static int id = 0;

        private void OnEnable() => _memberTime = (MemberTime)target;

        public override void OnInspectorGUI() => ShowEventEditor(_memberTime);

        public static void ShowEventEditor(MemberTime memberTime)
        {
            GUILayout.Label("Найдена заметка");

            EditorGUILayout.BeginVertical("Box");

            object[] allNotes = Resources.LoadAll("Notes/", typeof(Note));

            string[] names = new string[allNotes.Length];

            Note nameConvert;

            for (int i = 0; i < names.Length; i++)
            {
                nameConvert = (Note)allNotes[i];

                if(nameConvert.noteName == "") names[i] = nameConvert.name;
                else names[i] = nameConvert.noteName;
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
            
            if (allNotes.Length > 0)
            {
                id = EditorGUILayout.Popup(id, names);
                memberTime._note = (Note)allNotes[id];
            }
            else GUILayout.Label("Нет заметок");

            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Создать", GUILayout.Width(70)))
            {
                AssetDatabase.CreateAsset(CreateInstance(typeof(Note)), "Assets/Resources/Notes/" + allNotes.Length + "_Note.asset");
            }

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;

            if (memberTime._note != null) NotesGUI_Inspector.ShowItemEditor(memberTime._note);

            EditorGUILayout.EndVertical();
        }
    }
}

#endif