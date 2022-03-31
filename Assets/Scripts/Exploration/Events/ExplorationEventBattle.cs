using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class ExplorationEventBattle : ExplorationEvent
    {
        [SerializeField]
        List<SpriteRenderer> backgroundSprites = null;

        [Title("Battle")] // Si soucis de perf injecter battleManager et battleReward dans CreateEvent et mettre un battleManager dans la scène direct
        [SerializeField]
        BattleManager battleManager = null;
        [SerializeField]
        BattleReward battleReward = null;


        [Title("Data")]
        [SerializeField]
        GameRunData runData = null;

        [Space]
        [SerializeField]
        EncounterDatabase encounterDatabase = null;

        EncounterData encounter = null;
        public EncounterData Encounter
        {
            get { return encounter; }
        }


        public override void CreateEvent(ExplorationManager manager)
        {
            explorationManager = manager;

            if (runData.KillCount <= 0) // C'est vraiment la marouflerie, si on a tué personne, c'est notre premier combat et donc on reset
                encounterDatabase.HardReset();

            // Détermine la rencontre
            encounter = Instantiate(encounterDatabase.GachaEncounter(runData.Floor), this.transform);
            encounter.gameObject.SetActive(false);

            // Détermine le background
            Sprite background = encounter.GetBackground();
            for (int i = 0; i < backgroundSprites.Count; i++)
            {
                backgroundSprites[i].sprite = background;
            }


            battleManager.OnEventBattleEnd += InitializeBattleReward; 
            battleReward.OnEventEnd += EndEvent;
        }

        public override void StartEvent()
        {
            ShowBackground(true);
            DestroyPreviousRoom();
            explorationManager.InputController.SetControllable(explorationManager.Player);
            battleManager.InitializeBattle(explorationManager.Player, encounter.Encounter);
            encounter.gameObject.SetActive(true);

            // Initialize battle modifiers
            for (int i = 0; i < runData.BattleModifiers.Count; i++)
            {
                // Battle Modifier pour le player
                if(runData.BattleModifiers[i].battleModifierTargets == BattleModifierTargets.Player ||
                   runData.BattleModifiers[i].battleModifierTargets == BattleModifierTargets.Both)
                {
                    explorationManager.Player.CharacterStatusController.ApplyStatus(runData.BattleModifiers[i].statusEffect, 100);
                }

                // Battle Modifier pour les ennemis
                if (runData.BattleModifiers[i].battleModifierTargets == BattleModifierTargets.Enemies ||
                    runData.BattleModifiers[i].battleModifierTargets == BattleModifierTargets.Both)
                {
                    for (int j = 0; j < encounter.Encounter.Length; j++)
                    {
                        encounter.Encounter[j].Character.CharacterStatusController.ApplyStatus(runData.BattleModifiers[i].statusEffect, 100);
                    }
                }
            }
        }



        public void InitializeBattleReward()
        {
            explorationManager.InputController.SetControllable(battleReward, true);
            battleReward.InitializeBattleReward(explorationManager.Player);
        }


        private void OnDestroy()
        {
            battleManager.OnEventBattleEnd -= InitializeBattleReward;
            battleReward.OnEventEnd -= EndEvent;
        }


        // :shrug:
        public void PlayLevelBGM()
        {
            AudioManager.Instance.PlayMusic(runData.FloorLayout.Bgm);
        }


        // :shrug:
        public void ChangeLevelBackground()
        {
            explorationManager.ChangeLevelBackground();
        }

    }
}
