using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Использование предмета")]
public class ItemInfl : GameEvent
{
    /// <summary> Уничтожить в процессе </summary>
    public bool isRemove;

    /// <summary> Предмет </summary>
    public UsableItem useItem;

    /// <summary> Глава при провале </summary>
    public GamePart _failPart;

    /// <summary> Влияние </summary>
    public override bool EventStart()
    {
        if (DataController.playerData.playerInventory.Contains(useItem))
        {
            useItem.UseThisItem();
            if (isRemove) DataController.playerData.playerInventory.Remove(useItem);
            return true;
        }
        else return false;
    }


    /// <summary> Вернуть главу провала </summary>
    public override GamePart FailPart() { return _failPart; }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(ItemInfl))]
    public class ItemInflGUI_Inspector : Editor
    {
        private ItemInfl _itemInfl;

        private void OnEnable() => _itemInfl = (ItemInfl)target;

        public override void OnInspectorGUI() => ShowEventEditor(_itemInfl);

        public static void ShowEventEditor(ItemInfl itemInfl)
        {
            GUILayout.Label("Принудительно использовать предмет");

            EditorGUILayout.BeginVertical("Box");

            // Код

            EditorGUILayout.EndVertical();
        }
    }
}

#endif