using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UsableItem))]
public class UsableItemGUI_Inspector : Editor
{
    private UsableItem _usableItem;

    private void OnEnable() => _usableItem = (UsableItem)target;

    public override void OnInspectorGUI() => ShowUsableItemEditor(_usableItem);

    /// <summary>
    /// Показать редактор активного предмета
    /// </summary>
    public static void ShowUsableItemEditor(UsableItem usableItem)
    {
        GUILayout.Label("Активный предмет");

        GUILayout.BeginHorizontal("Box");

        GUILayout.BeginVertical();

        usableItem.itemName = EditorGUILayout.TextField("Название :", usableItem.itemName);
        GUILayout.Label("Описание :");
        usableItem.itemDescript = EditorGUILayout.TextArea(usableItem.itemDescript, GUILayout.Height(40));

        GUILayout.EndVertical();

        usableItem.itemIco = (Sprite)EditorGUILayout.ObjectField(usableItem.itemIco, typeof(Sprite), true, GUILayout.Height(75), GUILayout.Width(75));

        GUILayout.EndHorizontal();

        GUILayout.Label("Влияние");

        GUILayout.BeginVertical("Box");

        usableItem.healthInf = EditorGUILayout.IntSlider("Здоровье :", usableItem.healthInf, -100, 100);
        usableItem.mindInf = EditorGUILayout.IntSlider("Рассудок :", usableItem.mindInf, -100, 100);

        GUILayout.EndVertical();

        if (GUILayout.Button("Сохранить предмет", GUILayout.Height(20))) EditorUtility.SetDirty(usableItem);
    }
}