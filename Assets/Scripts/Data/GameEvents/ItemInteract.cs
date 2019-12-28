using Data.Characters;
using Data.GameItems;
using UnityEditor;
using UnityEngine;

namespace Data.GameEvents
{
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
            Player mPlayer = MainController.instance.dataController.mainPlayer;

            if (isAddOrLostItem)
            {
                if (!mPlayer.playerInventory.Contains(gameItem)) mPlayer.playerInventory.Add(gameItem);
                return true;
            }
            else
            {
                if (mPlayer.playerInventory.Contains(gameItem))
                {
                    mPlayer.playerInventory.Remove(gameItem);
                    return true;
                }
                else return false;
            }
        }

        /// <summary> Вернуть главу провала </summary>
        public override GamePart FailPart() { return failPart; }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(ItemInteract))]
    public class ItemInteractGInspector : Editor
    {
        private ItemInteract _itemInteract;
        private static int _id = 0;

        private void OnEnable() => _itemInteract = (ItemInteract)target;

        public override void OnInspectorGUI() => ShowEventEditor(_itemInteract);

        public static void ShowEventEditor(ItemInteract itemInteract)
        {
            GUILayout.Label("Получение или потеря предмета");

            EditorGUILayout.BeginVertical("Box");

            Object[] allItems = Resources.LoadAll("GameItems/", typeof(GameItem));

            string[] names = new string[allItems.Length];

            for (int i = 0; i < names.Length; i++)
            {
                GameItem nameConvert = (GameItem)allItems[i];

                if (nameConvert.itemName == "") names[i] = nameConvert.name;
                else names[i] = nameConvert.itemName;
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));

            if (allItems.Length > 0)
            {
                _id = EditorGUILayout.Popup(_id, names);
                itemInteract.gameItem = (GameItem)allItems[_id];
            }
            else GUILayout.Label("Нет локаций");

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Active", GUILayout.Width(70))) AssetDatabase.CreateAsset(CreateInstance(typeof(UsableItem)),
                "Assets/Resources/Locations/" + allItems.Length + "_UsableItem.asset");
            if (GUILayout.Button("Pasive", GUILayout.Width(70))) AssetDatabase.CreateAsset(CreateInstance(typeof(PasiveItem)),
                "Assets/Resources/Locations/" + allItems.Length + "_PasiveItem.asset");

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
            if (itemInteract.gameItem != null)
            {
                if(itemInteract.gameItem is PasiveItem pasiveItem) PasiveItemGInspector.ShowItemEditor(pasiveItem);
                else if (itemInteract.gameItem is UsableItem usableItem) UsableItemGInspector.ShowItemEditor(usableItem);
            }

            EditorGUILayout.EndVertical();
        }
    }

#endif
}