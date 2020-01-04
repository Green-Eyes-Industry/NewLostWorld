using Data.Characters;
using UnityEditor;
using UnityEngine;

namespace Data.GameEvents
{
    public class CheckPlayerInfl : GameEvent
    {
        /// <summary> Non Player </summary>
        public NonPlayer nonPlayer;

        /// <summary> Inflation </summary>
        public int value;

        /// <summary> Fail part </summary>
        public GamePart failPart;

        /// <summary> Check inflation </summary>
        /// <returns> Fail part </returns>
        public override bool EventStart()
        {
            return nonPlayer.npToPlayerRatio >= value;
        }

        /// <summary> Return false part </summary>
        public override GamePart FailPart() { return failPart; }

#if UNITY_EDITOR
        public int id;
#endif
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(CheckPlayerInfl))]
    public class CheckPlayerInflGUInspector : Editor
    {
        private CheckPlayerInfl _checkPlayerInfl;

        private void OnEnable() => _checkPlayerInfl = (CheckPlayerInfl)target;

        public override void OnInspectorGUI() => ShowEventEditor(_checkPlayerInfl);

        public static void ShowEventEditor(CheckPlayerInfl checkPlayerInfl)
        {
            GUILayout.Label("Проверка влияния персонажа на НПС");

            EditorGUILayout.BeginVertical("Box");

            Object[] allItems = Resources.LoadAll("Players/NonPlayers/", typeof(NonPlayer));

            string[] names = new string[allItems.Length];

            for (int i = 0; i < names.Length; i++)
            {
                NonPlayer nameConvert = (NonPlayer)allItems[i];

                if (nameConvert.npName == "") names[i] = nameConvert.name;
                else names[i] = nameConvert.npName;
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));

            if (allItems.Length > 0)
            {
                checkPlayerInfl.id = EditorGUILayout.Popup(checkPlayerInfl.id, names);
                checkPlayerInfl.nonPlayer = (NonPlayer)allItems[checkPlayerInfl.id];
            }
            else GUILayout.Label("Нет НПС персонажей");

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Создать", GUILayout.Width(70))) AssetDatabase.CreateAsset(CreateInstance(typeof(NonPlayer)),
                "Assets/Resources/Players/NonPlayers/" + allItems.Length + "_NonPlayer.asset");

            EditorGUILayout.EndHorizontal();

            if (checkPlayerInfl.value < 0) GUI.backgroundColor = Color.red;
            else if (checkPlayerInfl.value > 0) GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.white;

            checkPlayerInfl.value = EditorGUILayout.IntSlider("Требуемые отношения :", checkPlayerInfl.value, -10, 10);

            GUI.backgroundColor = Color.white;
            checkPlayerInfl.failPart = (GamePart)EditorGUILayout.ObjectField("Глава провала :",checkPlayerInfl.failPart, typeof(GamePart), true);

            if (checkPlayerInfl.nonPlayer != null) NonPlayerGUInspector.ShowNonPlayerGUI(checkPlayerInfl.nonPlayer);

            EditorGUILayout.EndVertical();
        }
    }

#endif
}