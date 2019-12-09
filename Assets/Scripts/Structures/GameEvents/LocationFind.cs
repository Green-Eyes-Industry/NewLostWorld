using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Найдена локация")]
public class LocationFind : GameEvent
{
    /// <summary> Найденная локация </summary>
    public MapMark _location;

    /// <summary> Найти локацию </summary>
    public override bool EventStart()
    {
        if(!DataController.playerData.playerMap.Contains(_location))
        {
            DataController.playerData.playerMap.Add(_location);
            DataController.SaveMap();
        }

        return true;
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(LocationFind))]
    public class LocationFindGUI_Inspector : Editor
    {
        private LocationFind _locationFind;

        private void OnEnable() => _locationFind = (LocationFind)target;

        public override void OnInspectorGUI() => ShowEventEditor(_locationFind);

        public static void ShowEventEditor(LocationFind locationFind)
        {
            GUILayout.Label("Найдена локация");

            EditorGUILayout.BeginVertical("Box");

            // Код

            EditorGUILayout.EndVertical();
        }
    }
}

#endif