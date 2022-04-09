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

            if (character.CharacterAction.PreviousAttackManager != null || character.State.ID == CharacterStateID.CardBreak)
            {
                character.CharacterAction.SpecialCancelCount += 1;
                int cancel = character.CharacterAction.SpecialCancelCount;
                cancel = Mathf.Min(cancel, maxCancel);

                if(character.State.ID != CharacterStateID.CardBreak) // pas de zoom quand on cancel
                    BattleFeedbackManager.Instance.CameraSpecialZoom(character.CharacterAction.SpecialCancelCount);

                if (cancel == 1)
                {
                    BattleFeedbackManager.Instance.SetBattleMotionSpeed(0.1f, 0.6f);
                    character.SetCharacterMotionSpeed(0.2f, 0.4f);
                    aura.AuraFeedback(0.6f, 1.5f, c);
                    BattleFeedbackManager.Instance.Speedlines(0.3f, c, character.ParticlePoint.position);
                }
                else if (cancel == 2)
                {
                    BattleFeedbackManager.Instance.SetBattleMotionSpeed(0.1f, 1f);
                    character.SetCharacterMotionSpeed(0.2f, 0.5f);
                    aura.AuraFeedback(1f, 1.6f, c);
                    BattleFeedbackManager.Instance.Speedlines(0.5f, c, character.ParticlePoint.position);
                }
                else if (cancel >= 3)
                {
                    BattleFeedbackManager.Instance.SetBattleMotionSpeed(0.1f, 1.5f);
                    character.SetCharacterMotionSpeed(0.2f, 0.6f);
                    aura.AuraFeedback(1.5f, 1.7f, c);
                    BattleFeedbackManager.Instance.Speedlines(0.75f, c, character.ParticlePoint.position);
                }


            }
            else
            {
                aura.AuraFeedback(0.33f, 1.5f, c);
            }

        }

    } 

} // #PROJECTNAME# namespace