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

        [SerializeField]
        Menu.MenuWeaponSelection menuWeaponSelection = null;

        [Title("Description")]
        [SerializeField]
        float deadzone = 0.5f;
        [SerializeField]
        Menu.MenuCursor cursor = null;
        [SerializeField]
        Menu.Textbox textbox = null;

        [Title("Description")]
        [SerializeField]
        int hpRestore = 250;
        [SerializeField]
        TMPro.TextMeshProUGUI textHP = null;


        [Title("Deck Drawer")]
        [SerializeField]
        Menu.MenuDeckDrawer menuDeckDrawer = null;

        [Title("Animator")]
        [SerializeField]
        Canvas canvasReward = null;
        [SerializeField]
        Animator animatorBattleWin = null;
        [SerializeField]
        Animator animatorCardControllers = null;


        [Title("Sounds")]
        [SerializeField]
        SoundParameter soundVictory = null;
       /* [SerializeField]
        SoundParameter soundSelect = null;*/
        [SerializeField]
        SoundParameter soundValidate = null;
        [SerializeField]
        SoundParameter soundCardGet = null;

        private CharacterBase player;
        public delegate void Action();
        public event Action OnEventEnd;

        bool active = false;
        bool activeStick = false;
        bool focusDeckDrawer = false;
        bool equipmentActive = false;
        int keyboardSelection = 0;
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
            player.CharacterMovement.InMovement = false;

            canvasReward.gameObject.SetActive(true);
            animatorBattleWin.gameObject.SetActive(true);
            animatorBattleWin.SetBool("Appear", true);
            if (rewardEquipment)
                CreateRewardsEquipment();
            else
                CreateRewards();

            if (hpRestore > 0)
                textHP.text = "+" + hpRestore + " HP";
            else
                textHP.text = "Skip";

            keyboardSelection = 99;

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
            }

            // Si 2 cartes ont la même valeur plutot que de refaire un tirage en banissant les cartes qu'on a
            // et donc s'épargner 4 5 boucles et des copies de liste inutilement complexe, on transforme une des cartes en cartes premium
            for (int i = 0; i < cardRewards.Count - 1; i++)
            {
                for (int j = i+1; j < cardRewards.Count; j++)
                {
                    if (cardRewards[i].baseCardValue == cardRewards[j].baseCardValue && cardRewards[i].CardData == cardRewards[j].CardData)
                    {
                        cardRewards[j].CardPremium = true;
                        break;
                    }
                }
            }

            // Enfin on dessine
            for (int i = 0; i < rewardNumber; i++)
            {
                cardControllers[i].DrawCard(cardRewards[i], cardBattleType);
            }
        }

        private void CreateRewardsEquipment()
        {
            // à opti parce que c'est lourd un peu
            // On copie la database pour piocher 3 cartes équipements différentes
            List<CardEquipmentData> equipmentPool = equipmentDatabase.CopyCardDatabase(false);
            int r = 0;
            for (int i = 0; i < rewardNumber; i++)
            {
                r = Random.Range(0, equipmentPool.Count-1);
                cardRewards.Add(new CardEquipment(equipmentPool[r]));
                cardControllers[i].DrawCard(cardRewards[i].GetCardIcon(), cardEquipmentType.GetColorType(cardRewards[i].GetCardType()));
                equipmentPool.RemoveAt(r);
            }
        }





        private IEnumerator BattleRewardCoroutine()
        {
            soundVictory.PlaySound();
            yield return new WaitForSeconds(1f);
            animatorCardControllers.gameObject.SetActive(true);
            active = true;
            yield return new WaitForSeconds(1f);
            activeStick = true;
        }


        public void UpdateControl(InputController input)
        {
            if (active == false)
                return;

            // Input pour valider
            if (!GameSettings.Keyboard)
            {
                if (buttonHoldControllerA.HoldButton(input.InputA.InputValue == 1 ? true : false))
                    AddReward(99); // Skip
                else if (buttonHoldControllerX.HoldButton(input.InputX.InputValue == 1 ? true : false))
                    AddReward(1);
                else if (buttonHoldControllerY.HoldButton(input.InputY.InputValue == 1 ? true : false))
                    AddReward(0);
                else if (buttonHoldControllerB.HoldButton(input.InputB.InputValue == 1 ? true : false))
                    AddReward(2);
            }

            if (focusDeckDrawer == false)
            {
                if (activeStick == false)
                    return;

                UpdateKeyboard(input);

                // Input pour description
                if (input.InputLeftStickX.InputValue < -deadzone && Mathf.Abs(input.InputLeftStickY.InputValue) < deadzone)
                {
                    KeyboardSelec(1);
                    DescriptionReward(1); 
                }
                else if (input.InputLeftStickX.InputValue > deadzone && Mathf.Abs(input.InputLeftStickY.InputValue) < deadzone)
                {
                    DescriptionReward(2);
                    KeyboardSelec(2);
                }
                else if (input.InputLeftStickY.InputValue > deadzone && Mathf.Abs(input.InputLeftStickX.InputValue) < deadzone)
                {
                    DescriptionReward(0);
                    KeyboardSelec(0);
                }

                if (input.InputRB.Registered)
                {
                    DrawDeck();
                    focusDeckDrawer = true;
                    input.InputRB.ResetBuffer();
                }
            }
            else if (focusDeckDrawer) // le menu deck est ouvert
            {
                menuDeckDrawer.UpdateControl(input);

                if (input.InputRB.Registered)// && equipmentActive)
                {
                    // ferme le menu
                    menuDeckDrawer.gameObject.SetActive(false);
                    focusDeckDrawer = false;
                    input.InputRB.ResetBuffer();
                    equipmentActive = false;
                }
                /*else if (input.InputRB.Registered)
                {
                    // ouvre le menu equipement
                    equipmentActive = true;
                    DrawDeck(true);
                    input.InputRB.ResetBuffer();
                }*/
            }
                
        }



        public void AddReward(int index)
        {
            active = false;

            cursor.gameObject.SetActive(false);
            textbox.HideTextbox();
            menuDeckDrawer.gameObject.SetActive(false);

            if (index > rewardNumber)
            {
                // Skip
                animatorCardControllers.SetInteger("Reward", 0);
                player.CharacterStat.HP += hpRestore;
            }
            else
            {

                animatorCardControllers.SetInteger("Reward", index + 1);
                // Add reward
                if (rewardEquipment)
                {
                    CardEquipment reward = (CardEquipment)cardRewards[index];
                    if(!gameRunData.AddEquipmentCard(reward))
                    {
                        // Inventaire plein alerte
                        menuWeaponSelection.SetCard(player, reward);
                        menuWeaponSelection.InitializeMenu();
                        menuWeaponSelection.OnEnd += BackToReward;
                        player.Inputs.SetControllable(menuWeaponSelection, true);
                        animatorCardControllers.speed = 0; // manoeuvre artisanale
                        return;
                    }
                    player.CharacterEquipment.EquipCard(reward.CardEquipmentData, 1);
                }
                else
                {
                    gameRunData.AddCard(cardRewards[index]);
                }
            }
            animatorBattleWin.SetTrigger("Feedback");
            soundValidate.PlaySound();
        }

        private void BackToReward()
        {
            menuWeaponSelection.OnEnd -= BackToReward;
            animatorCardControllers.speed = 1;
        }

        public void DescriptionReward(int index)
        {
            textbox.gameObject.SetActive(true);
            cursor.gameObject.SetActive(true);
            cursor.MoveCursor(cardControllers[index].GetRectTransform().anchoredPosition);
            textbox.DrawTextbox(cardRewards[index].GetCardDescription());
        }

        // Appelé par un animator
        // Appelé à la fin de l'anim de animatorCardControllers
        public void AnimationGetoDaze()
        {
            // Joue animation carte
            player.Animator.Play("GetCard");
            soundCardGet.PlaySound();
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


        private void DrawDeck(bool equipment = false)
        {
            if (!equipment)
            {
                cursor.gameObject.SetActive(false);
                textbox.HideTextbox();
                menuDeckDrawer.gameObject.SetActive(true);
                menuDeckDrawer.DrawDeck(gameRunData.PlayerDeck);
            }
            else
            {
                List<Card> deckEquipment = new List<Card>();
                for (int i = 0; i < gameRunData.PlayerEquipmentDeck.Count; i++)
                {
                    deckEquipment.Add(gameRunData.PlayerEquipmentDeck[i]);
                }
                //menuDeckDrawer.CardTypeData
                menuDeckDrawer.DrawDeck(deckEquipment);
            }
        }

        private void KeyboardSelec(int index)
        {
            keyboardSelection = index;
        }

        private void UpdateKeyboard(InputController input)
        {
            if(GameSettings.Keyboard)
            {
                if(input.InputA.InputValue == 1)
                {
                    /*if (keyboardSelection == 3)
                        AddReward(99); // Skip*/
                    if (keyboardSelection == 1)
                        AddReward(1);
                    else if (keyboardSelection == 0)
                        AddReward(0);
                    else if (keyboardSelection == 2)
                        AddReward(2);
                }
                else if (input.InputB.Registered)
                {
                    AddReward(99);  
                }
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace