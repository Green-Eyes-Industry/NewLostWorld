using UnityEngine;
using NLW.Parts;

#if UNITY_EDITOR
using UnityEditor;
using NLW.Data;
#endif

namespace NLW.Data
{
    public class RandomPart : GameEvent
    {
        /// <summary> Случайная глава </summary>
        public GamePart[] part_random = new GamePart[3];

        /// <summary> Вероятность попадания в главу </summary>
        public int[] randomChance = new int[3];

        /// <summary> Просчитать вероятность </summary>
        public GamePart Randomize(GamePart first, GamePart second, int chance)
        {
            int rand = Random.Range(0, 100);

            if (rand > chance) return second;
            else return first;
        }
    }
}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(RandomPart))]
    public class RandomPartGUI_Inspector : Editor
    {
        private RandomPart _randomPart;

        private void OnEnable() => _randomPart = (RandomPart)target;

        public override void OnInspectorGUI() => ShowEventEditor(_randomPart);

        public static void ShowEventEditor(RandomPart randomPart)
        {
            GUILayout.Label("Случайный переход");

            EditorGUILayout.BeginVertical("Box");

            for (int i = 0; i < randomPart.part_random.Length ; i++)
            {
                EditorGUILayout.BeginHorizontal();

                randomPart.part_random[i] = (GamePart)EditorGUILayout.ObjectField(randomPart.part_random[i], typeof(GamePart), true);

                if (randomPart.part_random[i] != null)
                {
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Отключить", GUILayout.Width(70))) randomPart.part_random[i] = null;
                }
                else
                {
                    GUI.backgroundColor = Color.green;
                    EditorGUILayout.LabelField("Пусто", GUILayout.Width(70));
                }
                EditorGUILayout.EndHorizontal();

                GUI.backgroundColor = Color.white;
                randomPart.randomChance[i] = EditorGUILayout.IntSlider(randomPart.randomChance[i], 0, 100);

                if(i < randomPart.part_random.Length - 1) EditorGUILayout.Space();
            }

            EditorGUILayout.EndVertical();
        }
    }
}

#endif