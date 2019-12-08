using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Взаимодействие с предметом")]
public class ItemInteract : GameEvent
{
    /// <summary> Получение или потеря </summary>
    public bool isAddOrLostItem;

    /// <summary> Получаемый предмет </summary>
    public GameItem gameItem;

    /// <summary> Глава при провале </summary>
    public GamePart failPart;

    /// <summary> Старт события </summary>
    public override bool EventStart()
    {
        if (isAddOrLostItem)
        {
            if (!DataController.playerData.playerInventory.Contains(gameItem))
            {
                DataController.playerData.playerInventory.Add(gameItem);
            }
            return true;
        }
        else
        {
            if (DataController.playerData.playerInventory.Contains(gameItem))
            {
                DataController.playerData.playerInventory.Remove(gameItem);
                return true;
            }
            else return true;
        }
    }
}


#if UNITY_EDITOR

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

#endif