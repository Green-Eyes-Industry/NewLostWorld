using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Data;
#endif

namespace NLW.Data
{
    [CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Взаимодействие с предметом")]
    public class ItemInteract : GameEvent
    {
        /// <summary> Получение или потеря </summary>
        public bool isAddOrLostItem;

        /// <summary> Получаемый предмет </summary>
        public GameItem gameItem;

        /// <summary> Глава при провале </summary>
        public Parts.GamePart _failPart;

        /// <summary> Старт события </summary>
        public override bool EventStart()
        {
            Player mPlayer = MainController.Instance.mainPlayer;

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
        public override Parts.GamePart FailPart() { return _failPart; }
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(ItemInteract))]
    public class ItemInteractGUI_Inspector : Editor
    {
        private ItemInteract _itemInteract;
        public static int id = 0;

        private void OnEnable() => _itemInteract = (ItemInteract)target;

        public override void OnInspectorGUI() => ShowEventEditor(_itemInteract);

        public static void ShowEventEditor(ItemInteract itemInteract)
        {
            GUILayout.Label("Получение или потеря предмета");

            EditorGUILayout.BeginVertical("Box");

            object[] allItems = Resources.LoadAll("GameItems/", typeof(GameItem));

            string[] names = new string[allItems.Length];

            GameItem nameConvert;

            for (int i = 0; i < names.Length; i++)
            {
                nameConvert = (GameItem)allItems[i];

                if (nameConvert.itemName == "") names[i] = nameConvert.name;
                else names[i] = nameConvert.itemName;
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));

            if (allItems.Length > 0)
            {
                id = EditorGUILayout.Popup(id, names);
                itemInteract.gameItem = (GameItem)allItems[id];
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
                if(itemInteract.gameItem is PasiveItem pasiveItem) PasiveItemGUI_Inspector.ShowItemEditor(pasiveItem);
                else if (itemInteract.gameItem is UsableItem usableItem) UsableItemGUI_Inspector.ShowItemEditor(usableItem);
            }

            EditorGUILayout.EndVertical();
        }
    }
}

#endif