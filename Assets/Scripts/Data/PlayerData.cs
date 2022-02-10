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
    [CreateAssetMenu(fileName = "PlayerData", menuName = "CharacterData/Player", order = 1)]
    public class PlayerData : CharacterData
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [TabGroup("DeckExploration")]
        [SerializeField]
        private CardExplorationData[] initialDeckExploration = null;
        public CardExplorationData[] InitialDeckExploration
        {
            get { return initialDeckExploration; }
        }


        [TabGroup("CardDatabase")]
        [OnValueChanged("CalculateMaxProbability", true)]
        [SerializeField]
        private List<Card> cardProbability = null;
        public List<Card> CardProbability
        {
            get { return cardProbability; }
        }

        [TabGroup("CardDatabase")]
        [SerializeField]
        [ReadOnly]
        private int maxProbability;


        [SerializeField]
        private Sprite spriteProfile = null;
        public Sprite SpriteProfile
        {
            get { return spriteProfile; }
        }

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        public Card CreateCharacterNewCard()
        {
            int r = Random.Range(0, maxProbability);
            int i = 0;
            int sum = cardProbability[i].baseCardValue;
            while (r >= sum && i < cardProbability.Count)
            {              
                i += 1;
                sum += cardProbability[i].baseCardValue;
            }

            // Check si c'est premium 
            bool isPremium = false;
            r = Random.Range(0, 100);
            if (r < 3) // 3% de chance de gacha une carte premium
                isPremium = true;

            return new Card(cardProbability[i].CardData, cardProbability[i].CardData.GetRandomCardValue(), isPremium);
        }



        private void CalculateMaxProbability()
        {
            int sum = 0;
            for (int i = 0; i < cardProbability.Count; i++)
            {
                sum += cardProbability[i].baseCardValue;
            }
            maxProbability = sum;
        }


        public List<CardData> CopyCardDatabase()
        {
            List<CardData> res = new List<CardData>(cardProbability.Count);
            for (int i = 0; i < cardProbability.Count; i++)
            {
                res.Add(cardProbability[i].CardData);
            }
            return res;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace