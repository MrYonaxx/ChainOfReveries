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
    public class StatusEffectCardValueRandom : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        List<Card> cardReferences;
        List<int> newValue;

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
        public StatusEffectCardValueRandom()
        {

        }

        public StatusEffectCardValueRandom(StatusEffectCardValueRandom data)
        {

        }

        public override StatusEffect Copy()
        {
            return new StatusEffectCardValueRandom(this);
        }



        public override void ApplyEffect(CharacterBase character)
        {
            cardReferences = new List<Card>();
            newValue = new List<int>();
            int r;
            for (int i = 0; i < character.DeckController.DeckData.Count; i++)
            {
                cardReferences.Add(character.DeckController.DeckData[i]);
                r = Random.Range(0, 9);
                newValue.Add(r);
                character.DeckController.DeckData[i].AddCardValue(r);
            }
            character.DeckController.RefreshDeck();
        }


        public override void RemoveEffect(CharacterBase character)
        {
            for (int i = 0; i < cardReferences.Count; i++)
            {
                cardReferences[i].AddCardValue(-newValue[i]);
            }
            character.DeckController.RefreshDeck();
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