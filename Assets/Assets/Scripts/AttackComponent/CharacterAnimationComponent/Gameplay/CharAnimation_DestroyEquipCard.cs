using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_DestroyEquipCard : CharacterAnimationEvent
    {

        public override void Execute(CharacterBase character)
        {
            CharacterBase target = character.LockController.TargetLocked;
            if(target != null)
            {
                for (int i = target.CharacterStatusController.CharacterStatusEffect.Count-1; i >= 0; i--)
                {
                    if (target.CharacterStatusController.CharacterStatusEffect[i].StatusEffect.StatusShowLabel) // indique que c'est une carte equipement
                    {
                        target.CharacterStatusController.RemoveStatus(target.CharacterStatusController.CharacterStatusEffect[i].StatusEffect);
                    }
                }
            }
        }

    }
}
