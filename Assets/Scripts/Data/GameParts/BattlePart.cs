using UnityEditor;
using UnityEngine;

namespace Data.GameParts
{
    public class BattlePart : GamePart
    {
        /// <summary> Текст первой кнопки </summary>
        public string[] buttonText;
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(BattlePart))]
    public class BattlePartGUInspector : Editor
    {
        private BattlePart _battlePart;

        private Texture _dellConnect;
        private Texture _noneConnect;

        private void OnEnable()
        {
            _battlePart = (BattlePart)target;
            _dellConnect = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeEditor/Images/GUIInspector/DellConnectButton.png", typeof(Texture));
            _noneConnect = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeEditor/Images/GUIInspector/NullConnectButton.png", typeof(Texture));
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

                if (_battlePart.movePart[i] != null && GUILayout.Button(_dellConnect, GUILayout.Width(40), GUILayout.Height(40))) _battlePart.movePart[i] = null;
                else GUILayout.Label(_noneConnect, GUILayout.Width(40), GUILayout.Height(40));

                GUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

            GUILayout.EndVertical();

            GUILayout.Label("Параметры");

            if (_battlePart.mainEvents != null) GlobalHelperGUInspector.ShowPartEventList(_battlePart.mainEvents);
            else _battlePart.mainEvents = new System.Collections.Generic.List<GameEvent>();
        }
    }

#endif
}