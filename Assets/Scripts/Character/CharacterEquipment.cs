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

        List<CardEquipment> cardsEquipment = null; // Liste comprenant tout les équipements
        CardEquipment[] cardsEquipmentWeapon; // Liste comprenant uniquement les equipements weapon
        public CardEquipment[] CardsEquipmentWeapon
        {
            get { return cardsEquipmentWeapon; }
        }


        // Variable pas très habile pour récupérer la carte qu'on vient d'équiper
        CardEquipment latestCardEquipment;
        public CardEquipment LatestCardEquipment
        {
            get { return latestCardEquipment; }
        }


        public delegate void ActionCharacterEquipment(CardEquipment[] equipments);
        public event ActionCharacterEquipment OnEquipWeapon;

        public void InitializeComponent(CharacterBase character)
        {
            characterStat = character.CharacterStat;
            characterStatusController = character.CharacterStatusController;

            // Créer automatiquement un deck à partir du deck initial et équipe les cartes
            cardsEquipment = new List<CardEquipment>(character.CharacterData.InitialEquipment.Length);
            for (int i = 0; i < character.CharacterData.InitialEquipment.Length; i++)
            {
                cardsEquipment.Add(new CardEquipment(character.CharacterData.InitialEquipment[i].cardEquipment));
                EquipCard(character.CharacterData.InitialEquipment[i].cardEquipment);
            }

            // On set le deck du character pour bénéficier des effets actifs
            SetWeaponDeck(cardsEquipment);
        }


        /// <summary>
        /// Créer le deck équipement
        /// </summary>
        /// <param name="deckEquipment"></param>
        public void SetEquipmentDeck(List<CardEquipment> deckEquipment)
        {
            UnequipAll();

            cardsEquipment = new List<CardEquipment>(deckEquipment.Count);
            for (int i = 0; i < deckEquipment.Count; i++)
            {
                cardsEquipment.Add(deckEquipment[i]);
                EquipCard(deckEquipment[i].CardEquipmentData);
            }
        }

        /// </summary>
        /// Créer un deck de carte équipement Arme pour les utiliser en combat, à partir d'une liste de carte 
        /// </summary>
        /// /// <param name="deckEquipment"></param>
        public void SetWeaponDeck(List<CardEquipment> deckEquipment)
        {
            cardsEquipment = deckEquipment;
            cardsEquipmentWeapon = new CardEquipment[4];
            int index = 0;

            // Seul les cartes équipement avec un status actif peuvent être joué
            for (int i = 0; i < deckEquipment.Count; i++)
            {
                if (index >= cardsEquipmentWeapon.Length)
                    break;
                if (deckEquipment[i].CardEquipmentData.EquipmentAction != null) 
                {
                    cardsEquipmentWeapon[index] = deckEquipment[i];
                    index++;
                }
            }
            OnEquipWeapon?.Invoke(cardsEquipmentWeapon);
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

            latestCardEquipment = card;

            // Ajoute les effets de la carte
            if (card.CardEquipmentData.StatusEffectActive != null)
                characterStatusController.ApplyStatus(card.CardEquipmentData.StatusEffectActive, 100);
        }

        // En utilisant le pavé numérique 2 = bas, 4 = gauche, 6 = droite, 8 = haut
        public CardEquipment PlayCard(int input)
        {
            input /= 2;
            input--;

            if (cardsEquipmentWeapon[input] == null)
                return null;

            CardEquipment card = cardsEquipmentWeapon[input];
            cardsEquipmentWeapon[input] = null;

            latestCardEquipment = card;

            // Ajoute les effets de la carte
            if (card.CardEquipmentData.StatusEffectActive != null)
                characterStatusController.ApplyStatus(card.CardEquipmentData.StatusEffectActive, 100);

            OnEquipWeapon?.Invoke(cardsEquipmentWeapon);
            return card;
        }







        // Appelés à la création 
        private void EquipAll()
        {
            for (int i = 0; i < cardsEquipment.Count; i++)
            {
                EquipCard(cardsEquipment[i].CardEquipmentData, 0);
            }
        }

        // Ajoute les effets passives de l'équipement au character
        public void EquipCard(CardEquipmentData cardData, int level = 0)
        {
            for (int i = 0; i < cardData.StatsModifier.Count; i++)
            {
                characterStat.AddStat(cardData.StatsModifier[i]);
            }
            if(cardData.StatusEffectPassive != null)
                characterStatusController.ApplyStatus(cardData.StatusEffectPassive, 100);
        }

        public void UnequipAll()
        {
            for (int i = 0; i < cardsEquipment.Count; i++)
            {
                UnequipCard(cardsEquipment[i].CardEquipmentData, 0);
            }
        }
        public void UnequipCard(CardEquipmentData cardData, int level)
        {
            for (int i = 0; i < cardData.StatsModifier.Count; i++)
            {
                characterStat.RemoveStat(cardData.StatsModifier[i]);
            }
            if (cardData.StatusEffectPassive != null)
                characterStatusController.RemoveStatus(cardData.StatusEffectPassive);
        }

    }
}
