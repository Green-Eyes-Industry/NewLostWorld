using UnityEditor;
using UnityEngine;

namespace Data.GameEffects
{
    [CreateAssetMenu(fileName = "New negative effect", menuName = "Игровые обьекты/Новый игровой эффект/Отрицательный", order = 1)]
    public class NegativeEffect : GameEffect { }

#if UNITY_EDITOR

    [CustomEditor(typeof(NegativeEffect))]
    public class NegativeEffectGUInspector : Editor
    {
        private NegativeEffect _negativeEffect;

        private void OnEnable() => _negativeEffect = (NegativeEffect)target;

        public override void OnInspectorGUI() => ShowNegativeEffectGUI(_negativeEffect);

        /// <summary> Показать редактор негативного еффекта </summary>
        public static void ShowNegativeEffectGUI(NegativeEffect negativeEffect)
        {
            EditorGUILayout.LabelField("Негативный эффект");

            EditorGUILayout.BeginHorizontal("Box");

            EditorGUILayout.BeginVertical();

            negativeEffect.nameEffect = EditorGUILayout.TextField("Название :", negativeEffect.nameEffect);
            negativeEffect.durationEffect = EditorGUILayout.IntSlider("Длительность :", negativeEffect.durationEffect, 0, 100);
            EditorGUILayout.LabelField("Снижение");
            negativeEffect.healthInfluenceEffect = EditorGUILayout.IntSlider("Здоровья :", negativeEffect.healthInfluenceEffect, 0, 20);
            negativeEffect.mindInfluenceEffect = EditorGUILayout.IntSlider("Рассудка :", negativeEffect.mindInfluenceEffect, 0, 20);

            EditorGUILayout.EndVertical();

            negativeEffect.icoEffect = (Sprite)EditorGUILayout.ObjectField(negativeEffect.icoEffect, typeof(Sprite), true, GUILayout.Width(75), GUILayout.Height(75));

            EditorGUILayout.EndHorizontal();
        }
    }

#endif
}