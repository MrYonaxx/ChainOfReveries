using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class ExplorationEventBoss : ExplorationEvent
    {
        [SerializeField]
        List<SpriteRenderer> backgroundSprites = null;

        [Title("Data")]
        [SerializeField]
        GameRunData runData = null;

        [Title("Battle")] // Si soucis de perf injecter battleManager et battleReward dans CreateEvent
        [SerializeField]
        BattleManager battleManager = null;
        [SerializeField]
        BattleReward battleReward = null;
        [SerializeField]
        AIController[] bosses = null;

        [Title("Intro")]
        [SerializeField]
        CharacterState stateCinematic = null;
        [SerializeField]
        Transform playerPos = null;
        [SerializeField]
        Animator enemyCinematic = null;
        [SerializeField]
        Canvas canvasIntro = null;

        [Title("Sound")]
        [SerializeField]
        AudioClip battleTheme = null;


        public override void CreateEvent(ExplorationManager manager)
        {
            explorationManager = manager;
            battleManager.OnEventBattleEnd += EndBossBattle;
            battleReward.OnEventEnd += EndEvent;
            AudioManager.Instance?.StopMusic(6f);
        }

        public override void StartEvent()
        {
            StartCoroutine(BossIntroCoroutine());
        }

        private IEnumerator BossIntroCoroutine()
        {
            explorationManager.Player.SetState(stateCinematic);
            explorationManager.InputController.SetControllable(explorationManager.Player);
            explorationManager.Player.CanPlay(false);
            explorationManager.Player.Inputs.InputLeftStickX.InputValue = 0;
            explorationManager.Player.Inputs.InputLeftStickY.InputValue = 0;

            float t = 0;
            float time = 1;
            while (t < time)
            {
                t += Time.deltaTime;
                if(explorationManager.Player.CharacterMovement.MoveToPoint(playerPos.position, 2f))
                {
                    explorationManager.Player.CharacterMovement.InMovement = false;
                    explorationManager.Player.CharacterMovement.Move(0, 0);
                }
                yield return null;
            }
            explorationManager.Player.CharacterMovement.InMovement = false;
            explorationManager.Player.CharacterMovement.Move(0, 0);

            yield return new WaitForSeconds(1f);

            BattleFeedbackManager.Instance.CameraController.AddTarget(enemyCinematic.transform, 0);
            AudioManager.Instance?.PlayMusic(battleTheme);
            yield return new WaitForSeconds(3f);

            ShowBackground(true);
            enemyCinematic.gameObject.SetActive(true);
            yield return new WaitForSeconds(9f);

            canvasIntro.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            // Idéalement le combat doit commencer seconde 16

            // START BOSS BATTLE
            for (int i = 0; i < bosses.Length; i++)
            {
                bosses[i].gameObject.SetActive(true);
                bosses[i].Character.CharacterMovement.SetDirection(-1);
            }

            explorationManager.InputController.SetControllable(explorationManager.Player);
            battleManager.InitializeBattle(explorationManager.Player, bosses);
            explorationManager.Player.CanPlay(true);

            for (int i = 0; i < bosses.Length; i++)
            {
                bosses[i].Character.DeckController.ReloadDeck();
            }

            BattleFeedbackManager.Instance.CameraController.RemoveTarget(enemyCinematic.transform);
            enemyCinematic.gameObject.SetActive(false);



            // Initialize battle modifiers
            for (int i = 0; i < runData.BattleModifiers.Count; i++)
            {
                // Battle Modifier pour le player
                if (runData.BattleModifiers[i].battleModifierTargets == BattleModifierTargets.Player ||
                   runData.BattleModifiers[i].battleModifierTargets == BattleModifierTargets.Both)
                {
                    explorationManager.Player.CharacterStatusController.ApplyStatus(runData.BattleModifiers[i].statusEffect, 100);
                }

                // Battle Modifier pour les boss
                if (runData.BattleModifiers[i].battleModifierTargets == BattleModifierTargets.Enemies ||
                    runData.BattleModifiers[i].battleModifierTargets == BattleModifierTargets.Both)
                {
                    for (int j = 0; j < bosses.Length; j++)
                    {
                        bosses[j].Character.CharacterStatusController.ApplyStatus(runData.BattleModifiers[i].statusEffect, 100);
                    }
                }
            }
        }




        public void EndBossBattle()
        {
            explorationManager.InputController.SetControllable(battleReward);
            battleReward.InitializeBattleReward(explorationManager.Player);
        }


        private void OnDestroy()
        {
            battleManager.OnEventBattleEnd -= EndBossBattle;
            battleReward.OnEventEnd -= EndEvent;
        }


    }
}
