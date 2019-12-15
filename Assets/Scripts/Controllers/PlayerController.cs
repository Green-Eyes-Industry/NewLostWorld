using System.Collections.Generic;
using UnityEngine;

/// <summary> Игровые события и переключения </summary>
public class PlayerController : MonoBehaviour
{
    #region PARTS_EVENTS

    /// <summary> Запуск событий главы </summary>
    public void EventsStart(List<GameEvent> partEvents)
    {
        if (partEvents.Count != 0)
        {
            for (int i = 0; i < partEvents.Count; i++)
            {
                if (partEvents[i] is RandomPart) StartRandomEvent((RandomPart)partEvents[i]);
                else if (!partEvents[i].EventStart()) AnimController.NextPart(partEvents[i].FailPart());
            }
        }
    }

    /// <summary> Запустить эвент с рандомом </summary>
    private void StartRandomEvent(RandomPart e)
    {
        if (e.part_1_random != null) AnimController.thisPart.movePart_1 = e.Randomize(AnimController.thisPart, e.part_1_random, e.randomChance_1);
        if (e.part_2_random != null) AnimController.thisPart.movePart_2 = e.Randomize(AnimController.thisPart, e.part_2_random, e.randomChance_2);
        if (e.part_2_random != null) AnimController.thisPart.movePart_3 = e.Randomize(AnimController.thisPart, e.part_3_random, e.randomChance_3);
    }

    #endregion
}