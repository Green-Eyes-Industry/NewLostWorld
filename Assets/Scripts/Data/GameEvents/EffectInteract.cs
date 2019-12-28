using Data.Characters;
using UnityEditor;
using UnityEngine;

namespace Data.GameEvents
{
    [CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Взаимодействие с эффектом")]
    public class EffectInteract : GameEvent
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
        public override GamePart FailPart() { return failPart; }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(EffectInteract))]
    public class EffectInteractGInspector : Editor
    {
        private EffectInteract _effectInteract;

        private void OnEnable() => _effectInteract = (EffectInteract)target;

        public override void OnInspectorGUI() => ShowEventEditor(_effectInteract);

        public static void ShowEventEditor(EffectInteract effectInteract)
        {
            GUILayout.Label("Получение или потеря эффекта");

            EditorGUILayout.BeginVertical("Box");

            // TODO : Получение или потеря эффекта

            EditorGUILayout.EndVertical();
        }
    }

#endif
}