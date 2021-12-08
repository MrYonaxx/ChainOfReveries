using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_SubAction : CharacterAnimationEvent
    {
        [InfoBox("The sub action is card breakable", "InstanceShowFunction")]
        [InfoBox("The sub action is NOT card breakable", "InstanceShowFunction2")]
        [SerializeField]
        AttackManager action = null;

        [Space]
        [SerializeField]
        [HorizontalGroup("Bool")]
        bool setTransform = false;
        [Space]
        [SerializeField]
        [HorizontalGroup("Bool")]
        bool setRotation = false;

        [ShowIf("setTransform")]
        [SerializeField]
        Transform spawnPoint = null;

        [ShowIf("setRotation")]
        [SerializeField]
        float rotationZ = 0;


        public bool InstanceShowFunction()
        {
            return (action.SubActionCardBreakable);
        }

        public bool InstanceShowFunction2()
        {
            return !(action.SubActionCardBreakable);
        }

        AttackManager currentAction = null;

        public override void Execute(CharacterBase character)
        {
            // à virer 
            if (action.AttackAnimation != null)
            {
                character.Animator.ResetTrigger("Idle");
                character.Animator.Play(action.AttackAnimation.name, 0, 0f);
            }


            currentAction = Instantiate(action, this.transform.position, Quaternion.identity);
            currentAction.CreateAttack(character.CharacterAction.CurrentAttackCard, character);

            if (setRotation)
            {
                currentAction.transform.localEulerAngles = new Vector3(0, 0, rotationZ);
            }
        }


    }
}
