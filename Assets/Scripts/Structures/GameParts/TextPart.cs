﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Parts;
#endif

namespace NLW.Parts
{
    public class TextPart : GamePart
    {
        /// <summary> Текст первой кнопки </summary>
        public string buttonText;
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{

    [CustomEditor(typeof(TextPart))]
    public class TextPartGInspector : Editor
    {
        private TextPart _textPart;

        private Texture _dellConnect;
        private Texture _noneConnect;

        private void OnEnable()
        {
            _textPart = (TextPart)target;
            _dellConnect = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeEditor/Images/GUIInspector/DellConnectButton.png", typeof(Texture));
            _noneConnect = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeEditor/Images/GUIInspector/NullConnectButton.png", typeof(Texture));
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Текстовые поля");

            GUILayout.BeginVertical("Box");

            _textPart.mainText = EditorGUILayout.TextArea(_textPart.mainText, GUILayout.Height(100));
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            _textPart.buttonText = EditorGUILayout.TextArea(_textPart.buttonText, GUILayout.Height(40));

            if (_textPart.movePart[0] != null)
            {
                if (GUILayout.Button(_dellConnect, GUILayout.Width(40), GUILayout.Height(40))) _textPart.movePart[0] = null;
            }
            else GUILayout.Label(_noneConnect, GUILayout.Width(40), GUILayout.Height(40));

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.Label("Параметры");

            if (_textPart.mainEvents != null) GlobalHelperGInspector.ShowPartEventList(_textPart.mainEvents);
            else _textPart.mainEvents = new System.Collections.Generic.List<NLW.Data.GameEvent>();
        }
    }
}

#endif