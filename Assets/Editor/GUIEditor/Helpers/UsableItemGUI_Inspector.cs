using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UsableItem))]
public class UsableItemGUI_Inspector : Editor
{
    private UsableItem _usableItem;

    private void OnEnable() => _usableItem = (UsableItem)target;

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Активный предмет");

        GUILayout.BeginHorizontal("Box");

        GUILayout.BeginVertical();

        _usableItem.itemName = EditorGUILayout.TextField("Название :", _usableItem.itemName);
        GUILayout.Label("Описание :");
        _usableItem.itemDescript = EditorGUILayout.TextArea(_usableItem.itemDescript, GUILayout.Height(40));

        GUILayout.EndVertical();

        _usableItem.itemIco = (Sprite)EditorGUILayout.ObjectField(_usableItem.itemIco, typeof(Sprite), true, GUILayout.Height(75), GUILayout.Width(75));

        GUILayout.EndHorizontal();

        GUILayout.Label("Влияние");

        GUILayout.BeginVertical("Box");

        _usableItem.healthInf = EditorGUILayout.IntSlider("Здоровье :", _usableItem.healthInf, -100, 100);
        _usableItem.mindInf = EditorGUILayout.IntSlider("Рассудок :", _usableItem.mindInf, -100, 100);

        GUILayout.EndVertical();

        if (GUILayout.Button("Сохранить", GUILayout.Height(30))) _usableItem.SetDirty();
    }
}