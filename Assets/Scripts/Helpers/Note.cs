using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Data;
#endif

namespace NLW.Data
{
    [CreateAssetMenu(fileName = "new note", menuName = "Игровые обьекты/Заметка")]
    public class Note : ScriptableObject
    {
        /// <summary> Название заметки </summary>
        public string noteName;

        /// <summary> Текст заметки </summary>
        public string noteDescription;

        /// <summary> Глава воспоминания </summary>
        public Parts.GamePart partNote;
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(Note))]
    public class NotesGUI_Inspector : Editor
    {
        private Note _notes;

        private void OnEnable() => _notes = (Note)target;

        public override void OnInspectorGUI() => ShowItemEditor(_notes);

        /// <summary> Отобразить в редакторе </summary>
        public static void ShowItemEditor(Note note)
        {
            EditorGUILayout.LabelField("Заметка");

            GUILayout.BeginVertical("Box");

            note.noteName = EditorGUILayout.TextField("Название заметки :", note.noteName);
            EditorGUILayout.LabelField("Текст заметки");
            note.noteDescription = EditorGUILayout.TextArea(note.noteDescription, GUILayout.Height(40));
            EditorGUILayout.Space();
            note.partNote = (NLW.Parts.GamePart)EditorGUILayout.ObjectField("Глава воспоминания :", note.partNote, typeof(NLW.Parts.GamePart), true);

            GUILayout.EndVertical();
        }
    }
}

#endif