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
using Feedbacks;

namespace VoiceActing
{
    public class AttackC_AuraEffect: AttackComponent
    {
        AuraSpriteEffect auraEffect;

        [SerializeField]
        bool auraUnbreakable = true;
        [SerializeField]
        bool auraCounter = false;
        [SerializeField]
        bool custom = false;

        [ShowIf("custom")]
        [SerializeField]
        float time = 1f;
        [ShowIf("custom")]
        [SerializeField]
        float scale = 1.5f;
        [ShowIf("custom")]
        [SerializeField]
        Color color = Color.green;

        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            // à cache si ça pose problème
            auraEffect = character.FeedbacksComponents.GetComponent<AuraSpriteEffect>();

            if (auraUnbreakable)
                auraEffect.AuraFeedbackReverse(0.2f, 1.2f, Color.red, true);
            else if (auraCounter)
                auraEffect.AuraFeedbackReverse(0.2f, 1.2f, Color.white, true);
            else if (custom)
                auraEffect.AuraFeedback(time, scale, color);
        }

        /*public override void UpdateComponent(CharacterBase character)
        {
            if(character.CharacterAction.card)
        }*/

        public override void EndComponent(CharacterBase character)
        {
            auraEffect.EndAuraFeedback();
        }





    } 

} // #PROJECTNAME# namespace