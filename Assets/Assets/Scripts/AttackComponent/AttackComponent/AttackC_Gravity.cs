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
    // Cherche le composant Gravity Effect, si il existe on applique un effet gravity, sinon tant pis (Les boss par exemple ne doivent pas se faire appliquer l'effet gravity, donc on ne leur ajoute pas le composant)
    public class AttackC_Gravity: AttackComponent
    {
        [SerializeField]
        float timeTransition;
        [SerializeField]
        float timeFlat;
        public override void OnHitComponent(CharacterBase character, CharacterBase target)
        {
            base.OnHitComponent(character, target);
            GravityEffect gravityEffect = target.FeedbacksComponents.GetComponent<GravityEffect>();
            if (gravityEffect != null)
                gravityEffect.Gravity(timeTransition * target.CharacterStat.KnockbackTime.Value, timeFlat * target.CharacterStat.KnockbackTime.Value);

        }

    } 

} // #PROJECTNAME# namespace