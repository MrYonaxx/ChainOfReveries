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
        GameData gameData = null;

        [SerializeField]
        bool onlySlot1 = false;
        public bool OnlySlot1
        {
            set { onlySlot1 = value; }
        }

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

        [SerializeField]
        Animator animatorMenu = null;
        [SerializeField]
        Sprite keyboardSprite = null;

        [Title("Sounds")]
        [SerializeField]
        SoundParameter soundSelect;

        List<bool> joystickInput;
        bool ready = false;

        public delegate void EventVoid();
        public event EventVoid OnStart;
        public event EventVoid OnEnd;



        public void Setup()
        {
            ActivateInput(true);

            player1ID = -1;
            player2ID = -1;
            ready = false;

            buttonStart.gameObject.SetActive(false);

            joystickInput = new List<bool>();
            for (int i = 0; i < cardControllers.Count; i++)
            {
                if (ReInput.players.Players[i].controllers.hasKeyboard || ReInput.players.Players[i].controllers.joystickCount != 0)
                {
                    // Ajoute le controller aux listes
                    cardControllers[i].gameObject.SetActive(true);
                    cardControllers[i].DrawCardValue(i+1);
                    inputControllers[i].SetID(i);
                    inputControllers[i].SetControllable(this, true);
                    joystickInput.Add(false);

                    if(ReInput.players.Players[i].controllers.hasKeyboard)
                    {
                        cardControllers[i].DrawCard(keyboardSprite, Color.black, i+1);
                    }
                }
                else
                {
                    cardControllers[i].gameObject.SetActive(false);
                    cardSlots[i].gameObject.SetActive(false);
                    inputControllers[i].SetID(-1);
                    inputControllers[i].SetControllable(null, true);
                }
            }

            slot2.gameObject.SetActive(!onlySlot1);
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
                if (input.InputLeftStickX.InputValue < 0 && player2ID == input.Id && !onlySlot1)
                {
                    // Reset Player 2
                    cardControllers[input.Id].MoveCard(cardSlots[input.Id], timeCardMove);
                    player2ID = -1;
                    joystickInput[input.Id] = true;
                    CheckNextButton();
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
                    CheckNextButton();
                }
                else if (input.InputLeftStickX.InputValue > 0 && player2ID == -1 && !onlySlot1)
                {
                    // Set player 2
                    cardControllers[input.Id].MoveCard(slot2, timeCardMove);
                    player2ID = input.Id;
                    joystickInput[input.Id] = true;
                    CheckNextButton();
                }
            }

            if(ready && input.InputA.Registered)
            {
                input.ResetAllBuffer();
                ActivateInput(false);

                // Set Input ID
                gameData.SetControllerID(1, player1ID);
                gameData.SetControllerID(2, player2ID);

                GameSettings.Keyboard = false;
                // Ici on reset la config manette si on est au clavier pour ne pas désorienter le joueur clavier 
                if (ReInput.players.Players[player1ID].controllers.hasKeyboard)
                {
                    gameData.GetInputConfig(1).Reset();
                    GameSettings.Keyboard = true;
                }
                if (player2ID != -1)
                {
                    if (ReInput.players.Players[player2ID].controllers.hasKeyboard)
                        gameData.GetInputConfig(2).Reset();
                }

                OnStart?.Invoke();
            } 
            else if (input.InputB.Registered || (GameSettings.Keyboard && input.InputY.Registered))
            {
                // Return
                input.ResetAllBuffer();
                ActivateInput(false);
                OnEnd?.Invoke();
            }
        }

        private void CheckNextButton()
        {
            if (onlySlot1)
            {
                ready = (player1ID != -1);
                if (player1ID != -1)
                    buttonStart.gameObject.SetActive(true);
                else
                    buttonStart.gameObject.SetActive(false);
            }
            else
            {
                ready = (player1ID != -1 && player2ID != -1);
                if (player1ID != -1 && player2ID != -1)
                    buttonStart.gameObject.SetActive(true);
                else
                    buttonStart.gameObject.SetActive(false);
            }
            soundSelect.PlaySound();
        }

        private void ActivateInput(bool b)
        {
            animatorMenu.gameObject.SetActive(true);
            animatorMenu.SetBool("Appear", b);
            for (int i = 0; i < inputControllers.Count; i++)
            {
                inputControllers[i].enabled = b;
            }
        }
    }
}
