using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Menu;

namespace VoiceActing
{
    public class Blacksmith : MonoBehaviour, IInteractable
    {
        [SerializeField]
        GameObject buttonPrompt = null;
        [SerializeField]
        MenuBase menu = null;

        bool active = true;
        CharacterBase character = null;
        IControllable previousControllable = null; // Généralement l'exploration manager


        void Start()
        {
            menu.OnEnd += Quit;
        }

        void OnDestroy()
        {
            menu.OnEnd += Quit;
        }



        public void CanInteract(bool b)
        {
            if (!active)
                b = false;
            buttonPrompt.gameObject.SetActive(b);
        }

        public void Interact(CharacterBase character)
        {
            if (!active)
                return;

            this.character = character;
            previousControllable = character.Inputs.Controllable;
            BattleFeedbackManager.Instance?.CameraController.AddTarget(this.transform, 10);

            GoToMenu();
            CanInteract(false);
        }

        public void GoToMenu()
        {
            menu.InitializeMenu();
            character.Inputs.SetControllable(menu, true);
            CountPremium();
        }

        public void Quit()
        {
            character.Inputs.SetControllable(previousControllable);
            BattleFeedbackManager.Instance?.CameraController.RemoveTarget(this.transform);
            CheckPremium();
            CanInteract(true);
        }

        // C'est inutilement compliqué pour rien j'en peux plus 
        // Tout était si générique jusqu'à ce problème
        [SerializeField]
        GameRunData runData = null;
        int nbPremium = 0;

        private void CountPremium()
        {
            for (int i = 0; i < runData.PlayerDeck.Count; i++)
            {
                if (runData.PlayerDeck[i].CardPremium)
                    nbPremium++;
            }
        }

        private void CheckPremium()
        {
            int previousNbPremium = nbPremium;
            CountPremium();
            if (previousNbPremium != nbPremium)
                active = false;
        }
    }
}