using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "new location", menuName = "Игровые обьекты/Метка на карте")]
public class MapMark : ScriptableObject
{
    /// <summary> Название локации </summary>
    public string nameLocation;

    /// <summary> Лор и описание локации </summary>
    public string loreLocation;

    /// <summary> Глава для перемещения </summary>
    public GamePart _partLocation;
}

#if UNITY_EDITOR

namespace GUIInspector
{

    [CustomEditor(typeof(MapMark))]
    public class MapMarkGUI_Inspector : Editor
    {
        private MapMark _mapMark;

        private void OnEnable() => _mapMark = (MapMark)target;

        public override void OnInspectorGUI() => ShowItemEditor(_mapMark);

        /// <summary> Отобразить в редакторе </summary>
        public static void ShowItemEditor(MapMark mapMark)
        {
            EditorGUILayout.LabelField("Метка на карте");

            GUILayout.BeginVertical("Box");

            mapMark.nameLocation = EditorGUILayout.TextField("Название локации :", mapMark.nameLocation);
            EditorGUILayout.LabelField("Лор локации");
            mapMark.loreLocation = EditorGUILayout.TextArea(mapMark.loreLocation, GUILayout.Height(40));
            EditorGUILayout.Space();
            mapMark._partLocation = (GamePart)EditorGUILayout.ObjectField("Глава локации :", mapMark._partLocation, typeof(GamePart), true);

            GUILayout.EndVertical();
        }
    }
}

#endif