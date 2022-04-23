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
    public class AttackC_DispatchCards: AttackComponent
    {
        [SerializeField]
        CardType cardType = null;
        [SerializeField]
        GetCardObject cardObject = null;

        [SerializeField]
        Vector2 cardDispatchRangeX = new Vector2(-5, 5);
        [SerializeField]
        Vector2 cardDispatchRangeY = new Vector2(-2, 1);

        public override void OnHitComponent(CharacterBase character, CharacterBase target)
        {
            float posX = 0;
            float posY = 0;
            Card c = null;

            for (int i = target.DeckController.Deck.Count-1; i >= 0; i--)
            {
                c = target.DeckController.Deck[i];
                target.DeckController.BanishCard(c);

                // Generate card
                posX = BattleUtils.Instance.BattleCenter.position.x + Random.Range(cardDispatchRangeX.x, cardDispatchRangeX.y);
                posY = BattleUtils.Instance.BattleCenter.position.y + Random.Range(cardDispatchRangeY.x, cardDispatchRangeY.y);

                GetCardObject fieldCard = Instantiate(cardObject, target.transform.position, Quaternion.identity);
                fieldCard.Initialize(target, c, cardType.GetColorType(c.GetCardType()), posX, posY);

                target.DeckController.Remove(i);
                target.DeckController.SetIndex(0);

                target.DeckController.RefreshDeck();

            }
        }

    } 

} // #PROJECTNAME# namespace