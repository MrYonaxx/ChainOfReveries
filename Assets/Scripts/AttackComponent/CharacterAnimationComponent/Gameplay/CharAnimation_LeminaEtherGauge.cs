using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_LeminaEtherGauge : CharacterAnimationEvent
    {
        [SerializeField]
        int amount = 1;
        [SerializeField]
        int[] time;

        [SerializeField]
        bool actionOnFinish = false;
        [SerializeField]
        AttackManager attackManagerOnFinish = null;

        int lastFrame = 0;
        float interval = 0;
        float nextTick = 0;
        StatusEffectLemina etherGauge = null;

        public override void Execute(CharacterBase character)
        {

            GetEtherGauge(character);

            if (etherGauge == null)
                return;

            Card currentCard = character.CharacterAction.CurrentAttackCard;
            lastFrame = Frame;
            lastFrame += time[Mathf.Min(currentCard.GetCardValue(), 9)];

            if (Frame == lastFrame)
            {
                etherGauge.AddEtherGauge(amount);
            }
            else
            {
                interval = (lastFrame - Frame) / (float)amount;
                nextTick = Frame + interval;
            }

        }


        // Comme d'hab c'est pas très performant,faut que je fasse gaffe
        private void GetEtherGauge(CharacterBase character)
        {
            for (int j = 0; j < character.CharacterStatusController.CharacterStatusEffect.Count; j++)
            {
                for (int i = 0; i < character.CharacterStatusController.CharacterStatusEffect[j].StatusController.Count; i++)
                {
                    if (character.CharacterStatusController.CharacterStatusEffect[j].StatusController[i] is StatusEffectLemina)
                    {
                        etherGauge = (StatusEffectLemina)character.CharacterStatusController.CharacterStatusEffect[j].StatusController[i];
                        return;
                    }
                }
            }
        }

        public override void UpdateComponent(CharacterBase character, int frame)
        {
            if (frame <= lastFrame)
            {
                if (frame >= nextTick)
                {
                    etherGauge.AddEtherGauge(1);
                    nextTick += interval;
                }
            }
            else if (actionOnFinish)
                character.CharacterAction.Action(attackManagerOnFinish);
        }

        /*public override bool ShowSecondFrame()
        {
            return true;
        }*/

    }
}
