using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class ChestEquipment : MonoBehaviour, IInteractable
    {

        /*[SerializeField]
        GameRunData gameRunData = null;*/

        [SerializeField]
        BattleReward battleReward = null;

        [Title("UI")]
        [SerializeField]
        Animator chestAnimator = null;
        [SerializeField]
        GameObject buttonPrompt = null;

        bool isOpen = false;
        CharacterBase characterToInteract = null;

        IControllable previousControllable = null;


        public void CanInteract(bool b)
        {
            buttonPrompt.gameObject.SetActive(b);

        }

        public void Interact(CharacterBase character)
        {
            characterToInteract = character;
            previousControllable = character.Inputs.Controllable;

            battleReward.InitializeBattleReward(character);
            battleReward.OnEventEnd += EndChest;
            character.Inputs.SetControllable(battleReward);

            isOpen = true;
            chestAnimator.SetTrigger("Feedback");

            // Flemme detecté
            GetComponent<BoxCollider2D>().enabled = false;
        }

        private void EndChest()
        {
            characterToInteract.Inputs.SetControllable(previousControllable);
            battleReward.OnEventEnd += EndChest;
        }

    }
}
