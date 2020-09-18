using Controllers;
using Data.Characters;
using UnityEditor;
using UnityEngine;
using Data.GameEffects;

namespace Data.GameEvents
{
    [CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Взаимодействие с эффектом")]
    public class EffectInteract : GameEvent, IMessage
    {
        /// <summary> Эффект </summary>
        public GameEffect gameEffect;

        /// <summary> Получение или потеря </summary>
        public bool isAddOrRemove;

        /// <summary> Глава при провале </summary>
        public GamePart failPart;

        /// <summary> Взаимодействие </summary>
        public override bool EventStart()
        {
            Player mPlayer = MainController.instance.dataController.mainPlayer;

            MainController.instance.effectsController.ShowMessage(this);
            
            if (isAddOrRemove && !mPlayer.playerEffects.Contains(gameEffect)) mPlayer.playerEffects.Add(gameEffect);
            else if (!mPlayer.playerEffects.Contains(gameEffect)) return false;
            else mPlayer.playerEffects.Remove(gameEffect);
            
            return true;
        }

        /// <summary> Вернуть главу провала </summary>
        public override GamePart FailPart() { return failPart; }

        public string GetText()
        {
            Player mPlayer = MainController.instance.dataController.mainPlayer;
            return ((isAddOrRemove && !mPlayer.playerEffects.Contains(gameEffect)) ? "Получен еффект\n" : "Снят еффект\n") + gameEffect.nameEffect;
        }

        public AnimController.MessangeType GetAnimationType() => AnimController.MessangeType.EFFECT_MS;
        
#if UNITY_EDITOR
        public int id;
#endif
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(EffectInteract))]
    public class EffectInteractGUInspector : Editor
    {
        private EffectInteract _effectInteract;

        private void OnEnable() => _effectInteract = (EffectInteract)target;

        public override void OnInspectorGUI() => ShowEventEditor(_effectInteract);

        public static void ShowEventEditor(EffectInteract effectInteract)
        {
            GUILayout.Label("Получение или потеря эффекта");

            EditorGUILayout.BeginVertical("Box");

            Object[] allItems = Resources.LoadAll("PlayerEffects/", typeof(GameEffect));

            string[] names = new string[allItems.Length];

            for (int i = 0; i < names.Length; i++)
            {
                GameEffect nameConvert = (GameEffect)allItems[i];
                names[i] = (nameConvert.nameEffect == "") ? nameConvert.name : nameConvert.nameEffect;
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));

            if (allItems.Length > 0)
            {
                effectInteract.id = EditorGUILayout.Popup(effectInteract.id, names);
                effectInteract.gameEffect = (GameEffect)allItems[effectInteract.id];
                effectInteract.isAddOrRemove = EditorGUILayout.Toggle(effectInteract.isAddOrRemove, GUILayout.Width(20));
            }
            else GUILayout.Label("Нет предметов");

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Positive", GUILayout.Width(70))) AssetDatabase.CreateAsset(CreateInstance(typeof(PositiveEffect)),
                "Assets/Resources/PlayerEffects/" + allItems.Length + "_PositiveEffect.asset");
            if (GUILayout.Button("Negative", GUILayout.Width(70))) AssetDatabase.CreateAsset(CreateInstance(typeof(NegativeEffect)),
                "Assets/Resources/PlayerEffects/" + allItems.Length + "_NegativeEffect.asset");

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
            if (!effectInteract.isAddOrRemove)
                effectInteract.failPart = (GamePart)EditorGUILayout.ObjectField("Глава провала : ", effectInteract.failPart, typeof(GamePart), true);

            if (effectInteract.gameEffect != null)
            {
                if (effectInteract.gameEffect is PositiveEffect positiveEffect) PositiveEffectGUInspector.ShowPositiveEffectGUI(positiveEffect);
                else if (effectInteract.gameEffect is NegativeEffect negativeEffect) NegativeEffectGUInspector.ShowNegativeEffectGUI(negativeEffect);
            }

            EditorGUILayout.EndVertical();
        }
    }

#endif
}