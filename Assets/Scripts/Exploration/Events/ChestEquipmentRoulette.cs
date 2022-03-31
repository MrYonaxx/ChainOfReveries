using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class ChestEquipmentRoulette : MonoBehaviour, IInteractable
    {

        [SerializeField]
        GameRunData gameRunData = null;

        [SerializeField]
        int nbCard = 10;
        [SerializeField]
        CardEquipmentDatabase equipmentDatabase = null;
        [SerializeField]
        MenuRoulette menuRoulette = null;
        [SerializeField]
        Menu.MenuWeaponSelection menuWeaponSelection = null;

        [Title("UI")]
        [SerializeField]
        Animator chestAnimator = null;
        [SerializeField]
        GameObject buttonPrompt = null;

        bool isOpen = false;
        CharacterBase characterToInteract = null;
        List<Card> cardsGacha = new List<Card>();

        IControllable previousControllable = null;


        public void CanInteract(bool b)
        {
            buttonPrompt.gameObject.SetActive(b);

        }

        public void Interact(CharacterBase character)
        {
            characterToInteract = character;
            GenerateDeck();
            menuRoulette.CreateRoulette(cardsGacha);
            menuRoulette.OnCardSelected += GetEquipmentCard;
            previousControllable = character.Inputs.Controllable;
            character.Inputs.SetControllable(menuRoulette, true);
            isOpen = true;
            chestAnimator.SetTrigger("Feedback");
            // Flemme detecte
            GetComponent<BoxCollider2D>().enabled = false;
        }

        private void GenerateDeck()
        {
            cardsGacha = new List<Card>(nbCard);
            for (int i = 0; i < nbCard; i++)
            {
                cardsGacha.Add(new CardEquipment(equipmentDatabase.GachaEquipment()));
            }
        }

        private void GetEquipmentCard(int index)
        {
            menuRoulette.OnCardSelected -= GetEquipmentCard;

            CardEquipment reward = (CardEquipment)cardsGacha[index];
            // On ajoute la carte équipement au deck du joueur
            if(!gameRunData.AddEquipmentCard(reward))
            {
                // Inventaire plein
                menuWeaponSelection.SetCard(characterToInteract, reward);
                menuWeaponSelection.InitializeMenu();
                menuWeaponSelection.OnEnd += BackToPlayer;
                characterToInteract.Inputs.SetControllable(menuWeaponSelection, true);
                return;
            }
            // On applique les effets de la carte équipement au joueur
            characterToInteract.CharacterEquipment.EquipCard(reward.CardEquipmentData, 1);
            // (Note : ça se fait en 2 appel de fonction le fait de gagner une carte équipement c'est un peu spaghetti)
            // (Une alternative serait que GameRunData ait une référence du player de la scène)
            // (Mais à chaque changement de scène (s'il y'en a) il faudra re assginer la ref du player à Game Run Data)
            characterToInteract.Inputs.SetControllable(previousControllable);
        }


        private void BackToPlayer()
        {
            menuWeaponSelection.OnEnd -= BackToPlayer;
            characterToInteract.Inputs.SetControllable(previousControllable);
        }
    }
}
