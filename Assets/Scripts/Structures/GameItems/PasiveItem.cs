using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Data;
#endif

namespace NLW.Data
{
    [CreateAssetMenu(fileName = "New item", menuName = "Игровые обьекты/Новый предмет/Пасивный")]
    public class PasiveItem : GameItem
    {
        /// <summary> Накладываемый эффект пока предмет у персонажа </summary>
        public GameEffect itemEffect;
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{

    [CustomEditor(typeof(PasiveItem))]
    public class PasiveItemGUI_Inspector : Editor
    {
        private PasiveItem _pasiveItem;

        private void OnEnable() => _pasiveItem = (PasiveItem)target;

        public override void OnInspectorGUI() => ShowItemEditor(_pasiveItem);

        /// <summary> Показать редактор пассивного предмета </summary>
        public static void ShowItemEditor(PasiveItem pasiveItem)
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
        }
    }
}

#endif