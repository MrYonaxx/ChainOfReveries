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
    public class AttackC_BanishCard : AttackComponent
    {
        [SerializeField]
        bool banish = false;

        [Space]
        [SerializeField]
        CardController cardController = null;
        [SerializeField]
        CardType cardType = null;


        public override void OnHitComponent(CharacterBase character, CharacterBase target)
        {
            base.OnHitComponent(character, target);
            if (target.DeckController.currentIndex != 0)
            {
                Card c = target.DeckController.SelectCard();
                if(banish)
                    target.DeckController.BanishCard(c);

                //Debug
                cardController.gameObject.transform.parent.gameObject.SetActive(true);
                cardController.DrawCard(c, cardType);
            }
        }

    } 

} // #PROJECTNAME# namespace