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
    public class CharacterStateReversal : CharacterState
    {
        [SerializeField]
        CardBreakController cardBreakController;

        [SerializeField]
        AttackManager actionReversal;

        [SerializeField]
        float timeReversal = 30f;

        float t = 0f;
        float[] zoomValue;
        float[] zoomTime;

        private void Start()
        {
            zoomValue = new float[] { -0.5f, -0.5f, 0 };
            zoomTime = new float[] { 6f, timeReversal - 6f, 6f};
            timeReversal /= 60f;
        }

        public override void StartState(CharacterBase character, CharacterState oldState)
        {
            t = 0f;
            character.FeedbacksComponents.GetComponent<Feedbacks.ShakeSprite>().Shake(0.1f, timeReversal);
            character.CharacterKnockback.IsInvulnerable = true;
            character.CharacterMovement.SetSpeed(0, 0);
            character.CharacterAction.Action(actionReversal);

            BattleFeedbackManager.Instance?.SetBattleMotionSpeed(0, timeReversal);
            BattleFeedbackManager.Instance?.CameraZoom(zoomValue, zoomTime);

        }

        /// <summary>
        /// Update avant le check de collision
        /// Mettre les inputs ici
        /// </summary>
        /// <param name="character"></param>
        public override void UpdateState(CharacterBase character)
        {
            /*character.DeckController.MoveHand(character.Inputs.InputLB.InputValue == 1 ? true : false, character.Inputs.InputRB.InputValue == 1 ? true : false);

            t += Time.deltaTime;
            if (t >= timeReversal)
            {
                cardBreakController.ForceCardBreak(character);
                if (actionReversal != null)
                    character.CharacterAction.Action(actionReversal);
                else
                    character.ResetToIdle();
            }*/
        }

        public override void EndState(CharacterBase character, CharacterState oldState)
        {
            //character.CharacterKnockback.IsInvulnerable = false;
        }

    } 

} // #PROJECTNAME# namespace