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
        float stunTime;
        public float StunTime
        {
            get { return stunTime; }
        }

        [SerializeField]
        AnimationParticle stunAnimation;
        public AnimationParticle StunAnimation
        {
            get { return stunAnimation; }
        }

        bool once = true;
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
            stunTime = data.StunTime;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectStun(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            once = true;
            stunObject = character.CreateAnimation(stunAnimation);
            stunObject.transform.SetParent(character.ParticlePoint);
            character.CharacterKnockback.Knockback(1, true);
        }

        public override void UpdateEffect(CharacterBase character)
        {
            stunTime -= Time.deltaTime * character.MotionSpeed;
            character.CharacterKnockback.KnockbackTime = stunTime;
            character.CharacterMovement.SetSpeed(0, 0);
        }

        public override void RemoveEffect(CharacterBase character)
        {
            stunObject.Destroy();
        }

        #endregion

    } 

} // #PROJECTNAME# namespace