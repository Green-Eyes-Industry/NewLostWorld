using Controllers;
using Data.Characters;
using Helpers;
using UnityEditor;
using UnityEngine;

namespace Data.GameEvents
{
    public class MemberTime : GameEvent, IMessage
    {
        /// <summary> Заметка </summary>
        public Note note;

        public override bool EventStart()
        {
            Player mPlayer = MainController.instance.dataController.mainPlayer;

            if (!mPlayer.playerNotes.Contains(note))
            {
                MainController.instance.effectsController.ShowMessage(this);
                mPlayer.playerNotes.Add(note);
            }

            return true;
        }

        public string GetText() => "Новая заметка\n" + note.noteName;

        public AnimController.MessangeType GetAnimationType() => AnimController.MessangeType.NOTE_MS;
        
#if UNITY_EDITOR
        public int id;
#endif
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MemberTime))]
    public class MemberTimeGUInspector : Editor
    {
        private MemberTime _memberTime;

        private void OnEnable() => _memberTime = (MemberTime)target;

        public override void OnInspectorGUI() => ShowEventEditor(_memberTime);

        public static void ShowEventEditor(MemberTime memberTime)
        {
            GUILayout.Label("Найдена заметка");

            EditorGUILayout.BeginVertical("Box");

            object[] allNotes = Resources.LoadAll("Notes/", typeof(Note));

            string[] names = new string[allNotes.Length];

            for (int i = 0; i < names.Length; i++)
            {
                Note nameConvert = (Note)allNotes[i];

                names[i] = (nameConvert.noteName == "") ? nameConvert.name : nameConvert.noteName;
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
            
            if (allNotes.Length > 0)
            {
                memberTime.id = EditorGUILayout.Popup(memberTime.id, names);
                memberTime.note = (Note)allNotes[memberTime.id];
            }
            else GUILayout.Label("Нет заметок");

            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Создать", GUILayout.Width(70)))
                AssetDatabase.CreateAsset(CreateInstance(typeof(Note)), "Assets/Resources/Notes/" + allNotes.Length + "_Note.asset");

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;

            if (memberTime.note != null) NotesGUInspector.ShowItemEditor(memberTime.note);

            EditorGUILayout.EndVertical();
        }
    }

#endif
}