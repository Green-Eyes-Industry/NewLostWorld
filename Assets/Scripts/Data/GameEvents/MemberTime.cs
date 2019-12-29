﻿using Data.Characters;
using Helpers;
using UnityEditor;
using UnityEngine;

namespace Data.GameEvents
{
    public class MemberTime : GameEvent
    {
        /// <summary> Заметка </summary>
        public Note note;

        public override bool EventStart()
        {
            Player mPlayer = MainController.instance.dataController.mainPlayer;

            if (!mPlayer.playerNotes.Contains(note))
            {
                MainController.instance.effectsController.AddNoteMessage(note);
                mPlayer.playerNotes.Add(note);
            }

            return true;
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MemberTime))]
    public class MemberTimeGInspector : Editor
    {
        private MemberTime _memberTime;
        private static int _id = 0;

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

                if(nameConvert.noteName == "") names[i] = nameConvert.name;
                else names[i] = nameConvert.noteName;
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
            
            if (allNotes.Length > 0)
            {
                _id = EditorGUILayout.Popup(_id, names);
                memberTime.note = (Note)allNotes[_id];
            }
            else GUILayout.Label("Нет заметок");

            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Создать", GUILayout.Width(70)))
            {
                AssetDatabase.CreateAsset(CreateInstance(typeof(Note)), "Assets/Resources/Notes/" + allNotes.Length + "_Note.asset");
            }

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;

            if (memberTime.note != null) NotesGInspector.ShowItemEditor(memberTime.note);

            EditorGUILayout.EndVertical();
        }
    }

#endif
}