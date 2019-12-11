using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
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
        private List<Achivemants> _achivemants;
        private string[] _newAchiveNames;
        private Achivemants[] _newAchiveFiles;
        private int _newAchiveIndex;
        private int _memIndex;

        private void OnEnable()
        {
            _memIndex = 999;
            _finalPart = (FinalPart)target;
            _achivemants = new List<Achivemants>();
            ReloadAchives();
        }

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

            EditorGUILayout.BeginHorizontal();

            _newAchiveIndex = EditorGUILayout.Popup(_newAchiveIndex, _newAchiveNames);

            if(_memIndex != _newAchiveIndex)
            {
                _memIndex = _newAchiveIndex;
                _finalPart.newAchive = _newAchiveFiles[_memIndex];
            }
            
            if (_finalPart.newAchive != null)
            {
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Создать", GUILayout.Width(70)))
                {
                    string nameFile = _newAchiveFiles.Length + "_Achive";
                    AssetDatabase.CreateAsset(CreateInstance(typeof(Achivemants)), "Assets/Resources/Achivemants/" + nameFile + ".asset");
                    ReloadAchives();
                }

                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Удалить", GUILayout.Width(70)))
                {
                    AssetDatabase.DeleteAsset("Assets/Resources/Achivemants/" + _finalPart.newAchive.name + ".asset");
                    ReloadAchives();
                }
            }
            else
            {
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Создать", GUILayout.Width(70)))
                {
                    string nameFile = _newAchiveFiles.Length + "_Achive";
                    AssetDatabase.CreateAsset(CreateInstance(typeof(Achivemants)), "Assets/Resources/Achivemants/" + nameFile + ".asset");
                    _finalPart.newAchive = (Achivemants)Resources.Load("Achivemants/" + nameFile, typeof(Achivemants));
                    ReloadAchives();
                }
            }
            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
            if (_finalPart.newAchive != null) AchivemantsGUI_Inspector.ShowAchiveGUI(_finalPart.newAchive);
        }

        /// <summary> Перезагрузить список достижений </summary>
        private void ReloadAchives()
        {
            Object[] obj = Resources.LoadAll("Achivemants", typeof(Achivemants));

            List<string> _newAchiveList = new List<string>();
            List<Achivemants> _newAchiveFileList = new List<Achivemants>();

            for (int i = 0; i < obj.Length; i++)
            {
                _achivemants.Add((Achivemants)obj[i]);
                _newAchiveList.Add(_achivemants[i].achiveName);
                _newAchiveFileList.Add(_achivemants[i]);
            }

            _newAchiveNames = _newAchiveList.ToArray();
            _newAchiveFiles = _newAchiveFileList.ToArray();
        }
    }
}

#endif