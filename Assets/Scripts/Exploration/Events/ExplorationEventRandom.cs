using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    public class ExplorationEventRandom : ExplorationEvent
    {

        [SerializeField]
        protected ExplorationEvent[] explorationEvents;


        ExplorationEvent eventSelected = null;

        public override void CreateEvent(ExplorationManager manager)
        {
            explorationManager = manager;
            //explorationManager.
            eventSelected = Instantiate(explorationEvents[Random.Range(0, explorationEvents.Length - 1)], this.transform);
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
