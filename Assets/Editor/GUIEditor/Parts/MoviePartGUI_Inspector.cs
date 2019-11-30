using UnityEngine;
using UnityEditor;

namespace GUIInspector
{

    [CustomEditor(typeof(MoviePart))]
    public class MoviePartGUI_Inspector : Editor
    {
        private MoviePart _moviePart;
        private Vector2 _slidesSlider = Vector2.zero;

        private void OnEnable() => _moviePart = (MoviePart)target;

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Список слайдов");

            GUILayout.BeginScrollView(_slidesSlider, "Box");

            if (_moviePart.movieSprites.Count > 0)
            {
                for (int i = 0; i < _moviePart.movieSprites.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    _moviePart.movieSprites[i] = (Sprite)EditorGUILayout.ObjectField(_moviePart.movieSprites[i], typeof(Sprite), true);
                    if (GUILayout.Button("Удалить", GUILayout.Width(70))) _moviePart.movieSprites.RemoveAt(i);

                    GUILayout.EndHorizontal();
                }
            }
            else GUILayout.Label("Нет слайдов");

            GUILayout.EndScrollView();

            if (GUILayout.Button("Добавить слайд", GUILayout.Height(30))) _moviePart.movieSprites.Add(null);

            if (GUILayout.Button("Сохранить", GUILayout.Height(30))) EditorUtility.SetDirty(_moviePart);
        }
    }
}