using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Parts;
#endif

namespace NLW.Parts
{
    [CreateAssetMenu(fileName = "New part", menuName = "Игровые обьекты/Новая глава/Глава Текстовой вставки", order = 5)]
    public class LeandPart : GamePart { }
}

#if UNITY_EDITOR

namespace GUIInspector
{

    [CustomEditor(typeof(LeandPart))]
    public class LeandPartGUI_Inspector : Editor
    {
        private LeandPart _leandPart;
        private Texture dellConnect;
        private Texture noneConnect;

        private void OnEnable()
        {
            _leandPart = (LeandPart)target;
            dellConnect = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeEditor/Images/GUIInspector/DellConnectButton.png", typeof(Texture));
            noneConnect = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeEditor/Images/GUIInspector/NullConnectButton.png", typeof(Texture));
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Текстовые поля");

            GUILayout.BeginHorizontal("Box");

            _leandPart.mainText = EditorGUILayout.TextArea(_leandPart.mainText, GUILayout.Height(100));

            if (_leandPart.movePart[0] != null)
            {
                if (GUILayout.Button(dellConnect, GUILayout.Width(40), GUILayout.Height(40))) _leandPart.movePart[0] = null;
            }
            else GUILayout.Label(noneConnect, GUILayout.Width(40), GUILayout.Height(40));

            GUILayout.EndHorizontal();
        }
    }
}

#endif