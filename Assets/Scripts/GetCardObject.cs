using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    public class GetCardObject : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer cardSprite = null;
        [SerializeField]
        SpriteRenderer cardOutline = null;

        [SerializeField]
        float timeAirborne = 3;
        

        Card card = null;
        CharacterBase character = null;

        Vector3 startPos;
        Vector3 endPos;
        float t = 0f;
        public bool GetNewcard = false; // Si faux on récupère une deck qui provient du deck, si true c'est une carte inédite

        public void Initialize(CharacterBase owner, Card c, Color color, float posX, float posY)
        {
            t = 0f;
            character = owner;
            card = c;

            startPos = transform.position;
            endPos = new Vector3(posX, posY);

            DrawCard(c, color);

            if (!GetNewcard)
                character.OnBattleEnd += RetrieveCard;
        }


        public void DrawCard(Card c, Color color)
        {
            cardSprite.sprite = c.GetCardIcon();
            cardOutline.color = color;
        }


        public void Update()
        {
            if (t < timeAirborne)
            {
                t += Time.deltaTime;
                Vector3 pos = Vector3.Lerp(startPos, endPos, t / timeAirborne);
                pos.z = pos.y;
                this.transform.position = pos;
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject == character.gameObject && t >= timeAirborne)
            {
                if (GetNewcard)
                    AddCard();
                else
                    RetrieveCard();
            }
        }


        private void RetrieveCard() 
        {
            character.OnBattleEnd -= RetrieveCard;
            character.DeckController.UnbanishCard(card);
            character.DeckController.ReplaceCard(card);
            character.DeckController.RefreshDeck();
            Destroy(this.gameObject);
        }

        private void AddCard()
        {
            character.DeckController.Deck.Insert(1, card);
            character.DeckController.RefreshDeck(); 
            Destroy(this.gameObject);
        }

    }
}
