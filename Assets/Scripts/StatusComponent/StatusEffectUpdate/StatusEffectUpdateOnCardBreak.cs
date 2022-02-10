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
    public class StatusEffectUpdateOnCardBreak: StatusEffectUpdate
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public CardBreakController BreakController = null;
        [SerializeField]
        public int CardBreakCount = 10;
        [SerializeField]
        public bool UserCardBreaked = true;

        int count = 0;
        CharacterBase user;

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
        public StatusEffectUpdateOnCardBreak()
        {

        }

        public StatusEffectUpdateOnCardBreak(StatusEffectUpdateOnCardBreak statusEffect)
        {
            BreakController = statusEffect.BreakController;
            CardBreakCount = statusEffect.CardBreakCount;
            UserCardBreaked = statusEffect.UserCardBreaked;
        }

        public override StatusEffectUpdate Copy()
        {
            return new StatusEffectUpdateOnCardBreak(this);
        }



        
        public override void ApplyUpdate(CharacterBase character)
        {
            user = character;
            BreakController.OnCardBreak += UpdateCall;
            count = CardBreakCount;
        }

        public override bool Update()
        {
            if (count <= 0)
            {
                return false;
            }
            return true;
        }

        public override void Remove(CharacterBase character)
        {
            BreakController.OnCardBreak -= UpdateCall;
        }

        public override int ValueToDraw()
        {
            return count;
        }



        public void UpdateCall(CharacterBase characterBreaked, List<Card> cardBreaked, CharacterBase characterBreaker, List<Card> cardBreaker)
        {
            if (cardBreaker == null)
                return;
            if (characterBreaked == user && UserCardBreaked) // si le perso qui se fait breaker est le notre
            {
                count -= 1;
                user.CharacterStatusController.RefreshStatus();
            }
            else if (characterBreaker == user && !UserCardBreaked) // si le perso qui inflige le break est le notre
            {
                count -= 1;
                user.CharacterStatusController.RefreshStatus();
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