using System.Collections.Generic;
using NLW.Data;

namespace NLW
{
    /// <summary> Игровые события и переключения </summary>
    public class GameController : MainController
    {
        protected override void Init() { }

        #region PARTS_EVENTS

        /// <summary> Запуск событий главы </summary>
        public void EventsStart(List<GameEvent> partEvents)
        {
            if (partEvents.Count != 0)
            {
                for (int i = 0; i < partEvents.Count; i++)
                {
                    if (partEvents[i] is RandomPart) StartRandomEvent((RandomPart)partEvents[i]);
                    else if (!partEvents[i].EventStart()) animController.NextPart(partEvents[i].FailPart());
                }
            }
        }

        /// <summary> Запустить эвент с рандомом </summary>
        private void StartRandomEvent(RandomPart e)
        {
            for (int i = 0; i < e.part_random.Length; i++)
            {
                if (e.part_random[i] != null) animController.thisPart.movePart[i] = e.Randomize(animController.thisPart, e.part_random[i], e.randomChance[i]);
            }
        }

        #endregion
    }
}