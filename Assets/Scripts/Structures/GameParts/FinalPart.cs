using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New part", menuName = "Игровые обьекты/Новая глава/Финальная глава", order = 5)]
public class FinalPart : GamePart
{
    /// <summary> Получаемое достижение, если нужно </summary>
    public Achivemants newAchive;

    /// <summary> Текст кнопки возврата в меню </summary>
    public string backButtonText;
}

#if UNITY_EDITOR

namespace GUIInspector
{

    [CustomEditor(typeof(FinalPart))]
    public class FinalPartGUI_Inspector : Editor
    {
        private FinalPart _finalPart;

        private void OnEnable() => _finalPart = (FinalPart)target;

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Текстовые поля");

            GUILayout.BeginVertical("Box");

            _finalPart.mainText = EditorGUILayout.TextArea(_finalPart.mainText, GUILayout.Height(100));
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            _finalPart.backButtonText = EditorGUILayout.TextArea(_finalPart.backButtonText, GUILayout.Height(40));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            _finalPart.newAchive = (Achivemants)EditorGUILayout.ObjectField("Получаемое достижение :", _finalPart.newAchive, typeof(Achivemants), true);

            if (_finalPart.newAchive != null) AchivemantsGUI_Inspector.ShowAchiveGUI(_finalPart.newAchive);
        }
    }
}

#endif