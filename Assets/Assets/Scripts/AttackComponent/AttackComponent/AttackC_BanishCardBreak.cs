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
    public class AttackC_BanishCardBreak : AttackComponent
    {
        [SerializeField]
        CardBreakController breakController = null;
        [SerializeField]
        bool hitCharacter = true;

        [Space]
        [SerializeField]
        CardController[] cardControllerUp = null;
        [SerializeField]
        CardController[] cardControllerDown = null;
        [SerializeField]
        CardType cardType = null;

        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            base.StartComponent(character, attack);
            CharacterBase target = breakController.CharacterBreaked;
            if (target == null)
                target = character.LockController.TargetLocked;

            if(target != null)
            {
                if(breakController.CardsBreaked != null)
                {
                    for (int i = 0; i < breakController.CardsBreaked.Count; i++)
                    {
                        breakController.CharacterBreaked.DeckController.BanishCard(breakController.CardsBreaked[i]);

                        // Pour l'effet de slice on doit dessiner 2x (a opti avec un shader surement)
                        cardControllerUp[i].transform.parent.parent.gameObject.SetActive(true); // oups
                        cardControllerUp[i].DrawCard(breakController.CardsBreaked[i], cardType);
                        cardControllerDown[i].DrawCard(breakController.CardsBreaked[i], cardType);
                    }
                }
                else
                {
                    // Pour désafficher l'effet des cartes vu qu'il n'y a rien a card Break
                    cardControllerUp[0].transform.parent.parent.gameObject.SetActive(false); 
                }


                if (hitCharacter)
                    target.CharacterKnockback.Hit(attack);
                    //attack.HasHit(target);
            }
        }

    } 

} // #PROJECTNAME# namespace