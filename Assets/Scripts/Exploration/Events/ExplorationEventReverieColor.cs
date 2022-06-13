using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    public class ExplorationEventReverieColor : ExplorationEvent
    {
        [SerializeField]
        GameRunData runData = null;

        // 0 = Rouge
        // 1 = Bleu
        // 2 = Vert
        [SerializeField]
        protected ExplorationEvent[] explorationEvents;


        ExplorationEvent eventSelected = null;

        public override void CreateEvent(ExplorationManager manager)
        {
            explorationManager = manager;

            eventSelected = Instantiate(explorationEvents[runData.GetReverieColor()], this.transform);
            eventSelected.CreateEvent(explorationManager);
        }

        public override void DestroyEvent()
        {
            eventSelected.DestroyEvent();
            StartCoroutine(AutoDestroyCoroutine());
        }

        private IEnumerator AutoDestroyCoroutine()
        {
            yield return new WaitForSeconds(5f);
            Destroy(this.gameObject);
        }


    }
}
