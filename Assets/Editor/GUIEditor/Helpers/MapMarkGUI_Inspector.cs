using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapMark))]
public class MapMarkGUI_Inspector : Editor
{
    private MapMark _mapMark;

    private void OnEnable() => _mapMark = (MapMark)target;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Метка на карте");

        GUILayout.BeginVertical("Box");

        _mapMark.nameLocation = EditorGUILayout.TextField("Название локации :", _mapMark.nameLocation);
        EditorGUILayout.LabelField("Лор локации");
        _mapMark.loreLocation = EditorGUILayout.TextArea(_mapMark.loreLocation, GUILayout.Height(40));
        EditorGUILayout.Space();
        _mapMark._partLocation = (GamePart)EditorGUILayout.ObjectField("Глава локации :", _mapMark._partLocation, typeof(GamePart), true);

        GUILayout.EndVertical();

        if (GUILayout.Button("Сохранить", GUILayout.Height(20))) EditorUtility.SetDirty(_mapMark);
    }

}