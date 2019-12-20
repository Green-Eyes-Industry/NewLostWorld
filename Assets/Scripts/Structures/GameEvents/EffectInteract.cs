using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Data;
#endif

namespace NLW.Data
{
    [CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Взаимодействие с эффектом")]
    public class EffectInteract : GameEvent
    {
        /// <summary> Эффект </summary>
        public GameEffect gameEffect;

        /// <summary> Получение или потеря </summary>
        public bool isAddOrRemove;

        /// <summary> Глава при провале </summary>
        public Parts.GamePart _failPart;

        /// <summary> Взаимодействие </summary>
        public override bool EventStart()
        {
            Player mPlayer = MainController.Instance.mainPlayer;

            if (isAddOrRemove)
            {
                if (!mPlayer.playerEffects.Contains(gameEffect)) mPlayer.playerEffects.Add(gameEffect);
                return true;
            }
            else
            {
                if (mPlayer.playerEffects.Contains(gameEffect))
                {
                    mPlayer.playerEffects.Remove(gameEffect);
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
    [CustomEditor(typeof(EffectInteract))]
    public class EffectInteractGUI_Inspector : Editor
    {
        private EffectInteract _effectInteract;

        private void OnEnable() => _effectInteract = (EffectInteract)target;

        public override void OnInspectorGUI() => ShowEventEditor(_effectInteract);

        public static void ShowEventEditor(EffectInteract effectInteract)
        {
            GUILayout.Label("Получение или потеря еффекта");

            EditorGUILayout.BeginVertical("Box");

            // Код

            EditorGUILayout.EndVertical();
        }
    }
}

#endif