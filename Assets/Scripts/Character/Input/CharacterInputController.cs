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
    public class CharacterInputController: CharacterController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        bool canControl = true;


        ControllerLayout controllerLayout; // Les controles personnalisé du joueur (on s'en occupera plus tard)
        [SerializeField]
        InputController inputController;

        

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



        public override void UpdateCharacter(CharacterBase character)
        {
            // J'update 
            if (canControl == false)
                return;

            //inputController.UpdateInput();

            /*switch (character.State)
            {
                case CharacterState.Idle:
                    InputMovement(character);
                    InputPlayCard(character);
                    InputSleight(character);
                    InputMoveDeck(character);
                    break;

                case CharacterState.Acting:

                    InputPlayCard(character);
                    InputSleight(character);
                    InputMoveDeck(character);
                    break;
            }*/

        }

       /* private void InputMovement(CharacterBase character)
        {
            Vector2 move = new Vector2(inputController.InputLeftStickX.InputValue, inputController.InputLeftStickY.InputValue);
            move.Normalize();

            if(move.x != 0)
                character.CharacterMovement.SetDirection((int)Mathf.Sign(inputController.InputLeftStickX.InputValue));
            character.CharacterMovement.MoveDirection(move.x, move.y);
        }

        private void InputMoveDeck(CharacterBase character)
        {
            if (inputController.InputRT.BufferTime > 0)
            {
                character.CharacterAction.DeckController.MoveHandLeft();
                character.CharacterAction.DeckController.MoveHandLeft();
                inputController.InputRT.ResetBuffer();
            }

            if (inputController.InputLT.BufferTime > 0)
            {
                character.CharacterAction.DeckController.MoveHandRight();
                character.CharacterAction.DeckController.MoveHandRight();
                inputController.InputLT.ResetBuffer();
            }
        }
        */
        public void SetCanControl(bool b)
        {
            canControl = b;
        }


        private void InputPlayCard(CharacterBase character)
        {
           /* if (inputController.InputA.BufferTime > 0)
            {
                if(character.CharacterAction.PlayCard() == true)
                    inputController.ResetAllBuffer();
            }*/


            /*if (bufferActive == true && state == CharacterState.Acting)
            {
                if (character.CharacterAction.DeckController.GetCanPlayCard() == true && deckController.GetCurrentIndex() != 0)
                {
                    PlayCard();
                }
            }
            else if (inputController.InputA.BufferTime > 0)
            {
                if (deckController.GetCanPlayCard() == true)
                {
                    if (deckController.GetCurrentIndex() == 0)
                    {
                        if (state == CharacterState.Idle || state == CharacterState.Moving || state == CharacterState.Reload)
                        {
                            speedX = 0;
                            speedY = 0;
                            ReloadDeck();
                        }
                    }
                    else if (Input.GetButtonDown("ControllerA"))
                    {
                        if (CanAct() == true)
                        {
                            PlayCard();
                        }
                        else
                        {
                            if (bufferCoroutine != null)
                                StopCoroutine(bufferCoroutine);
                            bufferCoroutine = FrameBufferCoroutine();
                            StartCoroutine(bufferCoroutine);
                        }
                    }
                }
                else if (state == CharacterState.Reload)
                {
                    StopReload();
                }
            }
            else if (state == CharacterState.Reload && Input.GetButtonUp("ControllerA"))
            {
                StopReload();
            }*/
        }


        private void InputSleight(CharacterBase character)
        {
            /*if (inputController.InputY.BufferTime > 0 && character.CharacterAction.DeckController.GetCurrentIndex() != 0)
            {
                if (character.CharacterAction.PlaySleight() == true)
                    inputController.ResetAllBuffer();
            }*/
        }


        #endregion

    }

} // #PROJECTNAME# namespace