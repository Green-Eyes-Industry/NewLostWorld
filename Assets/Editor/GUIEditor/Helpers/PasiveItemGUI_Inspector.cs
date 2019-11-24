using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PasiveItem))]
public class PasiveItemGUI_Inspector : Editor
{
    private PasiveItem _pasiveItem;

    private void OnEnable() => _pasiveItem = (PasiveItem)target;

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Пассивный предмет");

        GUILayout.BeginHorizontal("Box");

        GUILayout.BeginVertical();

        _pasiveItem.itemName = EditorGUILayout.TextField("Название :", _pasiveItem.itemName);
        GUILayout.Label("Описание :");
        _pasiveItem.itemDescript = EditorGUILayout.TextArea(_pasiveItem.itemDescript,GUILayout.Height(40));

        GUILayout.EndVertical();

        _pasiveItem.itemIco = (Sprite)EditorGUILayout.ObjectField(_pasiveItem.itemIco, typeof(Sprite), true, GUILayout.Height(75),GUILayout.Width(75));

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Сохранить", GUILayout.Height(30))) _pasiveItem.SetDirty();
    }
}