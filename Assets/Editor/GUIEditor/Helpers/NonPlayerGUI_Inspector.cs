using UnityEditor;
using UnityEngine;

namespace GUIInspector
{

    [CustomEditor(typeof(NonPlayer))]
    public class NonPlayerGUI_Inspector : Editor
    {
        private NonPlayer _nonPlayer;

        private void OnEnable() => _nonPlayer = (NonPlayer)target;

        public override void OnInspectorGUI() => ShowNonPlayerGUI(_nonPlayer);

        /// <summary> Показать мено редактора не игрового персонажа </summary>
        public static void ShowNonPlayerGUI(NonPlayer nonPlayer)
        {
            EditorGUILayout.LabelField("Не игровой персонаж");

            EditorGUILayout.BeginVertical("Box");

            nonPlayer.npName = EditorGUILayout.TextField("Имя персонажа", nonPlayer.npName);
            nonPlayer.npToPlayerRatio = EditorGUILayout.IntSlider(nonPlayer.npToPlayerRatio, -10, 10);

            EditorGUILayout.EndVertical();
        }
    }
}