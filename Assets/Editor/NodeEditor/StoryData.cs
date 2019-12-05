using System.Collections.Generic;
using UnityEngine;

namespace GUIInspector.NodeEditor
{
    [CreateAssetMenu(fileName = "New story data", menuName = "Сюжет")]
    public class StoryData : ScriptableObject
    {
        /// <summary> Описание сюжета </summary>
        public string storyDescript;

        /// <summary> Главы сюжета </summary>
        public List<GamePart> nodesData;
    }
}