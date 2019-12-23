using System.Collections.Generic;
using UnityEngine;
using NLW.Data;

namespace NLW
{
    /// <summary> Игровые события и переключения </summary>
    public class GameController : ParentController
    {
        public override void Init() { }

        #region PARTS_EVENTS

        /// <summary> Запуск событий главы </summary>
        public void EventsStart(List<GameEvent> partEvents)
        {
            if (partEvents.Count != 0)
            {
                for (int i = 0; i < partEvents.Count; i++)
                {
                    if (partEvents[i] is RandomPart) StartRandomEvent((RandomPart)partEvents[i]);
                    else if (!partEvents[i].EventStart()) MainController.instance.animController.NextPart(partEvents[i].FailPart());
                }
            }
        }

        /// <summary> Запустить эвент с рандомом </summary>
        private void StartRandomEvent(RandomPart e)
        {
            for (int i = 0; i < e.partRandom.Length; i++)
            {
                if (e.partRandom[i] != null) MainController.instance.animController.thisPart.movePart[i] =
                        e.Randomize(MainController.instance.animController.thisPart, e.partRandom[i], e.randomChance[i]);
            }
        }

        #endregion
    }
}