using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Parts;
#endif

namespace NLW.Parts
{
    public class BattlePart : GamePart
    {
        /// <summary> Текст первой кнопки </summary>
        public string[] buttonText;
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{

    [CustomEditor(typeof(BattlePart))]
    public class BattlePartGUI_Inspector : Editor
    {
        private BattlePart _battlePart;

        private Texture dellConnect;
        private Texture noneConnect;

        private void OnEnable()
        {
            _battlePart = (BattlePart)target;
            dellConnect = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeEditor/Images/GUIInspector/DellConnectButton.png", typeof(Texture));
            noneConnect = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeEditor/Images/GUIInspector/NullConnectButton.png", typeof(Texture));
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Текстовые поля");

            GUILayout.BeginVertical("Box");

            _battlePart.mainText = EditorGUILayout.TextArea(_battlePart.mainText, GUILayout.Height(100));
            EditorGUILayout.Space();

            for (int i = 0; i < _battlePart.buttonText.Length; i++)
            {
                GUILayout.BeginHorizontal();
                _battlePart.buttonText[i] = EditorGUILayout.TextArea(_battlePart.buttonText[i], GUILayout.Height(40));

                if (_battlePart.movePart[i] != null)
                {
                    if (GUILayout.Button(dellConnect, GUILayout.Width(40), GUILayout.Height(40))) _battlePart.movePart[i] = null;
                }
                else GUILayout.Label(noneConnect, GUILayout.Width(40), GUILayout.Height(40));

                GUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

            GUILayout.EndVertical();

            GUILayout.Label("Параметры");

            if (_battlePart.mainEvents != null) GlobalHelperGUI_Inspector.ShowPartEventList(_battlePart.mainEvents);
            else _battlePart.mainEvents = new System.Collections.Generic.List<NLW.Data.GameEvent>();
        }
    }
}

#endif