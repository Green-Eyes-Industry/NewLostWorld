using UnityEngine;
using UnityEditor;

namespace GUIInspector
{
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

            itemInteract.isAddOrLostItem = EditorGUILayout.Toggle("Получить предмет :", itemInteract.isAddOrLostItem);
            itemInteract.gameItem = (GameItem)EditorGUILayout.ObjectField(itemInteract.gameItem, typeof(GameItem), true, GUILayout.Width(200));

            GUILayout.EndHorizontal();

            if (!itemInteract.isAddOrLostItem)
            {
                GUILayout.BeginHorizontal("Box");

                itemInteract.failPart = (GamePart)EditorGUILayout.ObjectField(
                    "Отсутствие :",
                    itemInteract.failPart,
                    typeof(GamePart),
                    true);

                GUILayout.EndHorizontal();
            }

            if (itemInteract.gameItem != null)
            {
                if (itemInteract.gameItem is PasiveItem) PasiveItemGUI_Inspector.ShowPassiveItemEditor((PasiveItem)itemInteract.gameItem);
                else if (itemInteract.gameItem is UsableItem) UsableItemGUI_Inspector.ShowUsableItemEditor((UsableItem)itemInteract.gameItem);
            }
        }
    }
}