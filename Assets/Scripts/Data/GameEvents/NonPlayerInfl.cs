using Data.Characters;
using UnityEditor;
using UnityEngine;

namespace Data.GameEvents
{
    [CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Влияние на НПС")]
    public class NonPlayerInfl : GameEvent
    {
        /// <summary> На какого персонажа </summary>
        public NonPlayer nonPlayer;

        /// <summary> Влияние </summary>
        public int value;

        /// <summary> Старт события </summary>
        public override bool EventStart()
        {
            nonPlayer.npToPlayerRatio += value;

            if (nonPlayer.npToPlayerRatio < -10) nonPlayer.npToPlayerRatio = -10;
            else if (nonPlayer.npToPlayerRatio > 10) nonPlayer.npToPlayerRatio = 10;

            if (!MainController.instance.dataController.mainPlayer.playerMeet.Contains(nonPlayer)) MainController.instance.dataController.mainPlayer.playerMeet.Add(nonPlayer);
            return true;
        }

#if UNITY_EDITOR
        public int id;
#endif

    }

#if UNITY_EDITOR

    [CustomEditor(typeof(NonPlayerInfl))]
    public class NonPlayerInflGUInspector : Editor
    {
        private NonPlayerInfl _nonPlayerInfl;

        private void OnEnable() => _nonPlayerInfl = (NonPlayerInfl)target;

        public override void OnInspectorGUI() => ShowEventEditor(_nonPlayerInfl);

        public static void ShowEventEditor(NonPlayerInfl nonPlayerInfl)
        {
            GUILayout.Label("Влияние на НПС");

            EditorGUILayout.BeginVertical("Box");

            Object[] allItems = Resources.LoadAll("Players/NonPlayers/", typeof(NonPlayer));

            string[] names = new string[allItems.Length];

            for (int i = 0; i < names.Length; i++)
            {
                NonPlayer nameConvert = (NonPlayer)allItems[i];

                names[i] = (nameConvert.npName == "") ? nameConvert.name : nameConvert.npName;
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));

            if (allItems.Length > 0)
            {
                nonPlayerInfl.id = EditorGUILayout.Popup(nonPlayerInfl.id, names);
                nonPlayerInfl.nonPlayer = (NonPlayer)allItems[nonPlayerInfl.id];
            }
            else GUILayout.Label("Нет НПС персонажей");

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Создать", GUILayout.Width(70))) AssetDatabase.CreateAsset(CreateInstance(typeof(NonPlayer)),
                "Assets/Resources/Players/NonPlayers/" + allItems.Length + "_NonPlayer.asset");

            EditorGUILayout.EndHorizontal();

            if (nonPlayerInfl.value < 0) GUI.backgroundColor = Color.red;
            else if (nonPlayerInfl.value > 0) GUI.backgroundColor = Color.green;
            else GUI.backgroundColor = Color.white;

            nonPlayerInfl.value = EditorGUILayout.IntSlider("Влияние :", nonPlayerInfl.value, -10, 10);

            GUI.backgroundColor = Color.white;
            if (nonPlayerInfl.nonPlayer != null) NonPlayerGUInspector.ShowNonPlayerGUI(nonPlayerInfl.nonPlayer);

            EditorGUILayout.EndVertical();
        }
    }

#endif
}