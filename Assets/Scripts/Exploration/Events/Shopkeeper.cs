using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Menu;

namespace VoiceActing
{
    public class Shopkeeper : MonoBehaviour, IInteractable
    {
        [SerializeField]
        GameObject buttonPrompt = null;
        [SerializeField]
        Transform cameraFocus = null;
        [SerializeField]
        BattleReward battleReward = null;
        [SerializeField]
        MenuShop menuShop = null;

        bool firstTime = false;

        CharacterBase character = null;
        IControllable previousControllable = null; // Généralement l'exploration manager


        void Start()
        {
            battleReward.OnEventEnd += Shop;
            menuShop.OnEnd += QuitShop;
        }

        void OnDestroy()
        {
            battleReward.OnEventEnd -= Shop;
            menuShop.OnEnd += QuitShop;
        }



        public void CanInteract(bool b)
        {
            buttonPrompt.gameObject.SetActive(b);
        }

        public void Interact(CharacterBase character)
        {
            this.character = character;
            character.CharacterMovement.InMovement = false; // hack pour que le perso arrête de bouger

            previousControllable = character.Inputs.Controllable;
            

            if (!firstTime)
            {
                character.Inputs.SetControllable(battleReward);
                battleReward.InitializeBattleReward(character);
                firstTime = true;
            }
            else
            {
                Shop();
            }
            CanInteract(false);


        }

        public void Shop()
        {
            menuShop.InitializeMenu();
            menuShop.SetCharacter(character);
            character.Inputs.SetControllable(menuShop);
            BattleFeedbackManager.Instance?.CameraController.AddTarget(cameraFocus, 10);
        }

        public void QuitShop()
        {
            character.Inputs.SetControllable(previousControllable);
            BattleFeedbackManager.Instance?.CameraController.RemoveTarget(cameraFocus);
        }
    }
}