using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Data;
#endif

namespace NLW.Data
{
    [CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Использование предмета")]
    public class ItemInfl : GameEvent
    {
        /// <summary> Уничтожить в процессе </summary>
        public bool isRemove;

        /// <summary> Предмет </summary>
        public UsableItem useItem;

        /// <summary> Глава при провале </summary>
        public Parts.GamePart failPart;

        /// <summary> Влияние </summary>
        public override bool EventStart()
        {
            Player mPlayer = MainController.instance.dataController.mainPlayer;

            if (mPlayer.playerInventory.Contains(useItem))
            {
                useItem.UseThisItem();
                if (isRemove) mPlayer.playerInventory.Remove(useItem);
                return true;
            }
            else return false;
        }


        /// <summary> Вернуть главу провала </summary>
        public override Parts.GamePart FailPart() { return failPart; }
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(ItemInfl))]
    public class ItemInflGInspector : Editor
    {
        private ItemInfl _itemInfl;

        private void OnEnable() => _itemInfl = (ItemInfl)target;

        public override void OnInspectorGUI() => ShowEventEditor(_itemInfl);

        public static void ShowEventEditor(ItemInfl itemInfl)
        {
            GUILayout.Label("Принудительно использовать предмет");

            EditorGUILayout.BeginVertical("Box");

            // TODO : Код

            EditorGUILayout.EndVertical();
        }
    }
}

#endif