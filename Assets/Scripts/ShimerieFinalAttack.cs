using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class ShimerieFinalAttack : AttackComponent
    {

        CharacterBase player;
        CharacterBase shimerie;

        [SerializeField]
        ParticleSystem particleBladeBeam;
        [SerializeField]
        ParticleSystem particleBladeCircle;
        [SerializeField]
        Feedbacks.AuraSpriteEffect aura;

        [Title("Blade Attack")]
        [SerializeField]
        AttackController attackBladeBeam;
        [SerializeField]
        AttackController attackBladeKnockdown;

        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            base.StartComponent(character, attack);
            player = character.LockController.TargetLocked;
            shimerie = character;

            shimerie.SetCharacterMotionSpeed(1f);
            player.SetCharacterMotionSpeed(1f);
            player.CharacterAction.EndAction();
            player.CharacterAction.CancelAction();
            player.LockController.TargetLocked = null;
            player.LockController.StopTargeting();
            player.transform.position = BattleUtils.Instance.BattleCenter.position;

            shimerie.CharacterStatusController.RemoveAllStatus();
            player.CharacterStatusController.RemoveAllStatus();
            player.CharacterEquipment.UnequipAll();

            shimerie.CanPlay(false);
            shimerie.CharacterAction.CancelSleight();
            shimerie.DeckController.SetIndex(0);
            player.DeckController.SetIndex(1);
            player.CanPlay(false);

            StartCoroutine(FinalAttackCoroutine());
        }

        private IEnumerator FinalAttackCoroutine()
        {
            yield return null;

            yield return new WaitForSeconds(7f);

            particleBladeBeam.transform.SetParent(player.ParticlePoint);


            AudioManager.Instance.StopMusic(15f);
            BattleUtils.Instance.BattleParticle.Stop();
            float timeInBetween = 1;
            for (int i = 0; i < 99; i++)
            {
                particleBladeBeam.Play();
                yield return new WaitForSeconds(0.05f);
                player.CharacterKnockback.Hit(attackController);

                // Augmente le reload à chaque hit
                player.DeckController.SetReloadMax((i+4)/2);
                player.DeckController.AddReload(0);

                if (i == 0)
                    timeInBetween = 1;
                else
                    timeInBetween = 1f / i;

                timeInBetween = Mathf.Max(timeInBetween, 0.01f);
                yield return new WaitForSeconds(timeInBetween);
            }
            particleBladeCircle.Stop();
            player.CharacterKnockback.Hit(attackBladeKnockdown);
            player.OnStateChanged += HardKnockdown;

            yield return new WaitForSeconds(7f);
            aura.AuraFeedback(12, 1.2f, Color.white, true);
        }

        private void HardKnockdown(CharacterState oldState, CharacterState newState)
        {
            if(newState is CharacterStateDown)
            {
                player.OnStateChanged -= HardKnockdown;
                StartCoroutine(HardKnockdownCoroutine());
            }
        }

        private IEnumerator HardKnockdownCoroutine()
        {
            yield return new WaitForSeconds(0.4f);
            player.SetCharacterMotionSpeed(0);
        }
 
    }
}
