using UnityEngine;
using UnityEditor;

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

        if (GUILayout.Button("Сохранить", GUILayout.Height(30))) _leandPart.SetDirty();
    }
}