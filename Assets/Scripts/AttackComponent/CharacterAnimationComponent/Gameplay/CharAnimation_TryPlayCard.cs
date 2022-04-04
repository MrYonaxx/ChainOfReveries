using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    // Utilisé par l'attaque des épée de Shimerie
    public class CharAnimation_TryPlayCard : CharacterAnimationEvent
    {
        [SerializeField]
        AttackManager attack = null;
        [SerializeField]
        CardBreakController cardBreakController = null;

        [SerializeField]
        CardData cardToPlay = null;
        [SerializeField]
        int cardValue = 0;

        [SerializeField]
        int basePercentToPlay = 100;
        [SerializeField]
        int percentIncrement = 10;

        [SerializeField]
        [SuffixLabel("en frames")]
        float timeIntervalPlay = 10;

        CharacterBase user;
        List<Card> cards = new List<Card>();
        float t = 0f;

        public override bool ShowSecondFrame()
        {
            return true;
        }

        public override void Execute(CharacterBase character)
        {
            cards = new List<Card>();
            cards.Add(new Card(cardToPlay, cardValue));
            user = character;
            timeIntervalPlay /= 60f;
            TryPlayCard();
        }

        public override void UpdateComponent(CharacterBase character, int frame)
        {
            base.UpdateComponent(character, frame);
            t -= Time.deltaTime;
            if (t <= 0)
                TryPlayCard();
        }

        private void TryPlayCard()
        {
            int r = Random.Range(0, 100);
            if(r <= basePercentToPlay && user.State.ID == CharacterStateID.Idle)
            {
                if (cardBreakController.GetActiveCharacter() != null) // On check pour pas cancel une action déjà en cours
                {
                    if (cardBreakController.GetActiveCharacter().tag != user.tag) // On check pour pas cancel une action déjà en cours
                    {
                        if (cardBreakController.PlayCard(user, cards))
                        {
                            // La carte est jouée donc on détruit l'action en cours
                            attack.CancelAction();
                            user.CharacterAction.Action(cards[0]);
                            return;
                        }
                    }
                }
                else
                {
                    if (cardBreakController.PlayCard(user, cards))
                    {
                        // La carte est jouée donc on détruit l'action en cours
                        attack.CancelAction();
                        user.CharacterAction.Action(cards[0]);
                        return;
                    }
                }
            }


            // Sinon
            basePercentToPlay += percentIncrement;
            t = timeIntervalPlay;
        }


    }
}
