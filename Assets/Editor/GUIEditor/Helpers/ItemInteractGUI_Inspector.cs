using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemInteract))]
public class ItemInteractGUI_Inspector : Editor
{
    private ItemInteract _itemInteract;

    private void OnEnable() => _itemInteract = (ItemInteract)target;

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Событие взаимодействия с предметом");

        GUILayout.BeginHorizontal("Box");

        _itemInteract.addOrLostItem = EditorGUILayout.Toggle("Получить предмет :", _itemInteract.addOrLostItem);
        _itemInteract.gameItem = (GameItem)EditorGUILayout.ObjectField(_itemInteract.gameItem, typeof(GameItem), true, GUILayout.Width(200));

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Сохранить", GUILayout.Height(30))) _itemInteract.SetDirty();
    }
}