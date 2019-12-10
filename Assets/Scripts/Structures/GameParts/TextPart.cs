using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New part", menuName = "Игровые обьекты/Новая глава/Текстовая глава", order = 0)]
public class TextPart : GamePart
{
    /// <summary> Текст первой кнопки </summary>
    public string buttonText_1;
}

#if UNITY_EDITOR

namespace GUIInspector
{

    [CustomEditor(typeof(TextPart))]
    public class TextPartGUI_Inspector : Editor
    {
        private TextPart _textPart;

        private Texture dellConnect;
        private Texture noneConnect;

        private void OnEnable()
        {
            _textPart = (TextPart)target;
            dellConnect = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeEditor/Images/GUIInspector/DellConnectButton.png", typeof(Texture));
            noneConnect = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeEditor/Images/GUIInspector/NullConnectButton.png", typeof(Texture));
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Текстовые поля");

            GUILayout.BeginVertical("Box");

            _textPart.mainText = EditorGUILayout.TextArea(_textPart.mainText, GUILayout.Height(100));
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            _textPart.buttonText_1 = EditorGUILayout.TextArea(_textPart.buttonText_1, GUILayout.Height(40));

            if (_textPart.movePart_1 != null)
            {
                if (GUILayout.Button(dellConnect, GUILayout.Width(40), GUILayout.Height(40))) _textPart.movePart_1 = null;
            }
            else GUILayout.Label(noneConnect, GUILayout.Width(40), GUILayout.Height(40));

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.Label("Параметры");

            if (_textPart.mainEvents != null) GlobalHelperGUI_Inspector.ShowPartEventList(_textPart.mainEvents);
            else _textPart.mainEvents = new System.Collections.Generic.List<GameEvent>();
        }
    }
}

#endif