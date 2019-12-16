using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RandomPart : GameEvent
{
    /// <summary> Случайная глава 1 </summary>
    public GamePart part_1_random;

    /// <summary> Случайная глава 2 </summary>
    public GamePart part_2_random;

    /// <summary> Случайная глава 3 </summary>
    public GamePart part_3_random;

    /// <summary> Вероятность попадания в главу 1 </summary>
    public int randomChance_1;

    /// <summary> Вероятность попадания в главу 2 </summary>
    public int randomChance_2;

    /// <summary> Вероятность попадания в главу 3 </summary>
    public int randomChance_3;

    /// <summary> Просчитать вероятность </summary>
    public GamePart Randomize(GamePart first, GamePart second, int chance)
    {
        int rand = Random.Range(0, 100);

        if (rand > chance) return second;
        else return first;
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

            // Первая кнопка

            EditorGUILayout.BeginHorizontal();

            randomPart.part_1_random = (GamePart)EditorGUILayout.ObjectField(randomPart.part_1_random,typeof(GamePart),true);

            if(randomPart.part_1_random != null)
            {
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Отключить", GUILayout.Width(70))) randomPart.part_1_random = null;
            }
            else
            {
                GUI.backgroundColor = Color.green;
                EditorGUILayout.LabelField("Пусто", GUILayout.Width(70));
            }
            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
            randomPart.randomChance_1 = EditorGUILayout.IntSlider(randomPart.randomChance_1, 0, 100);

            EditorGUILayout.Space();

            // Вторая кнопка

            EditorGUILayout.BeginHorizontal();

            randomPart.part_2_random = (GamePart)EditorGUILayout.ObjectField(randomPart.part_2_random, typeof(GamePart), true);

            if (randomPart.part_2_random != null)
            {
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Отключить", GUILayout.Width(70))) randomPart.part_2_random = null;
            }
            else
            {
                GUI.backgroundColor = Color.green;
                EditorGUILayout.LabelField("Пусто", GUILayout.Width(70));
            }
            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
            randomPart.randomChance_2 = EditorGUILayout.IntSlider(randomPart.randomChance_2, 0, 100);

            EditorGUILayout.Space();

            // Третья кнопка

            EditorGUILayout.BeginHorizontal();

            randomPart.part_3_random = (GamePart)EditorGUILayout.ObjectField(randomPart.part_3_random, typeof(GamePart), true);

            if (randomPart.part_3_random != null)
            {
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Отключить", GUILayout.Width(70))) randomPart.part_3_random = null;
            }
            else
            {
                GUI.backgroundColor = Color.green;
                EditorGUILayout.LabelField("Пусто", GUILayout.Width(70));
            }
            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
            randomPart.randomChance_3 = EditorGUILayout.IntSlider(randomPart.randomChance_3, 0, 100);

            EditorGUILayout.EndVertical();
        }
    }
}

#endif