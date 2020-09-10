using UnityEditor;
using UnityEngine;

namespace Data.GameEvents
{
    public class RandomPart : GameEvent
    {
        /// <summary> Случайная глава </summary>
        public GamePart[] partRandom = new GamePart[3];

        /// <summary> Вероятность попадания в главу </summary>
        public int[] randomChance = new int[3];

        /// <summary> Просчитать вероятность </summary>
        public GamePart Randomize(GamePart first, GamePart second, int chance)
        {
            int rand = Random.Range(0, 100);

            return rand > chance ? second : first;
        }

#if UNITY_EDITOR
        public override string GetPathToIco() => "Assets/Editor/NodeEditor/Images/EventsIco/CheckDecision.png";
#endif
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(RandomPart))]
    public class RandomPartGUInspector : Editor
    {
        private RandomPart _randomPart;

        private void OnEnable() => _randomPart = (RandomPart)target;

        public override void OnInspectorGUI() => ShowEventEditor(_randomPart);

        public static void ShowEventEditor(RandomPart randomPart)
        {
            GUILayout.Label("Случайный переход");

            EditorGUILayout.BeginVertical("Box");

            for (int i = 0; i < randomPart.partRandom.Length ; i++)
            {
                EditorGUILayout.BeginHorizontal();

                randomPart.partRandom[i] = (GamePart)EditorGUILayout.ObjectField(randomPart.partRandom[i], typeof(GamePart), true);

                if (randomPart.partRandom[i] != null)
                {
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Отключить", GUILayout.Width(70))) randomPart.partRandom[i] = null;
                }
                else
                {
                    GUI.backgroundColor = Color.green;
                    EditorGUILayout.LabelField("Пусто", GUILayout.Width(70));
                }
                EditorGUILayout.EndHorizontal();

                GUI.backgroundColor = Color.white;
                randomPart.randomChance[i] = EditorGUILayout.IntSlider(randomPart.randomChance[i], 0, 100);

                if(i < randomPart.partRandom.Length - 1) EditorGUILayout.Space();
            }

            EditorGUILayout.EndVertical();
        }
    }

#endif
}