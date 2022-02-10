/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class BattleReward: MonoBehaviour, IControllable
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Data")]
        [SerializeField]
        GameRunData gameRunData = null;
        [SerializeField]
        CardType cardBattleType = null;


        [Title("Parameter")]
        [SerializeField]
        int rewardNumber = 3;
        [SerializeField]
        bool rewardEquipment = false;
        [SerializeField]
        bool rewardPremium = false;

        [SerializeField]
        [ShowIf("rewardEquipment")]
        [HorizontalGroup("Equipment")]
        [HideLabel]
        CardEquipmentDatabase equipmentDatabase = null;
        [SerializeField]
        [ShowIf("rewardEquipment")]
        [HorizontalGroup("Equipment")]
        [HideLabel]
        CardType cardEquipmentType = null;

        [Space]
        [SerializeField]
        CardController[] cardControllers;
        [SerializeField]
        ButtonHoldController buttonHoldControllerA = null;
        [SerializeField]
        ButtonHoldController buttonHoldControllerX = null;
        [SerializeField] 
        ButtonHoldController buttonHoldControllerY = null;
        [SerializeField] 
        ButtonHoldController buttonHoldControllerB = null;

        [Title("Description")]
        [SerializeField]
        float deadzone = 0.5f;
        [SerializeField]
        GameObject cursor = null;
        [SerializeField]
        Menu.Textbox textbox = null;

        [Title("Animator")]
        [SerializeField]
        Canvas canvasReward = null;
        [SerializeField]
        Animator animatorBattleWin = null;
        [SerializeField]
        Animator animatorCardControllers = null;

        private CharacterBase player;
        public delegate void Action();
        public event Action OnEventEnd;

        bool active = false;
        List<Card> cardRewards = new List<Card>();

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

        public void InitializeBattleReward(CharacterBase character = null)
        {
            buttonHoldControllerA.ResetButton();
            buttonHoldControllerX.ResetButton();
            buttonHoldControllerY.ResetButton();
            buttonHoldControllerB.ResetButton();

            player = character;

            canvasReward.gameObject.SetActive(true);
            animatorBattleWin.gameObject.SetActive(true);
            animatorBattleWin.SetBool("Appear", true);
            if (rewardEquipment)
                CreateRewardsEquipment();
            else
                CreateRewards();
            StartCoroutine(BattleRewardCoroutine());
        }


        // Create Rewards
        private void CreateRewards()
        {
            for(int i = 0; i < rewardNumber; i++)
            {
                cardRewards.Add(gameRunData.PlayerCharacterData.CreateCharacterNewCard());
                if (rewardPremium)
                    cardRewards[i].CardPremium = true;
                cardControllers[i].DrawCard(cardRewards[i], cardBattleType);
            }

            // sécurité pour que les cartes n'aient jamais la même valeur
            for (int i = 0; i < cardRewards.Count - 1; i++)
            {
                for (int j = i+1; j < cardRewards.Count; j++)
                {
                    if (cardRewards[i].baseCardValue == cardRewards[j].baseCardValue)
                    {
                        cardRewards[i].baseCardValue += j;
                    }
                }
                if (cardRewards[i].baseCardValue > 9)
                    cardRewards[i].baseCardValue -= 10;
                cardRewards[i].ResetCard();
            }
        }

        private void CreateRewardsEquipment()
        {
            for (int i = 0; i < rewardNumber; i++)
            {
                cardRewards.Add(new CardEquipment(equipmentDatabase.GachaEquipment()));
                cardControllers[i].DrawCard(cardRewards[i].GetCardIcon(), cardEquipmentType.GetColorType(cardRewards[i].GetCardType()));
            }
        }





        private IEnumerator BattleRewardCoroutine()
        {
            yield return new WaitForSeconds(1f);
            animatorCardControllers.gameObject.SetActive(true);
            //yield return new WaitForSeconds(1f);
            active = true;
        }


        public void UpdateControl(InputController input)
        {
            if (active == false)
                return;

            // Input pour valider
            if (buttonHoldControllerA.HoldButton(input.InputA.InputValue == 1 ? true : false)) 
                AddReward(99); // Skip
            else if (buttonHoldControllerX.HoldButton(input.InputX.InputValue == 1 ? true : false))
                AddReward(1);
            else if (buttonHoldControllerY.HoldButton(input.InputY.InputValue == 1 ? true : false))
                AddReward(0);
            else if (buttonHoldControllerB.HoldButton(input.InputB.InputValue == 1 ? true : false))
                AddReward(2);
            else
            {
                // Input pour description
                if (input.InputLeftStickX.InputValue < -deadzone && Mathf.Abs(input.InputLeftStickY.InputValue) < deadzone)
                    DescriptionReward(1);
                else if (input.InputLeftStickX.InputValue > deadzone && Mathf.Abs(input.InputLeftStickY.InputValue) < deadzone)
                    DescriptionReward(2);
                else if (input.InputLeftStickY.InputValue > deadzone && Mathf.Abs(input.InputLeftStickX.InputValue) < deadzone)
                    DescriptionReward(0);
            }
        }



        public void AddReward(int index)
        {
            active = false;
            if (index > rewardNumber)
            {
                // Skip
                animatorBattleWin.SetTrigger("Feedback");
                animatorCardControllers.SetInteger("Reward", 0);
            }
            else
            {
                animatorBattleWin.SetTrigger("Feedback");
                animatorCardControllers.SetInteger("Reward", index+1);

                // Add reward
                if (rewardEquipment)
                {
                    CardEquipment reward = (CardEquipment)cardRewards[index];
                    gameRunData.AddEquipmentCard(reward);
                    player.CharacterEquipment.EquipCard(reward.CardEquipmentData, 1);
                }
                else
                {
                    gameRunData.AddCard(cardRewards[index]);
                }
            }
            cursor.gameObject.SetActive(false);
            textbox.HideTextbox();
        }

        public void DescriptionReward(int index)
        {
            textbox.gameObject.SetActive(true);
            cursor.gameObject.SetActive(true);
            cursor.transform.position = cardControllers[index].transform.position;
            textbox.DrawTextbox(cardRewards[index].GetCardDescription());
        }

        // Appelé à la fin de l'anim de animatorCardControllers
        public void AnimationGetoDaze()
        {
            // Joue animation carte
            player.Animator.Play("GetCard");

            animatorBattleWin.SetBool("Appear", false);
            animatorCardControllers.SetInteger("Reward", 0);
            animatorCardControllers.gameObject.SetActive(false);
            StartCoroutine(BattleRewardEndCoroutine());
        }

        private IEnumerator BattleRewardEndCoroutine()
        {
            yield return new WaitForSeconds(1f);
            animatorBattleWin.gameObject.SetActive(false);
            canvasReward.gameObject.SetActive(false);
            OnEventEnd.Invoke();
        }

        #endregion

    } 

} // #PROJECTNAME# namespace