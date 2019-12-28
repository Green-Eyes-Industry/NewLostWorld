using UnityEditor;
using UnityEngine;

namespace Data.GameParts
{
    [CreateAssetMenu(fileName = "New part", menuName = "Игровые обьекты/Новая глава/Глава Текстовой вставки", order = 5)]
    public class LeandPart : GamePart { }

#if UNITY_EDITOR

    [CustomEditor(typeof(LeandPart))]
    public class LeandPartGInspector : Editor
    {
        private LeandPart _leandPart;
        private Texture _dellConnect;
        private Texture _noneConnect;

        private void OnEnable()
        {
            _leandPart = (LeandPart)target;
            _dellConnect = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeEditor/Images/GUIInspector/DellConnectButton.png", typeof(Texture));
            _noneConnect = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeEditor/Images/GUIInspector/NullConnectButton.png", typeof(Texture));
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Текстовые поля");

            GUILayout.BeginHorizontal("Box");

            _leandPart.mainText = EditorGUILayout.TextArea(_leandPart.mainText, GUILayout.Height(100));

            if (_leandPart.movePart[0] != null)
            {
                if (GUILayout.Button(_dellConnect, GUILayout.Width(40), GUILayout.Height(40))) _leandPart.movePart[0] = null;
            }
            else GUILayout.Label(_noneConnect, GUILayout.Width(40), GUILayout.Height(40));

            GUILayout.EndHorizontal();
        }
    }

#endif
}