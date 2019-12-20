using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Data;
#endif

namespace NLW.Data
{
    public class LocationFind : GameEvent
    {
        /// <summary> Найденная локация </summary>
        public MapMark _location;

        /// <summary> Найти локацию </summary>
        public override bool EventStart()
        {
            Player mPlayer = MainController.Instance.mainPlayer;

            if (!mPlayer.playerMap.Contains(_location)) mPlayer.playerMap.Add(_location);

            return true;
        }
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(LocationFind))]
    public class LocationFindGUI_Inspector : Editor
    {
        private LocationFind _locationFind;
        public static int id = 0;

        private void OnEnable() => _locationFind = (LocationFind)target;

        public override void OnInspectorGUI() => ShowEventEditor(_locationFind);

        public static void ShowEventEditor(LocationFind locationFind)
        {
            GUILayout.Label("Найдена локация");

            EditorGUILayout.BeginVertical("Box");

            object[] allLocations = Resources.LoadAll("Locations/", typeof(MapMark));

            string[] names = new string[allLocations.Length];

            MapMark nameConvert;

            for (int i = 0; i < names.Length; i++)
            {
                nameConvert = (MapMark)allLocations[i];

                if (nameConvert.nameLocation == "") names[i] = nameConvert.name;
                else names[i] = nameConvert.nameLocation;
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));

            if (allLocations.Length > 0)
            {
                id = EditorGUILayout.Popup(id, names);
                locationFind._location = (MapMark)allLocations[id];
            }
            else GUILayout.Label("Нет локаций");

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Создать", GUILayout.Width(70))) AssetDatabase.CreateAsset(CreateInstance(typeof(MapMark)),
                  "Assets/Resources/Locations/" + allLocations.Length + "_Location.asset");

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
            if (locationFind._location != null) MapMarkGUI_Inspector.ShowItemEditor(locationFind._location);


            EditorGUILayout.EndVertical();
        }
    }
}

#endif