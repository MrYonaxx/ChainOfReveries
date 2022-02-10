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
    public class StatusEffectAnimation : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        AnimationParticle animationObject;
        public AnimationParticle AnimationObject
        {
            get { return animationObject; }
        }

        AnimationParticle animation;
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
        public StatusEffectAnimation()
        {
        }

        public StatusEffectAnimation(StatusEffectAnimation data)
        {
            animationObject = data.AnimationObject;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectAnimation(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            animation = character.CreateAnimation(animationObject);
            animation.transform.SetParent(character.ParticlePoint);
        }


        public override void RemoveEffect(CharacterBase character)
        {
            animation.Destroy();
        }

        #endregion

    } 

} // #PROJECTNAME# namespace