using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New part", menuName = "Игровые обьекты/Новая глава/Глава Текстовой вставки", order = 5)]
public class LeandPart : GamePart { }

#if UNITY_EDITOR

namespace GUIInspector
{

    [CustomEditor(typeof(LeandPart))]
    public class LeandPartGUI_Inspector : Editor
    {
        private LeandPart _leandPart;

        private void OnEnable() => _leandPart = (LeandPart)target;

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Текстовые поля");

            GUILayout.BeginVertical("Box");

            _leandPart.mainText = EditorGUILayout.TextArea(_leandPart.mainText, GUILayout.Height(100));
            _leandPart.movePart_1 = (GamePart)EditorGUILayout.ObjectField("Следующая глава: ", _leandPart.movePart_1, typeof(GamePart), true);

            GUILayout.EndVertical();
        }
    }
}

#endif