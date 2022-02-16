/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class GameRunManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GameRunData runData = null;

        [Title("Init Run")]
        [SerializeField]
        InputController inputController = null;
        [SerializeField]
        ExplorationManager explorationManager = null;
        [SerializeField]
        HealthBarDrawer playerHealthBar = null;


        [Title("Placeholder")]
        [SerializeField]
        CharacterBase player = null;

        [Title("Debug")]
        [SerializeField]
        BattleManager debugBattle = null;
        [SerializeField]
        Transform debugParent = null;

        public bool instantExploration = false;
        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        private void Awake()
        {
            // On créer les data de la run
            runData.CreateRunData();
            inputController.SetControllable(player);

            // On setup le perso et son deck équipement
            player.SetCharacter(runData.PlayerCharacterData);
            player.CharacterEquipment.SetEquipmentDeck(runData.PlayerEquipmentDeck);

            explorationManager.Initialize(player, inputController);

            // On setup le HUD du perso
            playerHealthBar.DrawCharacter(runData.PlayerCharacterData, player.CharacterStat);
            player.CharacterStat.OnHPChanged += playerHealthBar.DrawHealth;
            player.CharacterKnockback.OnRVChanged += playerHealthBar.DrawRevengeValue;


            if(debugBattle.isActiveAndEnabled)
            {
                debugBattle.InitializeBattle(player, debugParent.GetComponentsInChildren<AIController>());
            }
            else
            {
#if UNITY_EDITOR
                if (instantExploration)
                {
                    explorationManager.AddCardLayout(); // Appel la fonction pour ajouter de manière random des cartes support au layout du level 1
                    explorationManager.CreateExplorationMenu();
                    inputController.SetControllable(explorationManager);
                    return;
                }
#endif
                Initialize();
            }



        }

        private void Initialize()
        {
            explorationManager.AddCardLayout(); // Appel la fonction pour ajouter de manière random des cartes support au layout du level 1
            explorationManager.AutoCreateRoom(runData.FloorLayout.FirstRoom, 1f);
        }


        private void OnDestroy()
        {
            player.CharacterStat.OnHPChanged -= playerHealthBar.DrawHealth;
            player.CharacterKnockback.OnRVChanged -= playerHealthBar.DrawRevengeValue;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace