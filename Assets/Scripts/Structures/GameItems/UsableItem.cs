using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Data;
#endif

namespace NLW.Data
{
    [CreateAssetMenu(fileName = "New item", menuName = "Игровые обьекты/Новый предмет/Активный")]
    public class UsableItem : GameItem
    {
        /// <summary> Влияние на здоровье </summary>
        public int healthInf;

        /// <summary> Влияние на сознание </summary>
        public int mindInf;

        /// <summary> Накладываемый эффект </summary>
        public GameEffect itemEffect;

        /// <summary> Использовать </summary>
        public void UseThisItem()
        {
            Player mPlayer = MainController.Instance.dataController.mainPlayer;

            mPlayer.playerHealth += healthInf;
            mPlayer.playerMind += mindInf;

            if (!mPlayer.playerEffects.Contains(itemEffect))
            {
                mPlayer.playerEffects.Add(itemEffect);
            }
        }
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{

    [CustomEditor(typeof(UsableItem))]
    public class UsableItemGUI_Inspector : Editor
    {
        private UsableItem _usableItem;

        private void OnEnable() => _usableItem = (UsableItem)target;

        public override void OnInspectorGUI() => ShowItemEditor(_usableItem);

        /// <summary> Показать редактор активного предмета </summary>
        public static void ShowItemEditor(UsableItem usableItem)
        {
            GUILayout.Label("Активный предмет");

            GUILayout.BeginHorizontal("Box");

            GUILayout.BeginVertical();

            usableItem.itemName = EditorGUILayout.TextField("Название :", usableItem.itemName);
            GUILayout.Label("Описание :");
            usableItem.itemDescript = EditorGUILayout.TextArea(usableItem.itemDescript, GUILayout.Height(40));

            GUILayout.EndVertical();

            usableItem.itemIco = (Sprite)EditorGUILayout.ObjectField(usableItem.itemIco, typeof(Sprite), true, GUILayout.Height(75), GUILayout.Width(75));

            GUILayout.EndHorizontal();

            GUILayout.Label("Влияние");

            GUILayout.BeginVertical("Box");

            usableItem.healthInf = EditorGUILayout.IntSlider("Здоровье :", usableItem.healthInf, -100, 100);
            usableItem.mindInf = EditorGUILayout.IntSlider("Рассудок :", usableItem.mindInf, -100, 100);

            usableItem.itemEffect = (GameEffect)EditorGUILayout.ObjectField("Накладываемый эффект :", usableItem.itemEffect, typeof(GameEffect), true);

            if (usableItem.itemEffect != null) GlobalHelperGUI_Inspector.ShowEffectFromPart(usableItem.itemEffect);

            GUILayout.EndVertical();
        }
    }
}

#endif