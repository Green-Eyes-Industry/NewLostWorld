using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New event", menuName = "Игровые обьекты/Новый эвент/Случайный переход")]
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

            // Код

            EditorGUILayout.EndVertical();
        }
    }
}

#endif