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
    public class StatusEffectPoison: StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        AnimationParticle poisonAnimation = null;
        public AnimationParticle PoisonAnimation
        {
            get { return poisonAnimation; }
        }

        [SerializeField]
        float damagePoison = 100f;
        public float DamagePoison
        {
            get { return damagePoison; }
        }
        [SerializeField]
        float timeInterval = 1f;
        public float TimeInterval
        {
            get { return timeInterval; }
        }

        float t = 0f;

        //AnimationParticle poisonObject;
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
        public StatusEffectPoison()
        {

        }

        public StatusEffectPoison(StatusEffectPoison data)
        {
            poisonAnimation = data.PoisonAnimation;
            timeInterval = data.TimeInterval;
            damagePoison = data.DamagePoison;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectPoison(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            t = 0f;
        }

        public override void UpdateEffect(CharacterBase character)
        {
            t += Time.deltaTime * character.MotionSpeed;
            if(t >= timeInterval)
            {
                t = 0f;
                if(character.CharacterStat.HP > damagePoison)
                    character.CharacterStat.HP -= damagePoison;

                if (poisonAnimation != null)
                {
                    AnimationParticle poisonObject = character.CreateAnimation(poisonAnimation);
                    poisonObject.transform.SetParent(character.ParticlePoint);
                }
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace