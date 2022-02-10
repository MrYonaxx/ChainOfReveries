/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    // J'aurais du nommer ça en CharacterSleightController
    public class SleightController: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */


        [Title("Debug")]
        [SerializeField]
        SleightData[] sleightsDatabase;


        int indexSleightCard = 0;
        List<Card> sleightCards = new List<Card>(3);
        SleightData currentSleight = null;

        public delegate void ActionSleight(SleightData currentSleight, List<Card> sleightCards);
        public event ActionSleight OnSleightUpdate;

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

        public void SetSleightDatabase(SleightData[] sleightDatas)
        {
            sleightsDatabase = sleightDatas;
        }

        public bool CanPlaySleight()
        {
            if (indexSleightCard == 3)
                return true;
            return false;
        }

        public int GetIndexSleightCard()
        {
            return indexSleightCard;
        }

        public List<Card> GetSleightCards()
        {
            return sleightCards;
        }

        /// <summary>
        /// Transform the list of cards into its sleight equivalent
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public List<Card> GetSleightAction(List<Card> cards, out bool success)
        {
            success = false;
            if (cards.Count <= 1)
                return cards;
            else if (currentSleight == null)
                return cards;

            success = true;
            return currentSleight.GetAttackData(cards);
        }





        public void AddSleightCard(Card card)
        {
            if (indexSleightCard == 3)
                return;
            sleightCards.Add(card);
            //DrawCard(card, handTransform);
            indexSleightCard += 1;
            CheckSleight();
            OnSleightUpdate?.Invoke(currentSleight, sleightCards);
            //DrawSleight();
        }

        public void ResetSleightCard()
        {
            currentSleight = null;
            indexSleightCard = 0;
            sleightCards.Clear();
            OnSleightUpdate?.Invoke(currentSleight, sleightCards);
        }

        private void CheckSleight()
        {
            for(int i = 0; i < sleightsDatabase.Length; i++)
            {
                if(sleightsDatabase[i].CheckSleight(sleightCards) == true)
                {
                    currentSleight = sleightsDatabase[i];
                    return;
                }
            }
        }







        #endregion

    } 

} // #PROJECTNAME# namespace