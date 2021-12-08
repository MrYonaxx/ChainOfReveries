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
    public class StatusEffectUpdateOnHit: StatusEffectUpdate
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public Vector2Int hitTime;

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

        int hit = 0;

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
        public StatusEffectUpdateOnHit()
        {

        }

        public StatusEffectUpdateOnHit(StatusEffectUpdateOnHit statusEffect)
        {
            hit = Random.Range(statusEffect.hitTime.x, statusEffect.hitTime.y);
            hitCondition = statusEffect.hitCondition;
            cardType = statusEffect.cardType;
            attackElement = statusEffect.attackElement;
        }

        public override StatusEffectUpdate Copy()
        {
            return new StatusEffectUpdateOnHit(this);
        }

        CharacterBase c; // tout allait bien, puis le système s'effondre, donc obligé de garder une référence du player ici pour
                         // dessiner les status
        public override void ApplyUpdate(CharacterBase character)
        {
            c = character;
            character.CharacterKnockback.OnHit += UpdateCall;
        }

        public override bool Update()
        {
            if (hit <= 0)
            {
                return false;
            }
            return true;
        }

        public void UpdateCall(DamageMessage attack)
        {
            if(hitCondition == true)
            {
                if (cardType > -1 && attack.attackType != cardType)
                {
                    return;
                }

                if (attackElement > -1 && attack.elements.Contains(attackElement) == false)
                {
                    return;
                }
            }
            hit -= 1;
            c.CharacterStatusController.RefreshStatus();
        }

        public override void Remove(CharacterBase character)
        {
            character.CharacterKnockback.OnHit -= UpdateCall;
        }

        public override int ValueToDraw()
        {
            return hit;
        }





#if UNITY_EDITOR
        private static IEnumerable SelectCardType()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("CardTypesData")[0]))
                .GetAllTypeID();
        }

        public static IEnumerable SelectCardElement()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("ElementTypeData")[0]))
                .GetAllTypeID();
        }
#endif





        #endregion

    }

} // #PROJECTNAME# namespace