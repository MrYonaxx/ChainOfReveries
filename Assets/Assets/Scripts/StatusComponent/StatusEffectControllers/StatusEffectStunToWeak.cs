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
    public class StatusEffectStunToWeak: StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public bool hitCondition = false;

        [ShowIf("hitCondition")]
        [HorizontalGroup("Condition", LabelWidth = 100)]
        [ValueDropdown("SelectCardType")]
        [SerializeField]
        public int cardType = -1;

        [ShowIf("hitCondition")]
        [HorizontalGroup("Condition")]
        [ValueDropdown("SelectCardElement")]
        [SerializeField]
        public int attackElement = -1;

        [HorizontalGroup("Status")]
        [HideLabel]
        [SerializeField]
        StatusEffectData statusOnHit;
        [HorizontalGroup("Status")]
        [SerializeField]
        public float statusChance = 100;

        bool hasHit = false;

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

        public StatusEffectStunToWeak(StatusEffectStunToWeak data)
        {
            hitCondition = data.hitCondition;
            cardType = data.cardType;
            attackElement = data.attackElement;
            statusOnHit = data.statusOnHit;
            statusChance = data.statusChance;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectStunToWeak(this);
        }






        public override void ApplyEffect(CharacterBase character)
        {
            character.CharacterKnockback.OnHit += CallbackHit;
        }

        public override void UpdateEffect(CharacterBase character)
        {
            if (hasHit == true)
            {
                Debug.Log("Status applied");
                character.CharacterStatusController.ApplyStatus(statusOnHit, statusChance);
                hasHit = false;
            }
        }

        // Call by event OnHit
        private void CallbackHit(DamageMessage damageMessage)
        {
            if (hitCondition == true)
            {

                if (cardType > -1 && damageMessage.attackType != cardType)
                {
                    return;
                }

                if (attackElement != -1 && damageMessage.elements.Contains(attackElement) == false)
                {
                    return;
                }
            }
            hasHit = true;
        }

        public override void RemoveEffect(CharacterBase character)
        {
            character.CharacterKnockback.OnHit -= CallbackHit;
        }




#if UNITY_EDITOR
        private static IEnumerable SelectCardType()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("CardTypesData")[0]))
                .GetAllTypeID();
        }


        /*public static IEnumerable SelectCardType()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("CardTypesData")[0]))
                .GetAllTypeName();
        }*/

        public static IEnumerable SelectCardElement()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("ElementTypeData")[0]))
                .GetAllTypeID();
        }

#endif 

        #endregion

    }

} // #PROJECTNAME# namespace