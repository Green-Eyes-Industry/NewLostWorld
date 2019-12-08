using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "new note", menuName = "Игровые обьекты/Заметка")]
public class Note : ScriptableObject
{
    /// <summary> Название заметки </summary>
    public string noteName;

    /// <summary> Текст заметки </summary>
    public string noteDescription;

    /// <summary> Глава воспоминания </summary>
    public GamePart partNote;
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(Note))]
    public class NotesGUI_Inspector : Editor
    {
        private Note _notes;

        private void OnEnable() => _notes = (Note)target;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Заметка");

            GUILayout.BeginVertical("Box");

            _notes.noteName = EditorGUILayout.TextField("Название заметки :", _notes.noteName);
            EditorGUILayout.LabelField("Текст заметки");
            _notes.noteDescription = EditorGUILayout.TextArea(_notes.noteDescription, GUILayout.Height(40));
            EditorGUILayout.Space();
            _notes.partNote = (GamePart)EditorGUILayout.ObjectField("Глава воспоминания :", _notes.partNote, typeof(GamePart), true);

            GUILayout.EndVertical();
        }
    }
}

#endif