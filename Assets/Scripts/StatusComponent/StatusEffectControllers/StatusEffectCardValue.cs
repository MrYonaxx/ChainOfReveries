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
            CardType = data.CardType;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectCardValue(this);
        }



        public override void ApplyEffect(CharacterBase character)
        {
            for (int i = 0; i < character.DeckController.DeckData.Count; i++)
            {
                if (BonusCondition)
                {
                    if(character.DeckController.DeckData[i].GetCardType() == CardType)
                        character.DeckController.DeckData[i].AddCardValue(BonusValue);
                }
                else
                {
                    character.DeckController.DeckData[i].AddCardValue(BonusValue);
                }
            }
            character.DeckController.RefreshDeck();
        }


        public override void RemoveEffect(CharacterBase character)
        {
            for (int i = 0; i < character.DeckController.DeckData.Count; i++)
            {
                if (BonusCondition)
                {
                    if (character.DeckController.DeckData[i].GetCardType() == CardType)
                        character.DeckController.DeckData[i].AddCardValue(-BonusValue);
                }
                else
                {
                    character.DeckController.DeckData[i].AddCardValue(-BonusValue);
                }
            }
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