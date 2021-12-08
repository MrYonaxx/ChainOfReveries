using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    public class ExplorationEvent : MonoBehaviour
    {

        protected ExplorationManager explorationManager = null;

        [SerializeField]
        protected Animator animatorBackground;


        protected bool eventStart = false;


        // L'event est crée mais n'a pas démarré
        // On file une ref à exploration manager pour pouvoir a peu près tout faire
        public virtual void CreateEvent(ExplorationManager manager)
        {
            explorationManager = manager;
        }

        // L'event démarre
        public virtual void StartEvent()
        {
            ShowBackground(true);
            EndEvent();
        }

        // L'event est fini on peut choisir des cartes
        public virtual void EndEvent()
        {
            explorationManager.CreateExplorationMenu();
        }

        // On a choisi un prochain event, on peut donc détruire l'event en cours
        public virtual void DestroyEvent()
        {
            ShowBackground(false);
            StartCoroutine(AutoDestroyCoroutine());
        }


        protected void ShowBackground(bool b)
        {
            animatorBackground.gameObject.SetActive(true);
            animatorBackground.SetBool("Appear", b);
        }

        private IEnumerator AutoDestroyCoroutine()
        {
            yield return new WaitForSeconds(5f);
            Destroy(this.gameObject);
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (eventStart)
                return;
            if(collision.CompareTag("Player"))
            {
                if(collision.GetComponent<CharacterBase>()) 
                {
                    StartEvent();
                    eventStart = true;
                }
            }
        }

    }
}
