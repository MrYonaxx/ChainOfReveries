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
            previousControllable = character.Inputs.Controllable;
            BattleFeedbackManager.Instance?.CameraController.AddTarget(this.transform, 10);
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
            character.Inputs.SetControllable(menuShop);
        }

        public void QuitShop()
        {
            character.Inputs.SetControllable(previousControllable);
            BattleFeedbackManager.Instance?.CameraController.RemoveTarget(this.transform);
        }
    }
}