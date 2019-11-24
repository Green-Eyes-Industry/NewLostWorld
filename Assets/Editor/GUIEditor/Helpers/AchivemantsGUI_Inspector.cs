using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Achivemants))]
public class AchivemantsGUI_Inspector : Editor
{
    private Achivemants _achivemants;

    private void OnEnable() => _achivemants = (Achivemants)target;

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Достижение");

        GUILayout.BeginHorizontal("Box");

        GUILayout.BeginVertical();

        _achivemants.achiveName = EditorGUILayout.TextField("Название :", _achivemants.achiveName);
        GUILayout.Label("Описание :");
        _achivemants.achiveDescript = EditorGUILayout.TextArea(_achivemants.achiveDescript, GUILayout.Height(40));

        GUILayout.EndVertical();

        _achivemants.achiveIco = (Sprite)EditorGUILayout.ObjectField(_achivemants.achiveIco, typeof(Sprite), true, GUILayout.Height(75), GUILayout.Width(75));

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Сохранить", GUILayout.Height(30))) _achivemants.SetDirty();
    }
}