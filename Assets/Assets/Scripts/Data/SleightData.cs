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
    [CreateAssetMenu(fileName = "SleightData", menuName = "Sleight", order = 1)]
    public class SleightData: ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        private string sleightName;
        public string SleightName
        {
            get { return sleightName; }
        }

        [SerializeField]
        [TextArea(2,3)]
        private string sleightDescription;
        public string SleightDescription
        {
            get { return sleightDescription; }
        }

        /*[SerializeField]
        int cardDamage;
        public int CardDamage
        {
            get { return cardDamage; }
        }

        [SerializeField]
        int breakRecovery;
        public int BreakRecovery
        {
            get { return breakRecovery; }
        }*/


        [Title("Sleight Parameter")]
        [SerializeField]
        Vector2 sleightValue;
        public Vector2 SleightValue
        {
            get { return sleightValue; }
        }

        /*[HorizontalGroup("SleightParameter")]
        [VerticalGroup("SleightParameter/Left")]*/
        [SerializeField]
        CardData[] sleightRecipe;
        public CardData[] SleightRecipe
        {
            get { return sleightRecipe; }
        }

        [Title("Sleight Attack Data")]
        [SerializeField]
        CardData sleightAttackData;
        public CardData SleightAttackData
        {
            get { return sleightAttackData; }
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
        public virtual bool CheckSleight(List<Card> cards)
        {
            int sum = 0;
            if (cards.Count < sleightRecipe.Length)
                return false;

            int recipeIndex = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                if (recipeIndex < sleightRecipe.Length)
                {
                    if (cards[i].CardData == sleightRecipe[recipeIndex])
                    {
                        recipeIndex += 1;
                    }
                    else
                    {
                        recipeIndex = 0;
                    }
                }
                sum += cards[i].GetCardValue();

            }
            if (recipeIndex != sleightRecipe.Length)
                return false;

            if (sum < sleightValue.x || sum > sleightValue.y)
                return false;
            return true;
        }

        /// <summary>
        /// Retourne soit une carte représentant la sleight, soit les 3 cartes si on trouve aucune sleight associé
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public virtual List<Card> GetAttackData(List<Card> cards)
        {
            List<Card> res = new List<Card>();

            int recipeIndex = 0;
            int sleightValue = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                if(recipeIndex < sleightRecipe.Length) 
                {
                    if (cards[i].CardData == sleightRecipe[recipeIndex])
                    {
                        recipeIndex += 1;
                        sleightValue += cards[i].GetCardValue();
                        if (recipeIndex >= sleightRecipe.Length)
                            res.Add(new Card(sleightAttackData, sleightValue));
                        continue;
                    }
                }
                res.Add(cards[i]);

            }

            return res;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace