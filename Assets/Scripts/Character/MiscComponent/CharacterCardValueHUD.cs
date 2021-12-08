using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace VoiceActing
{
    public class CharacterCardValueHUD : MonoBehaviour
    {
        [SerializeField]
        CharacterBase character = null;
        [SerializeField]
        CardBreakController cardBreakController = null;
        [SerializeField]
        TextMeshPro textMesh = null;

        [SerializeField]
        Color colorPlay;
        [SerializeField]
        Color colorHand;

        // Update is called once per frame
        void Update()
        {
            if (character.CharacterKnockback.IsDead)
            {
                textMesh.enabled = false;
                this.enabled = false;
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
            }
            else if (character.DeckController.Deck[character.DeckController.CurrentIndex].GetCardValue() >= 0)
            {
                textMesh.enabled = true;
                textMesh.text = character.DeckController.Deck[character.DeckController.CurrentIndex].GetCardValue().ToString();
                textMesh.color = colorHand;
            }
            else
            {
                textMesh.enabled = false;
            }
            textMesh.gameObject.transform.localScale = new Vector3(character.CharacterMovement.Direction, 1, 1);
        }
    }
}
