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
    public class StatusEffectCardValue: StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public int BonusValue = 1;

        [SerializeField]
        public bool BonusCondition = false;

        [ShowIf("BonusCondition")]
        [ValueDropdown("SelectCardType")]
        [SerializeField]
        public int CardType = -1;

        [SerializeField]
        public bool ClampValue = false;
        
        [SerializeField]
        public bool IgnoreZero = false;

        List<Card> cardReferences;
        List<int> bonusValues;

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
        public StatusEffectCardValue()
        {

        }

        public StatusEffectCardValue(StatusEffectCardValue data)
        {
            BonusValue = data.BonusValue;
            BonusCondition = data.BonusCondition;
            ClampValue = data.ClampValue; 
            IgnoreZero = data.IgnoreZero;
            CardType = data.CardType;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectCardValue(this);
        }



        public override void ApplyEffect(CharacterBase character)
        {
            cardReferences = new List<Card>();
            bonusValues = new List<int>();
            for (int i = 0; i < character.DeckController.DeckData.Count; i++)
            {
                if (BonusCondition)
                {
                    if (character.DeckController.DeckData[i].GetCardType() == CardType)
                    {
                        AddCard(character, i);
                        /*character.DeckController.DeckData[i].AddCardValue(BonusValue);
                        cardReferences.Add(character.DeckController.DeckData[i]);*/
                    }
                }
                else
                {
                    AddCard(character, i);
                    /*character.DeckController.DeckData[i].AddCardValue(BonusValue);
                    cardReferences.Add(character.DeckController.DeckData[i]);*/
                }
            }
            character.DeckController.RefreshDeck();
        }


        public override void RemoveEffect(CharacterBase character)
        {
            for (int i = 0; i < cardReferences.Count; i++)
            {
                cardReferences[i].AddCardValue(-bonusValues[i]);
            }
        }

        private void AddCard(CharacterBase character, int i)
        {
            if (IgnoreZero && character.DeckController.DeckData[i].baseCardValue == 0)
                return;

            if(ClampValue)
            {
                int value = character.DeckController.DeckData[i].GetCardValue() + BonusValue;
                if(value < 0)
                {
                    value = BonusValue - value;
                }
                else if (value > 9)
                {
                    value = 9 - value + BonusValue;
                }
                else
                {
                    value = BonusValue;
                }
                bonusValues.Add(value);
            }
            else
            {
                bonusValues.Add(BonusValue);
            }


            character.DeckController.DeckData[i].AddCardValue(bonusValues[bonusValues.Count - 1]);
            cardReferences.Add(character.DeckController.DeckData[i]);
        }


#if UNITY_EDITOR
        private static IEnumerable SelectCardType()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("CardTypesData")[0]))
                .GetAllTypeID();
        }
#endif
        #endregion

    }

} // #PROJECTNAME# namespace