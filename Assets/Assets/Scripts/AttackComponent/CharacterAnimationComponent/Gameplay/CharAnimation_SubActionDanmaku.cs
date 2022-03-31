using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_SubActionDanmaku : CharacterAnimationEvent
    {
        [InfoBox("The sub action is card breakable", "InstanceShowFunction")]
        [InfoBox("The sub action is NOT card breakable", "InstanceShowFunction2")]
        [SerializeField]
        AttackManager action = null;

        [SerializeField]
        Transform spawnPoint = null;
        [SerializeField]
        int number = 5;
        [SerializeField]
        Vector2 rotationInterval = new Vector2(80, -80);

        AttackManager currentAction = null;

        int projectileCount = 0;
        int deltaFrame = 0;
        int nextFrame = 0;

        public bool InstanceShowFunction()
        {
            return (action.SubActionCardBreakable);
        }

        public bool InstanceShowFunction2()
        {
            return !(action.SubActionCardBreakable);
        }

        public override bool ShowSecondFrame()
        {
            return true;
        }



        public override void Execute(CharacterBase character)
        {
            // à virer 
            if (action.AttackAnimation != null)
            {
                character.Animator.ResetTrigger("Idle");
                character.Animator.Play(action.AttackAnimation.name, 0, 0f);
            }

            projectileCount = 0;
            nextFrame = Frame;
            deltaFrame = (FrameEnd - Frame) / number;

            int nb = 1;
            if(deltaFrame == 0) // Cas particulier où on tire tout en même temps
            {
                nb = number;
            }

            for (int i = 0; i < nb; i++)
            {
                nextFrame += deltaFrame;
                SpawnProjectile(character, Mathf.Lerp(rotationInterval.x, rotationInterval.y, (float)projectileCount / number));
                projectileCount += 1;
            }
        }

        public override void UpdateComponent(CharacterBase character, int frame)
        {
            if (frame < FrameEnd)
            {
                while(frame >= nextFrame && projectileCount < number)
                {
                    nextFrame += deltaFrame;
                    SpawnProjectile(character, Mathf.Lerp(rotationInterval.x, rotationInterval.y, (float)projectileCount / number));
                    projectileCount += 1;
                }
            }
        }

        private void SpawnProjectile(CharacterBase character, float rotationZ)
        {
            currentAction = Instantiate(action, spawnPoint.position, Quaternion.identity);
            currentAction.CreateAttack(character.CharacterAction.CurrentAttackCard, character);
            currentAction.transform.localEulerAngles = new Vector3(0, 0, rotationZ);
        }
    }
}
