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
    public class StatusEffectDeckShuffle : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

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
        public StatusEffectDeckShuffle()
        {

        }

        public StatusEffectDeckShuffle(StatusEffectDeckShuffle data)
        {

        }

        public override StatusEffect Copy()
        {
            return new StatusEffectDeckShuffle(this);
        }



        public override void ApplyEffect(CharacterBase character)
        {
            List<Card> deckShuffled = new List<Card>(character.DeckController.DeckData);
            deckShuffled.RemoveAt(0); // On remove la carte Reload
            int n = deckShuffled.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n);
                Card value = deckShuffled[k];
                deckShuffled[k] = deckShuffled[n];
                deckShuffled[n] = value;
            }
            character.DeckController.SetDeck(deckShuffled);
        }


        /*public override void RemoveEffect(CharacterBase character)
        {
            for (int i = 0; i < cardReferences.Count; i++)
            {
                cardReferences[i].AddCardValue(-newValue[i]);
            }
            character.DeckController.RefreshDeck();
        }*/


        #endregion

    }

} // #PROJECTNAME# namespace