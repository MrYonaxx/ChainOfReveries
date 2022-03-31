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
    public class CardBreakType: CardBreakComponent
    {
        [ValueDropdown("SelectCardType")]
        [SerializeField]
        public int unbreakableType = -1;

        public override int CheckCardBreak(CharacterBase currentCharacter, List<Card> cardsActive, CharacterBase challengerCharacter, List<Card> newCards)
        {
            if(cardsActive[0].GetCardType() == unbreakableType)
            {
                return -1;
            }
            return base.CheckCardBreak(currentCharacter, cardsActive, challengerCharacter, newCards);
        }


        public override int CheckCardBreak(Card cardActive, Card newCard)
        {
            if (cardActive.GetCardType() == unbreakableType)
            {
                return -1;
            }
            return base.CheckCardBreak(cardActive, newCard);
        }

#if UNITY_EDITOR
        private static IEnumerable SelectCardType()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("CardTypesData")[0]))
                .GetAllTypeID();
        }
#endif

    }

} // #PROJECTNAME# namespace