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
            DestroyPreviousRoom();
            EndEvent();
        }

        // L'event est fini on peut choisir des cartes
        public virtual void EndEvent()
        {
            ShowBackground(false);
            explorationManager.CreateExplorationMenu();
        }

        // On a choisi un prochain event, on peut donc détruire l'event en cours
        public virtual void DestroyEvent()
        {
            StartCoroutine(AutoDestroyCoroutine());
        }


        protected void ShowBackground(bool b)
        {
            if(b)
                animatorBackground.gameObject.SetActive(true);
            animatorBackground.SetBool("Appear", b);
        }

        // Dis à exploration manager de détruire la room précédente, pas très smooth de le mettre là mais tant pis
        protected void DestroyPreviousRoom()
        {
            explorationManager.DestroyPreviousRoom();
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
