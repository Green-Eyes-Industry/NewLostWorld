using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Взаимодействие с эффектом")]
public class EffectInteract : GameEvent
{
    /// <summary> Эффект </summary>
    public GameEffect gameEffect;

    /// <summary> Получение или потеря </summary>
    public bool isAddOrRemove;

    /// <summary> Глава при провале </summary>
    public GamePart _failPart;

    /// <summary> Взаимодействие </summary>
    public override bool EventStart()
    {
        if (isAddOrRemove)
        {
            if (!DataController.playerData.playerEffects.Contains(gameEffect))
            {
                DataController.playerData.playerEffects.Add(gameEffect);
                DataController.SaveEffects();
            }
            return true;
        }
        else
        {
            if (DataController.playerData.playerEffects.Contains(gameEffect))
            {
                DataController.playerData.playerEffects.Remove(gameEffect);
                DataController.SaveEffects();
                return true;
            }
            else return false;
        }
    }

    /// <summary> Вернуть главу провала </summary>
    public override GamePart FailPart() { return _failPart; }
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