using UnityEngine;
using UnityEditor;

namespace GUIInspector
{

    [CustomEditor(typeof(Notes))]
    public class NotesGUI_Inspector : Editor
    {
        private Notes _notes;

        private void OnEnable() => _notes = (Notes)target;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Заметка");

            GUILayout.BeginVertical("Box");

            _notes.noteName = EditorGUILayout.TextField("Название заметки :", _notes.noteName);
            EditorGUILayout.LabelField("Текст заметки");
            _notes.noteDescription = EditorGUILayout.TextArea(_notes.noteDescription, GUILayout.Height(40));
            EditorGUILayout.Space();
            _notes._partNote = (GamePart)EditorGUILayout.ObjectField("Глава воспоминания :", _notes._partNote, typeof(GamePart), true);

            GUILayout.EndVertical();

            if (GUILayout.Button("Сохранить", GUILayout.Height(20))) EditorUtility.SetDirty(_notes);
        }
    }
}