using UnityEditor;
using UnityEngine;

namespace Data.GameEffects
{
    [CreateAssetMenu(fileName = "New positive effect", menuName = "Игровые обьекты/Новый игровой эффект/Положительный", order = 0)]
    public class PositiveEffect : GameEffect { }

#if UNITY_EDITOR

    [CustomEditor(typeof(PositiveEffect))]
    public class PositiveEffectGUInspector : Editor
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

            positiveEffect.nameEffect = EditorGUILayout.TextField("Название :", positiveEffect.nameEffect);
            positiveEffect.durationEffect = EditorGUILayout.IntSlider("Длительность :", positiveEffect.durationEffect, 0, 100);
            EditorGUILayout.LabelField("Увеличение");
            positiveEffect.healthInfluenceEffect = EditorGUILayout.IntSlider("Здоровья :", positiveEffect.healthInfluenceEffect, 0, 20);
            positiveEffect.mindInfluenceEffect = EditorGUILayout.IntSlider("Рассудка :", positiveEffect.mindInfluenceEffect, 0, 20);

            EditorGUILayout.EndVertical();

            positiveEffect.icoEffect = (Sprite)EditorGUILayout.ObjectField(positiveEffect.icoEffect, typeof(Sprite), true, GUILayout.Width(75), GUILayout.Height(75));

            EditorGUILayout.EndHorizontal();
        }
    }

#endif
}