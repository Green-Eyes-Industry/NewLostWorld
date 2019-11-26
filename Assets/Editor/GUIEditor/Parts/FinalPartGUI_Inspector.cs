using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FinalPart))]
public class FinalPartGUI_Inspector : Editor
{
    private FinalPart _finalPart;

    private void OnEnable() => _finalPart = (FinalPart)target;

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Текстовые поля");

        GUILayout.BeginVertical("Box");

        _finalPart.mainText = EditorGUILayout.TextArea(_finalPart.mainText, GUILayout.Height(100));
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        _finalPart.backButtonText = EditorGUILayout.TextArea(_finalPart.backButtonText, GUILayout.Height(40));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        _finalPart.newAchive = (Achivemants)EditorGUILayout.ObjectField("Получаемое достижение :", _finalPart.newAchive, typeof(Achivemants), true);

        if (GUILayout.Button("Сохранить", GUILayout.Height(30))) EditorUtility.SetDirty(_finalPart);
    }
}