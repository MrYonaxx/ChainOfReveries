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
    public class AttackC_SuperCancel: AttackComponent
    {
        [SerializeField]
        int maxCancel = 2;


        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            Color c = character.CharacterAction.CardTypes.GetColorType(attack.Card.GetCardType());
            AuraSpriteEffect aura = character.FeedbacksComponents.GetComponent<AuraSpriteEffect>();
            if (character.CharacterAction.PreviousAttackManager != null)
            {
                character.CharacterAction.SpecialCancelCount += 1;
                int cancel = character.CharacterAction.SpecialCancelCount;
                cancel = Mathf.Min(cancel, maxCancel);

                character.SetCharacterMotionSpeed(0.2f, 0.4f);
                BattleFeedbackManager.Instance.CameraSpecialZoom(character.CharacterAction.SpecialCancelCount);
                if (cancel == 1)
                {
                    BattleFeedbackManager.Instance.SetBattleMotionSpeed(0.1f, 0.6f);
                    aura.AuraFeedback(0.6f, 1.5f, c);
                }
                else if (cancel == 2)
                {
                    BattleFeedbackManager.Instance.SetBattleMotionSpeed(0.1f, 1f);
                    aura.AuraFeedback(1f, 1.6f, c);
                }
                else if (cancel == 3)
                {
                    BattleFeedbackManager.Instance.SetBattleMotionSpeed(0.1f, 1.5f);
                    aura.AuraFeedback(1.5f, 1.7f, c);
                }
            }
            else
            {
                aura.AuraFeedback(0.33f, 1.5f, c);
            }

        }


        /* public override void UpdateComponent(CharacterBase character)
         {

         }
         public override void OnHitComponent(CharacterBase character, CharacterBase target)
         {

         }
         public override void EndComponent(CharacterBase character)
         {

         }*/

    } 

} // #PROJECTNAME# namespace