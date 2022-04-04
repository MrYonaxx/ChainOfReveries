using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_CreateCard : CharacterAnimationEvent
    {
        [SerializeField]
        GetCardObject cardObject;
        [SerializeField]
        Card card;

        GetCardObject currentCardObject = null;

        public override void Execute(CharacterBase character)
        {
            currentCardObject = Instantiate(cardObject, this.transform.position, Quaternion.identity);

            Vector3 point = BattleUtils.Instance.BattleCenter.position;
            point.x += Random.Range(-4, 4);
            point.y += Random.Range(0, 1);
            currentCardObject.GetNewcard = true;
            currentCardObject.Initialize(character.LockController.TargetLocked, new Card(card.CardData, card.baseCardValue), Color.green, point.x, point.y);
        }

        private void OnDestroy()
        {
            if (currentCardObject != null)
                Destroy(currentCardObject.gameObject);
        }

    }
}
