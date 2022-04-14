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
        [ShowIf("banish")]
        [SerializeField]
        bool keepPremiumSafe = false;

        [Space]
        [SerializeField]
        CardController cardController = null;
        [SerializeField]
        CardController cardControllerEnemy = null;
        [SerializeField]
        CardType cardType = null;

        [SerializeField]
        RectTransform posEnemy = null;


        public override void OnHitComponent(CharacterBase character, CharacterBase target)
        {
            base.OnHitComponent(character, target);
            if (target.DeckController.currentIndex != 0)
            {
                Card c = target.DeckController.SelectCard();
                if (banish)
                {
                    if(!(keepPremiumSafe && c.CardPremium))
                        target.DeckController.BanishCard(c);
                }

                if (target.tag == "Enemy")
                {
                    cardControllerEnemy.gameObject.transform.parent.gameObject.SetActive(true);
                    cardControllerEnemy.DrawCard(c, cardType, c.GetCardType());
                }
                else
                {
                    cardController.gameObject.transform.parent.gameObject.SetActive(true);
                    cardController.DrawCard(c, cardType, c.GetCardType());
                }

            }
        }

    } 

} // #PROJECTNAME# namespace