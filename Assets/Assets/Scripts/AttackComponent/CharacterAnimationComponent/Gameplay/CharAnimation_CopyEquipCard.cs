using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_CopyEquipCard : CharacterAnimationEvent
    {

        public override void Execute(CharacterBase character)
        {
            CharacterBase target = character.LockController.TargetLocked;
            if (target != null)
            {
                List<StatusEffectData> status = new List<StatusEffectData>();
                for (int i = 0; i < target.CharacterStatusController.CharacterStatusEffect.Count; i++)
                {
                    if (target.CharacterStatusController.CharacterStatusEffect[i].StatusEffect.StatusShowLabel) // indique que c'est une carte equipement
                    {
                        status.Add(target.CharacterStatusController.CharacterStatusEffect[i].StatusEffect);
                    }
                }

                if (status.Count != 0)
                    character.CharacterStatusController.ApplyStatus(status[Random.Range(0, status.Count)], 100);
            }
        }

    }
}
