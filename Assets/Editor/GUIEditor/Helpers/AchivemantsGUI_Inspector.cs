using UnityEngine;
using UnityEditor;

namespace GUIInspector
{

    [CustomEditor(typeof(Achivemants))]
    public class AchivemantsGUI_Inspector : Editor
    {
        private Achivemants _achivemants;

        private void OnEnable() => _achivemants = (Achivemants)target;

        public override void OnInspectorGUI() => ShowAchiveGUI(_achivemants);

        /// <summary>
        /// Показать редактор достижения
        /// </summary>
        public static void ShowAchiveGUI(Achivemants achivemants)
        {
            GUILayout.Label("Достижение");

            GUILayout.BeginHorizontal("Box");

            GUILayout.BeginVertical();

            achivemants.achiveName = EditorGUILayout.TextField("Название :", achivemants.achiveName);
            GUILayout.Label("Описание :");
            achivemants.achiveDescript = EditorGUILayout.TextArea(achivemants.achiveDescript, GUILayout.Height(40));

            GUILayout.EndVertical();

            achivemants.achiveIco = (Sprite)EditorGUILayout.ObjectField(achivemants.achiveIco, typeof(Sprite), true, GUILayout.Height(75), GUILayout.Width(75));

            GUILayout.EndHorizontal();
        }
    }
}