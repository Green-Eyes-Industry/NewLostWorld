using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemInteract))]
public class ItemInteractGUI_Inspector : Editor
{
    private ItemInteract _itemInteract;

    private void OnEnable() => _itemInteract = (ItemInteract)target;

    public override void OnInspectorGUI() => ShowEventEditor(_itemInteract);

    public static void ShowEventEditor(ItemInteract itemInteract)
    {
        GUILayout.Label("Событие взаимодействия с предметом");

        GUILayout.BeginHorizontal("Box");

        itemInteract.addOrLostItem = EditorGUILayout.Toggle("Получить предмет :", itemInteract.addOrLostItem);
        itemInteract.gameItem = (GameItem)EditorGUILayout.ObjectField(itemInteract.gameItem, typeof(GameItem), true, GUILayout.Width(200));

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Сохранить событие", GUILayout.Height(20))) EditorUtility.SetDirty(itemInteract);

        if(itemInteract.gameItem != null)
        {
            if (itemInteract.gameItem is PasiveItem) PasiveItemGUI_Inspector.ShowPassiveItemEditor((PasiveItem)itemInteract.gameItem);
            else if(itemInteract.gameItem is UsableItem) UsableItemGUI_Inspector.ShowUsableItemEditor((UsableItem)itemInteract.gameItem);
        }
    }
}