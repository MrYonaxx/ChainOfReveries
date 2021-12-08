using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    public class CharacterEquipment : MonoBehaviour
    {
        [SerializeField]
        private DeckController deckEquipmentController;
        public DeckController DeckEquipmentController
        {
            get { return deckEquipmentController; }
        }


        CharacterStat characterStat = null;
        CharacterStatusController characterStatusController = null;

        List<CardEquipment> cardsEquipment = null;

        public bool InEquipmentDeck = false;

        public void InitializeComponent(CharacterBase character)
        {
            characterStat = character.CharacterStat;
            characterStatusController = character.CharacterStatusController;

            // Créer automatiquement un deck à partir du deck initial
            cardsEquipment = new List<CardEquipment>(character.CharacterData.InitialEquipment.Length);
            for (int i = 0; i < character.CharacterData.InitialEquipment.Length; i++)
            {
                cardsEquipment.Add(new CardEquipment(character.CharacterData.InitialEquipment[i].cardEquipment));
            }

            // On set le deck du character pour bénéficier des effets actifs
            SetEquipmentDeck(cardsEquipment);

            // On équipe les cartes équipements pour bénéficier des effets passifs
            EquipAll();
        }

        public void SetEquipmentDeck(List<CardEquipment> deckEquipment)
        {
            cardsEquipment = deckEquipment;

            // Seul les cartes équipement avec un status actif peuvent être joué
            List<Card> cardPlayable = new List<Card>();
            for (int i = 0; i < deckEquipment.Count; i++)
            {
                if(deckEquipment[i].CardEquipmentData.StatusEffectActive != null)
                    cardPlayable.Add(deckEquipment[i]);
            }
            deckEquipmentController.SetDeck(cardPlayable);
        }





        public void SwitchToEquipmentDeck(bool b)
        {
            InEquipmentDeck = b;
        }

        // Check si on peut jouer les cartes équipements
        public bool CanAct()
        {
            return true;
        }

        public void PlayCard()
        {
            // Supprime la carte du deck
            CardEquipment card = (CardEquipment)deckEquipmentController.SelectCard();
            // Ajoute les effets de la carte

            if (card.CardEquipmentData.StatusEffectActive != null)
                characterStatusController.ApplyStatus(card.CardEquipmentData.StatusEffectActive, 100);
        }





        // Appelés à la création 
        private void EquipAll()
        {
            for (int i = 0; i < cardsEquipment.Count; i++)
            {
                EquipCard(cardsEquipment[i].CardEquipmentData, 0);
            }
        }

        public void EquipCard(CardEquipmentData cardData, int level)
        {
            for (int i = 0; i < cardData.StatsModifier.Count; i++)
            {
                characterStat.AddStat(cardData.StatsModifier[i]);
            }
            if(cardData.StatusEffectPassive != null)
                characterStatusController.ApplyStatus(cardData.StatusEffectPassive, 100);
        }

        public void UnequipCard(CardEquipmentData cardData, int level)
        {
            for (int i = 0; i < cardData.StatsModifier.Count; i++)
            {
                characterStat.RemoveStat(cardData.StatsModifier[i]);
            }
            //characterStatusController.RemoveStatus(cardData.StatusEffectPassive, 100);
        }

    }
}
