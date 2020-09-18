using Controllers;
using Data.Characters;
using Helpers;
using UnityEditor;
using UnityEngine;

namespace Data.GameEvents
{
    public class LocationFind : GameEvent, IMessage
    {
        /// <summary> Найденная локация </summary>
        public MapMark location;

        /// <summary> Найти локацию </summary>
        public override bool EventStart()
        {
            Player mPlayer = MainController.instance.dataController.mainPlayer;

            if (!mPlayer.playerMap.Contains(location))
            {
                MainController.instance.effectsController.ShowMessage(this);
                mPlayer.playerMap.Add(location);
            }

            return true;
        }

        public string GetText() => "Найдена локация\n" + location.nameLocation;

        public AnimController.MessangeType GetAnimationType() => AnimController.MessangeType.MAP_MS;
        
#if UNITY_EDITOR
        public int id;
#endif
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(LocationFind))]
    public class LocationFindGUInspector : Editor
    {
        private LocationFind _locationFind;

        private void OnEnable() => _locationFind = (LocationFind)target;

        public override void OnInspectorGUI() => ShowEventEditor(_locationFind);

        public static void ShowEventEditor(LocationFind locationFind)
        {
            GUILayout.Label("Найдена локация");

            EditorGUILayout.BeginVertical("Box");

            Object[] allLocations = Resources.LoadAll("Locations/", typeof(MapMark));

            string[] names = new string[allLocations.Length];

            for (int i = 0; i < names.Length; i++)
            {
                MapMark nameConvert = (MapMark)allLocations[i];

                if (nameConvert.nameLocation == "") names[i] = nameConvert.name;
                else names[i] = nameConvert.nameLocation;
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));

            if (allLocations.Length > 0)
            {
                locationFind.id = EditorGUILayout.Popup(locationFind.id, names);
                locationFind.location = (MapMark)allLocations[locationFind.id];
            }
            else GUILayout.Label("Нет локаций");

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Создать", GUILayout.Width(70))) AssetDatabase.CreateAsset(CreateInstance(typeof(MapMark)),
                "Assets/Resources/Locations/" + allLocations.Length + "_Location.asset");

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
            if (locationFind.location != null) MapMarkGUInspector.ShowItemEditor(locationFind.location);


            EditorGUILayout.EndVertical();
        }
    }

#endif
}