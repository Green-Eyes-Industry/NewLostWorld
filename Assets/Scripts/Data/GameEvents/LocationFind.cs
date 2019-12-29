using Data.Characters;
using Helpers;
using UnityEditor;
using UnityEngine;

namespace Data.GameEvents
{
    public class LocationFind : GameEvent
    {
        /// <summary> Найденная локация </summary>
        public MapMark location;

        /// <summary> Найти локацию </summary>
        public override bool EventStart()
        {
            Player mPlayer = MainController.instance.dataController.mainPlayer;

            if (!mPlayer.playerMap.Contains(location))
            {
                MainController.instance.effectsController.AddMapMarkMessage(location);
                mPlayer.playerMap.Add(location);
            }

            return true;
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(LocationFind))]
    public class LocationFindGInspector : Editor
    {
        private LocationFind _locationFind;
        private static int _id = 0;

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
                _id = EditorGUILayout.Popup(_id, names);
                locationFind.location = (MapMark)allLocations[_id];
            }
            else GUILayout.Label("Нет локаций");

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Создать", GUILayout.Width(70))) AssetDatabase.CreateAsset(CreateInstance(typeof(MapMark)),
                "Assets/Resources/Locations/" + allLocations.Length + "_Location.asset");

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
            if (locationFind.location != null) MapMarkGInspector.ShowItemEditor(locationFind.location);


            EditorGUILayout.EndVertical();
        }
    }

#endif
}