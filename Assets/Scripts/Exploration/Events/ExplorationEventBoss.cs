using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

using TMPro;
using UnityEngine.UI;

namespace VoiceActing
{
    public class ExplorationEventBoss : ExplorationEvent
    {
        [SerializeField]
        List<SpriteRenderer> backgroundSprites = null;

        [Title("Data")]
        [SerializeField]
        GameRunData runData = null;
        [SerializeField]
        EncounterDatabaseBoss bossesData = null;

        [Title("Battle")] // Si soucis de perf injecter battleManager et battleReward dans CreateEvent
        [SerializeField]
        BattleManager battleManager = null;
        [SerializeField]
        BattleReward battleReward = null;

        [Title("Intro")]
        [SerializeField]
        CharacterState stateCinematic = null;


        EncounterData bossEncounter;
        AIController[] bosses = null;


        public override void CreateEvent(ExplorationManager manager)
        {
            explorationManager = manager;

            battleManager.OnEventBattleEnd += EndBossBattle;
            battleReward.OnEventEnd += EndEvent;

            AudioManager.Instance?.StopMusic(6f);

            // Instancie le boss
            bossEncounter = Instantiate(bossesData.SelectBoss(runData.Floor-1), this.transform);
            bosses = bossEncounter.Encounter;

            // Détermine le background
            Sprite background = bossEncounter.GetBackground();
            for (int i = 0; i < backgroundSprites.Count; i++)
            {
                backgroundSprites[i].sprite = background;
            }
        }

        public override void StartEvent()
        {
            StartCoroutine(BossIntroCoroutine());
        }

        private IEnumerator BossIntroCoroutine()
        {
            // Set le joueur en ciné
            explorationManager.Player.SetState(stateCinematic);
            explorationManager.InputController.SetControllable(explorationManager.Player);
            ShowBackground(true);
            DestroyPreviousRoom();

            // Intro personnalisé du boss
            yield return bossEncounter.IntroCinematic(explorationManager.Player);


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
