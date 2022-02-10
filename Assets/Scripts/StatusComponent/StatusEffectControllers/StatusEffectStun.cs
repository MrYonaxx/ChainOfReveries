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
    public class StatusEffectStun: StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        AnimationParticle stunAnimation;
        public AnimationParticle StunAnimation
        {
            get { return stunAnimation; }
        }

        AnimationParticle stunObject;
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
        public StatusEffectStun(StatusEffectStun data)
        {
            stunAnimation = data.StunAnimation;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectStun(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            stunObject = character.CreateAnimation(stunAnimation);
            stunObject.transform.SetParent(character.ParticlePoint);
            character.CharacterKnockback.Knockback(1, true);
        }

        public override void UpdateEffect(CharacterBase character)
        {
            character.CharacterKnockback.KnockbackTime = 1f;
            character.CharacterMovement.SetSpeed(0, 0);
        }

        public override void RemoveEffect(CharacterBase character)
        {
            stunObject.Destroy();
        }

        #endregion

    } 

} // #PROJECTNAME# namespace