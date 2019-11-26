using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PasiveItem))]
public class PasiveItemGUI_Inspector : Editor
{
    private PasiveItem _pasiveItem;

    private void OnEnable() => _pasiveItem = (PasiveItem)target;

    public override void OnInspectorGUI() => ShowPassiveItemEditor(_pasiveItem);

    /// <summary>
    /// Показать редактор пассивного предмета
    /// </summary>
    public static void ShowPassiveItemEditor(PasiveItem pasiveItem)
    {
        GUILayout.Label("Пассивный предмет");

        GUILayout.BeginHorizontal("Box");

        GUILayout.BeginVertical();

        pasiveItem.itemName = EditorGUILayout.TextField("Название :", pasiveItem.itemName);
        GUILayout.Label("Описание :");
        pasiveItem.itemDescript = EditorGUILayout.TextArea(pasiveItem.itemDescript, GUILayout.Height(40));

        GUILayout.EndVertical();

        pasiveItem.itemIco = (Sprite)EditorGUILayout.ObjectField(pasiveItem.itemIco, typeof(Sprite), true, GUILayout.Height(75), GUILayout.Width(75));

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Сохранить предмет", GUILayout.Height(20))) EditorUtility.SetDirty(pasiveItem);
    }
}