using Data.Characters;
using Data.GameItems;
using UnityEditor;
using UnityEngine;

namespace Data.GameEvents
{
    public class ItemInfl : GameEvent
    {
        /// <summary> Уничтожить в процессе </summary>
        public bool isRemove;

        /// <summary> Предмет </summary>
        public UsableItem useItem;

        /// <summary> Глава при провале </summary>
        public GamePart failPart;

        /// <summary> Влияние </summary>
        public override bool EventStart()
        {
            Player mPlayer = MainController.instance.dataController.mainPlayer;

            if (mPlayer.playerInventory.Contains(useItem))
            {
                useItem.UseThisItem();
                if (isRemove)
                {
                    MainController.instance.effectsController.LostItemMessage(useItem);
                    mPlayer.playerInventory.Remove(useItem);
                }

                return true;
            }
            else return false;
        }

        /// <summary> Вернуть главу провала </summary>
        public override GamePart FailPart() => failPart;

#if UNITY_EDITOR
        public int id;
        public override string GetPathToIco() => "Assets/Editor/NodeEditor/Images/EventsIco/ItemInfl.png";
#endif

    }

#if UNITY_EDITOR

    [CustomEditor(typeof(ItemInfl))]
    public class ItemInflGUInspector : Editor
    {
        private ItemInfl _itemInfl;

        private void OnEnable() => _itemInfl = (ItemInfl)target;

        public override void OnInspectorGUI() => ShowEventEditor(_itemInfl);

        public static void ShowEventEditor(ItemInfl itemInfl)
        {
            GUILayout.Label("Принудительно использовать предмет");

            EditorGUILayout.BeginVertical("Box");

            Object[] allItems = Resources.LoadAll("GameItems/", typeof(UsableItem));

            string[] names = new string[allItems.Length];

            for (int i = 0; i < names.Length; i++)
            {
                UsableItem nameConvert = (UsableItem)allItems[i];

                if (nameConvert.itemName == "") names[i] = nameConvert.name;
                else names[i] = nameConvert.itemName;
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));

            if (allItems.Length > 0)
            {
                itemInfl.id = EditorGUILayout.Popup(itemInfl.id, names);
                itemInfl.useItem = (UsableItem)allItems[itemInfl.id];
                itemInfl.isRemove = EditorGUILayout.Toggle(itemInfl.isRemove, GUILayout.Width(20));
            }
            else GUILayout.Label("Нет предметов");

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Создать", GUILayout.Width(70))) AssetDatabase.CreateAsset(CreateInstance(typeof(UsableItem)),
                "Assets/Resources/GameItems/" + allItems.Length + "_UsableItem.asset");

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;

            itemInfl.failPart = (GamePart)EditorGUILayout.ObjectField("Глава провала : ", itemInfl.failPart, typeof(GamePart), true);

            if (itemInfl.useItem != null) UsableItemGUInspector.ShowItemEditor(itemInfl.useItem);

            EditorGUILayout.EndVertical();
        }
    }

#endif
}