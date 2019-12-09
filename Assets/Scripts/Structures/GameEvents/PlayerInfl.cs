using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Влияние на игрока")]
public class PlayerInfl : GameEvent
{
    /// <summary> Влияние на здоровье </summary>
    public int _healthInfl;

    /// <summary> Влияние на рассудок </summary>
    public int _mindInfl;

    /// <summary> Влияние </summary>
    public override bool EventStart()
    {
        DataController.playerData.playerHealth += _healthInfl;
        DataController.playerData.playerMind += _mindInfl;
        DataController.SaveCharacteristic();
        return true;
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(PlayerInfl))]
    public class PlayerInflGUI_Inspector : Editor
    {
        private PlayerInfl _playerInfl;

        private void OnEnable() => _playerInfl = (PlayerInfl)target;

        public override void OnInspectorGUI() => ShowEventEditor(_playerInfl);

        public static void ShowEventEditor(PlayerInfl _playerInfl)
        {
            GUILayout.Label("Влияние на характеристики игрока");

            EditorGUILayout.BeginVertical("Box");

            // Код

            EditorGUILayout.EndVertical();
        }
    }
}

#endif