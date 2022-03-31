using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Stats;

namespace VoiceActing
{
    // En fonction du parametre du menu option on joue soit la version courte, soit la version longue
    public class CharAnimation_EquipmentAction : CharacterAnimationEvent
    {
        [SerializeField]
        AttackManager actionShort = null;
        [SerializeField]
        AttackManager actionLong = null;

        public override void Execute(CharacterBase character)
        {
            // si y'a un problème de perf, go stocker ça dans équipement
            int option = PlayerPrefs.GetInt("EquipmentTime", 1);


            if (character.tag == "Player" && option <= 1)
            {
                if (actionShort.AttackAnimation != null)
                {
                    character.Animator.ResetTrigger("Idle");
                    character.Animator.Play(actionShort.AttackAnimation.name, 0, 0f);
                }
                AttackManager currentAction = Instantiate(actionShort, this.transform.position, Quaternion.identity);
                currentAction.CreateAttack(character.CharacterAction.CurrentAttackCard, character);
            }
            else
            {
                if (actionLong.AttackAnimation != null)
                {
                    character.Animator.ResetTrigger("Idle");
                    character.Animator.Play(actionLong.AttackAnimation.name, 0, 0f);
                }
                AttackManager currentAction = Instantiate(actionLong, this.transform.position, Quaternion.identity);
                currentAction.CreateAttack(character.CharacterAction.CurrentAttackCard, character);
            }
        }


    }
}
