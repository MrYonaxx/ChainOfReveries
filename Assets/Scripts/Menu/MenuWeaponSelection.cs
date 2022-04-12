using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;
using Sirenix.OdinInspector;

namespace Menu
{


    public class MenuWeaponSelection : MenuBase, IControllable
    {

        [SerializeField]
        GameRunData runData = null;
        [SerializeField]
        CardType cardTypeData = null;


        [Title("Cards")]
        [SerializeField]
        float cardMoveSpeed = 0.1f;
        [SerializeField]
        CardController centerCard = null;
        [SerializeField]
        RectTransform centerSlot = null;
        [SerializeField]
        CardController[] cardController = null;
        [SerializeField]
        RectTransform[] cardSlot = null;


        [SerializeField]
        ButtonHoldController holdController = null;

        [Title("UI")]
        [SerializeField]
        MenuCursor menuCursor = null;
        [SerializeField]
        Textbox textCardDescription = null;
        [SerializeField]
        Animator animatorMenu = null;

        CharacterBase character = null;
        Card cardToDiscard = null;
        List<Card> weaponEquipment = new List<Card>();
        List<int> weaponPosition = new List<int>();
        int indexSelected = -1; 


        public void SetCard(CharacterBase chara, CardEquipment card)
        {
            character = chara;
            cardToDiscard = card;
        }

        public override void InitializeMenu()
        {
            weaponEquipment = new List<Card>();
            int j = 0;
            for (int i = 0; i < runData.PlayerEquipmentDeck.Count; i++)
            {
                if (runData.PlayerEquipmentDeck[i].CardEquipmentData.EquipmentAction != null)
                {
                    character.CharacterEquipment.UnequipCard(runData.PlayerEquipmentDeck[i].CardEquipmentData, 0);
                    weaponEquipment.Add(runData.PlayerEquipmentDeck[i]);
                    weaponPosition.Add(i);

                    cardController[j].DrawCard(weaponEquipment[j], cardTypeData);
                    j++;
                }
            }

            centerCard.DrawCard(cardToDiscard, cardTypeData);
            gameObject.SetActive(true);
            animatorMenu.SetBool("Appear", true);

            base.InitializeMenu();
        }

        public override void UpdateControl(InputController input)
        {
            // 1 = Bas, 4 = Gauche, 6 = Droite, 8 = UP
            if (input.InputPadDown.Registered)
            {
                input.ResetAllBuffer();
                SwitchCard(2);
            }
            else if (input.InputPadLeft.Registered)
            {
                input.ResetAllBuffer();
                SwitchCard(4);
            }
            else if (input.InputPadRight.Registered)
            {
                input.ResetAllBuffer();
                SwitchCard(6);
            }
            else if (input.InputPadUp.Registered)
            {
                input.ResetAllBuffer();
                SwitchCard(8);
            }
            else if (holdController.HoldButton(input.InputB.InputValue == 1))
            {
                DiscardCard();
            }

            // Description
            if (input.InputLeftStickY.InputValue < 0)
            {
                SelectCard(2);
            }
            else if(input.InputLeftStickX.InputValue > 0)
            {
                SelectCard(6);
            }
            else if (input.InputLeftStickX.InputValue < 0)
            {
                SelectCard(4);
            }
            else if (input.InputLeftStickY.InputValue > 0)
            {
                SelectCard(8);
            }
            else
            {
                menuCursor.gameObject.SetActive(false);
                textCardDescription.HideTextbox();
            }
        }


        private void SwitchCard(int index)
        {
            index = (index / 2) - 1;

            Card tmp = cardToDiscard;
            cardToDiscard = weaponEquipment[index];
            weaponEquipment[index] = tmp;

            centerCard.DrawCard(cardToDiscard, cardTypeData);
            centerCard.transform.position = cardSlot[index].transform.position;
            centerCard.MoveCard(centerSlot, cardMoveSpeed);

            cardController[index].DrawCard(weaponEquipment[index], cardTypeData);
            cardController[index].transform.position = centerSlot.transform.position;
            cardController[index].MoveCard(cardSlot[index], cardMoveSpeed);
        }

        private void SelectCard(int index)
        {
            index = (index / 2) - 1;
            menuCursor.MoveCursor(cardSlot[index].anchoredPosition);
            menuCursor.gameObject.SetActive(true);

            textCardDescription.DrawTextbox(weaponEquipment[index].GetCardDescription());

        }


        private void DiscardCard()
        {
            for (int i = 0; i < weaponEquipment.Count; i++)
            {
                runData.PlayerEquipmentDeck[weaponPosition[i]] = (CardEquipment) weaponEquipment[i];
                character.CharacterEquipment.EquipCard(runData.PlayerEquipmentDeck[weaponPosition[i]].CardEquipmentData);
            }
            StartCoroutine(DiscardCoroutine());
        }

        private IEnumerator DiscardCoroutine()
        {
            centerCard.Disappear();
            yield return new WaitForSeconds(0.5f);
            animatorMenu.SetBool("Appear", false);
            yield return new WaitForSeconds(0.5f);
            QuitMenu();
        }
    }
}
