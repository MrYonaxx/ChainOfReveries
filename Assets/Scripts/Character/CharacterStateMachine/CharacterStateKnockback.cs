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
    // Quand le joueur subit des dégâts
    public class CharacterStateKnockback : CharacterState
    {
        [Title("States")]
        [SerializeField]
        CharacterState stateDown = null;
        [SerializeField]
        CharacterState stateDeath = null;

        [SerializeField]
        AttackManager evadeAction = null;
        [SerializeField]
        [SuffixLabel("en frames")]
        float timeDown = 30;


        [Title("Friction")]
        [SerializeField]
        AnimationCurve groundFriction;
        [SerializeField]
        [SuffixLabel("en frames")]
        float timeFriction = 30;
        [SerializeField]
        float airFriction = 0.1f;



        float tFriction = 0;
        float knockbackX = 0;
        bool inAir = false;

        private void Start()
        {
            timeFriction /= 60f;
            timeDown /= 60f;
        }

        public override void StartState(CharacterBase character, CharacterState oldState)
        {
            character.CharacterAction.RemoveCards();
            character.CharacterAction.CancelAction();

            tFriction = 0f;
            knockbackX = character.CharacterMovement.SpeedX;
            character.CharacterKnockback.ReversalTime = 0;
            inAir = character.CharacterMovement.InAir;

        }

        /// <summary>
        /// Update avant le check de collision
        /// Mettre les inputs ici
        /// </summary>
        /// <param name="character"></param>
        public override void UpdateState(CharacterBase character)
        {
            // On meurt si on touche le sol
            if (character.CharacterMovement.InAir == false && character.CharacterKnockback.IsDead)
            {
                character.SetState(stateDeath);
                return;
            }

            // Update knockback movement
            if (character.CharacterMovement.InAir)
            {
                if (Mathf.Abs(knockbackX) < (airFriction * Time.deltaTime * character.MotionSpeed) * 2)
                    knockbackX = 0;
                else
                    knockbackX -= (airFriction * Mathf.Sign(knockbackX)) * Time.deltaTime * character.MotionSpeed;
                character.CharacterMovement.ApplyGravity(character.MotionSpeed);
                character.CharacterMovement.Move(knockbackX, 0);
            }
            else
            {
                tFriction += Time.deltaTime * character.MotionSpeed;
                character.CharacterMovement.Move(groundFriction.Evaluate(tFriction / timeFriction) * knockbackX, 0);
            }

            if(character.CharacterKnockback.IsDead)
                return;

            // Inputs
            character.DeckController.MoveHand(character.Inputs.InputLB.InputValue == 1 ? true : false, character.Inputs.InputRB.InputValue == 1 ? true : false);
            character.DeckController.MoveCategory(character.Inputs.InputLT.InputValue == 1 ? true : false, character.Inputs.InputRT.InputValue == 1 ? true : false);
            InputSleight(character);

            // Update Knockback Time
            character.CharacterKnockback.KnockbackTime -= Time.deltaTime * character.MotionSpeed;

            if (inAir == true && character.CharacterMovement.InAir == false) 
            {
                inAir = false;
                if (InputEvade(character)) // On tech
                {
                    character.CharacterKnockback.KnockbackTime = 0;
                    return;
                }
                if (character.CharacterKnockback.Knockdown) // On s'écrase
                {
                    character.SetState(stateDown);
                    return;
                }
                else
                {
                    character.CharacterKnockback.KnockbackTime = Mathf.Max(timeDown, character.CharacterKnockback.KnockbackTime);
                }
            }


            if(character.CharacterKnockback.KnockbackTime <= 0)
            {
                character.CharacterKnockback.KnockbackTime = 0;
                character.ResetToIdle();
            }
            else
            {
                Revenge(character);
            }
        }


        // Wall bounce
        public override void LateUpdateState(CharacterBase character)
        {
            if(character.CharacterKnockback.Knockdown && character.CharacterRigidbody.WallHorizontalCollision && character.CharacterMovement.InAir)
            {
                WallBounce(character);

            }
        }

        public override void EndState(CharacterBase character, CharacterState oldState)
        {
            inAir = false;
            character.CharacterKnockback.ReversalTime = 0;
        }




        private bool InputEvade(CharacterBase character)
        {
            if (character.Inputs.InputX.Registered)
            {
                character.CharacterAction.Action(evadeAction);
                return true;
            }
            return false;
        }


        private void Revenge(CharacterBase character)
        {
            character.CharacterKnockback.CanRevenge = character.Inputs.InputA.InputValue == 1 ? true : false;
        }

        /*private bool InputDpad(CharacterBase character)
        {
            if (character.Inputs.InputPadDown.Registered || character.Inputs.InputPadUp.Registered)
            {
                character.Inputs.InputPadDown.ResetBuffer();
                character.Inputs.InputPadUp.ResetBuffer();
                character.CharacterEquipment.SwitchToEquipmentDeck(!character.CharacterEquipment.InEquipmentDeck);
                if (character.CharacterEquipment.InEquipmentDeck == true)
                {
                    character.CharacterEquipment.DeckEquipmentController.RefreshDeck();
                }
                else
                {
                    character.DeckController.RefreshDeck();
                }
                return true;
            }
            return false;
        }*/

        private void WallBounce(CharacterBase character)
        {
            character.FeedbacksComponents.GetComponent<Feedbacks.ShakeSprite>().Shake(0.2f, 0.2f);
            character.SetCharacterMotionSpeed(0, 0.1f);
            character.CharacterMovement.TurnBack();
            knockbackX = -knockbackX;
            character.CharacterMovement.Jump(3f);
        }

        // Permet de préparer une sleight mais pas de la joueur
        private void InputSleight(CharacterBase character)
        {
            if(character.Inputs.InputY.Registered)
            {
                character.Inputs.InputY.ResetBuffer();
                character.CharacterAction.PlaySleight(false); // On stock une carte mais en aucun cas on joue une sleight
            }
        }

    } 

} // #PROJECTNAME# namespace