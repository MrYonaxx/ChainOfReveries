using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace VoiceActing
{
    enum CardValueHUDType
    {
        Player,
        Boss,
        Enemy
    }

    public class CharacterCardValueHUD : MonoBehaviour
    {
        [SerializeField]
        CharacterBase character = null;
        [SerializeField]
        CardBreakController cardBreakController = null;
        [SerializeField]
        TextMeshPro textMesh = null;

        [SerializeField]
        TextMeshPro textMeshFeedback = null;
        [SerializeField]
        Animator animatorText = null;

        [SerializeField]
        CardValueHUDType characterType;
        [SerializeField]
        Color colorPlay;
        [SerializeField]
        Color colorHand;

        [Header("Cards")]
        [SerializeField]
        SpriteRenderer cardOutline;
        [SerializeField]
        SpriteRenderer cardSprite;

        private void Start()
        {
            character.OnBattleStart += Initialize;
            character.OnBattleEnd += Hide;
            Initialize();
        }

        private void OnDestroy()
        {
            character.OnBattleStart -= Initialize;
            character.OnBattleEnd -= Hide;
        }


        private void Initialize()
        {
            int setting = PlayerPrefs.GetInt("CardValue", 1);
            if(setting <= 2 && characterType == CardValueHUDType.Player)
            {
                Hide();
            }
            else if (setting <= 1 && characterType == CardValueHUDType.Boss)
            {
                Hide();
            }
            else if (setting == 0 && characterType == CardValueHUDType.Enemy)
            {
                Hide();
            }
            else
            {
                textMesh.enabled = true;
                this.enabled = true;
            }
        }
        private void Hide()
        {
            textMesh.enabled = false;
            this.enabled = false;
            cardOutline.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (character.CharacterKnockback.IsDead)
            {
                Hide();
                return;
            }

            if(cardBreakController.GetActiveCharacter() == character)
            {
                textMesh.enabled = true;
                List<Card> cards = cardBreakController.GetActiveCards();
                int sum = 0;
                for (int i = 0; i < cards.Count; i++)
                {
                    sum += cards[i].GetCardValue();
                }
                textMesh.text = sum.ToString();
                textMesh.color = colorPlay;

                /*textMeshFeedback.text = textMesh.text;
                textMeshFeedback.gameObject.SetActive(true);*/
            }
            else if (character.DeckController.Deck[character.DeckController.CurrentIndex].GetCardValue() >= 0)
            {
                textMesh.enabled = true;
                textMesh.text = character.DeckController.Deck[character.DeckController.CurrentIndex].GetCardValue().ToString();
                textMesh.color = colorHand;

                //textMeshFeedback.gameObject.SetActive(false);
                cardOutline.gameObject.SetActive(true);
                /*cardOutline.color = character.CharacterAction.CardTypes.GetColorType(character.DeckController.GetCurrentCard().GetCardType());
                cardSprite.sprite = character.DeckController.GetCurrentCard().GetCardIcon();*/
            }
            else
            {
                textMesh.enabled = false;
                cardOutline.gameObject.SetActive(false);
            }
            textMesh.gameObject.transform.localScale = new Vector3(character.CharacterMovement.Direction, 1, 1);
        }
    }
}
