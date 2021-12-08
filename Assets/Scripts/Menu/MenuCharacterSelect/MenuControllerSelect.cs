using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Rewired;

namespace VoiceActing
{
    public class MenuControllerSelect : MonoBehaviour, IControllable
    {
        int player1ID;
        int player2ID;

        [SerializeField]
        bool onlySlot1 = false;
        [SerializeField]
        float timeCardMove = 1;

        [Title("UI")]
        [SerializeField]
        RectTransform slot1 = null;
        [SerializeField]
        RectTransform slot2 = null;
        [SerializeField]
        GameObject buttonStart = null;

        [SerializeField]
        List<RectTransform> cardSlots = null;
        [SerializeField]
        List<CardController> cardControllers = null;
        [SerializeField]
        List<InputController> inputControllers = null;

        List<bool> joystickInput;

        void Start()
        {
            Setup();
        }

        void Setup()
        {
            player1ID = -1;
            player2ID = -1;

            buttonStart.gameObject.SetActive(false);

            joystickInput = new List<bool>();
            for (int i = 0; i < cardControllers.Count; i++)
            {
                if (ReInput.players.Players[i].controllers.hasKeyboard || ReInput.players.Players[i].controllers.joystickCount != 0)
                {
                    cardControllers[i].gameObject.SetActive(true);
                    cardControllers[i].DrawCardValue(i+1);
                    inputControllers[i].SetID(i);
                    inputControllers[i].SetControllable(this);
                    joystickInput.Add(false);
                }
                else
                {
                    cardControllers[i].gameObject.SetActive(false);
                    cardSlots[i].gameObject.SetActive(false);
                    inputControllers[i].SetID(-1);
                    inputControllers[i].SetControllable(null);
                }
            }
        }

        // Update is called once per frame
        public void UpdateControl(InputController input)
        {
            if (joystickInput[input.Id])
            {
                if (input.InputLeftStickX.InputValue == 0)
                    joystickInput[input.Id] = false;
            }
            if (joystickInput[input.Id] == false)
            {
                if (input.InputLeftStickX.InputValue < 0 && player2ID == input.Id)
                {
                    // Reset Player 2
                    cardControllers[input.Id].MoveCard(cardSlots[input.Id], timeCardMove);
                    player2ID = -1;
                    joystickInput[input.Id] = true;
                }
                else if (input.InputLeftStickX.InputValue < 0 && player1ID == -1)
                {
                    // Set player 1
                    cardControllers[input.Id].MoveCard(slot1, timeCardMove);
                    player1ID = input.Id;
                    joystickInput[input.Id] = true;
                    CheckNextButton();
                }
                else if (input.InputLeftStickX.InputValue > 0 && player1ID == input.Id)
                {
                    // Reset player 1
                    cardControllers[input.Id].MoveCard(cardSlots[input.Id], timeCardMove);
                    player1ID = -1;
                    joystickInput[input.Id] = true;
                }
                else if (input.InputLeftStickX.InputValue > 0 && player2ID == -1)
                {
                    // Set player 2
                    cardControllers[input.Id].MoveCard(slot2, timeCardMove);
                    player2ID = input.Id;
                    joystickInput[input.Id] = true;
                    CheckNextButton();
                }
            }

            if(player1ID != -1 && player2ID != -1 && input.InputA.Registered)
            {
                // Go versus
            } 
            else if (input.InputB.Registered)
            {
                // Return
            }
        }

        private void CheckNextButton()
        {
            if (onlySlot1)
            {

            }
            else
            {
                if (player1ID != -1 && player2ID != -1)
                    buttonStart.gameObject.SetActive(true);
                else
                    buttonStart.gameObject.SetActive(false);
            }
        }
    }
}
