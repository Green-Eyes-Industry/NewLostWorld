using System.Collections.Generic;
using Data;
using Data.GameEvents;
using Data.GameEffects;

namespace Controllers
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

            if (MainController.instance.dataController.mainPlayer.playerEffects != null &&
                MainController.instance.dataController.mainPlayer.playerEffects.Count > 0)
            {
                for (int i = 0; i < MainController.instance.dataController.mainPlayer.playerEffects.Count; i++)
                {
                    switch (MainController.instance.dataController.mainPlayer.playerEffects[i])
                    {
                        case PositiveEffect positiveEffect:
                            
                            if (positiveEffect.durationEffect > 0)
                            {
                                MainController.instance.dataController.mainPlayer.playerHealth += positiveEffect.healthInfluenceEffect;
                                MainController.instance.dataController.mainPlayer.playerMind += positiveEffect.mindInfluenceEffect;
                                positiveEffect.durationEffect--;
                            }
                            break;

                        case NegativeEffect negativeEffect:

                            if (negativeEffect.durationEffect > 0)
                            {
                                MainController.instance.dataController.mainPlayer.playerHealth -= negativeEffect.healthInfluenceEffect;
                                MainController.instance.dataController.mainPlayer.playerMind -= negativeEffect.mindInfluenceEffect;
                                negativeEffect.durationEffect--;
                            }
                            break;
                    }
                }

                ClearEffects();
            }
        }

        /// <summary> Убрать завершенные эффекты </summary>
        private void ClearEffects()
        {
            bool checker = false;

            foreach (GameEffect effect in MainController.instance.dataController.mainPlayer.playerEffects)
            {
                if(effect.durationEffect == 0)
                {
                    checker = true;
                    MainController.instance.dataController.mainPlayer.playerEffects.Remove(effect);
                    break;
                }
            }

            if (checker) ClearEffects();
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