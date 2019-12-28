using UnityEditor;
using UnityEngine;

namespace Data.GameEffects
{
    [CreateAssetMenu(fileName = "New positive effect", menuName = "Игровые обьекты/Новый игровой эффект/Положительный", order = 0)]
    public class PositiveEffect : GameEffect { }

#if UNITY_EDITOR

    [CustomEditor(typeof(PositiveEffect))]
    public class PositiveEffectGInspector : Editor
    {
        private PositiveEffect _positiveEffect;

        private void OnEnable() => _positiveEffect = (PositiveEffect)target;

        public override void OnInspectorGUI() => ShowPositiveEffectGUI(_positiveEffect);

        /// <summary> Показать редактор позитивного еффекта </summary>
        public static void ShowPositiveEffectGUI(PositiveEffect positiveEffect)
        {
            EditorGUILayout.LabelField("Позитивный эффект");

            EditorGUILayout.BeginHorizontal("Box");

            EditorGUILayout.BeginVertical();

            positiveEffect.durationEffect = EditorGUILayout.IntSlider("Длительность :", positiveEffect.durationEffect, 0, 50);
            EditorGUILayout.LabelField("Влияние");
            positiveEffect.healthInfluenceEffect = EditorGUILayout.IntSlider("На здоровье :", positiveEffect.healthInfluenceEffect, 0, 10);
            positiveEffect.mindInfluenceEffect = EditorGUILayout.IntSlider("На рассудок :", positiveEffect.mindInfluenceEffect, 0, 10);


            EditorGUILayout.EndVertical();

            positiveEffect.icoEffect = (Sprite)EditorGUILayout.ObjectField(positiveEffect.icoEffect, typeof(Sprite), true, GUILayout.Width(75), GUILayout.Height(75));

            EditorGUILayout.EndHorizontal();
        }
    }

#endif
}