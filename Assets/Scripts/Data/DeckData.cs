/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [CreateAssetMenu(fileName = "DeckData", menuName = "Data/DeckData", order = 1)]
    public class DeckData: ScriptableObject
    {
        [SerializeField]
        private string deckName;
        public string DeckName
        {
            get { return deckName; }
            set { deckName = value; }
        }

        [TabGroup("Deck")]
        [SerializeField]
        private List<Card> initialDeck;
        public List<Card> InitialDeck // Retourne une ref du deck
        {
            get { return initialDeck; }
        }

        [TabGroup("Deck")]
        [SerializeField]
        private List<int> premiumCardID;
        public List<int> PremiumCardID // Retourne une ref du deck
        {
            get { return premiumCardID; }
        }

        [TabGroup("Equip")]
        [SerializeField]
        private CardEquipmentProba[] initialEquipment;
        public CardEquipmentProba[] InitialEquipment
        {
            get { return initialEquipment; }
        }

        /// <summary>
        /// Crée une copie du deck
        /// </summary>
        /// <returns></returns>
        public List<Card> CreateDeck()
        {
            List<Card> res = new List<Card>(initialDeck.Count);
            for (int i = 0; i < initialDeck.Count; i++)
            {
                res.Add(new Card(initialDeck[i].CardData, initialDeck[i].baseCardValue));
            }

            // Set si il y a des cartes premium
            for (int i = 0; i < premiumCardID.Count; i++)
            {
                res[premiumCardID[i]].CardPremium = true;
            }
            return res;
        }

        // Utilisé pour bien afficher les bonnes valeurs dans les UI quand on utilise la ref directe de initial deck
        public void SetValue()
        {
            for (int i = 0; i < initialDeck.Count; i++)
            {
                initialDeck[i].ResetCard();
            }
        }

        public void SetDeck(List<Card> deck)
        {
            initialDeck = deck;
        }

    } 

} // #PROJECTNAME# namespace