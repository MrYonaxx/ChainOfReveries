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
        [SerializeField]
        ComboCountDrawer hitHud = null;
        [SerializeField]
        DataCollector dataCollector = null;
        [SerializeField]
        GameOverController gameOverController = null;

        [Title("Database")]
        [SerializeField]
        PlayerData[] playerData = null;
        [SerializeField]
        CharacterBase[] playerDatabase = null;

        [Title("Debug")]
        [SerializeField]
        InfiniteCorridor infiniteCorridor = null;
        [SerializeField]
        BattleManager debugBattle = null;
        [SerializeField]
        Transform debugParent = null;

        CharacterBase player = null;
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
            player = Instantiate(SpawnCharacter(), Vector3.zero, Quaternion.identity);
            BattleFeedbackManager.Instance?.CameraController.AddTarget(player.transform, 0);
            infiniteCorridor.focusCorridor = player.transform;
            inputController.SetControllable(player);

            // On setup le perso et son deck équipement
            player.SetCharacter(runData.PlayerCharacterData);
            player.CharacterEquipment.SetEquipmentDeck(runData.PlayerEquipmentDeck);

            explorationManager.Initialize(player, inputController);

            // On setup le HUD du perso
            playerHealthBar.DrawCharacter(runData.PlayerCharacterData, player.CharacterStat);
            player.CharacterStat.OnHPChanged += playerHealthBar.DrawHealth;
            player.CharacterKnockback.OnRVChanged += playerHealthBar.DrawRevengeValue;
            hitHud.SetCharacter(player);

            gameOverController.InitializeGameOver(player);
            dataCollector.Initialize(player);
                

            if (debugBattle.isActiveAndEnabled)
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
                    inputController.SetControllable(explorationManager, true);
                    return;
                }
#endif
                Initialize();
            }



        }

        // Plus simple de faire une boucle pour setup une run dans l'editeur plutot que d'utiliser l'ID
        CharacterBase SpawnCharacter()
        {
            for (int i = 0; i < playerData.Length; i++)
            {
                runData.CharacterID = i;
                if (playerData[i] == runData.PlayerCharacterData)
                    return playerDatabase[i];
            }
            return null;
        }

        private void Initialize()
        {
            explorationManager.AddCardLayout(); // Appel la fonction pour ajouter de manière random des cartes support au layout du level 1
            explorationManager.AutoCreateRoom(runData.FloorLayout.FirstRoom, 1f);
            explorationManager.QuitMenuStatus(); // porte très mal son nom mais permet de setup le joueur pour qu'il se déplave en mode exploration
        }


        private void OnDestroy()
        {
            player.CharacterStat.OnHPChanged -= playerHealthBar.DrawHealth;
            player.CharacterKnockback.OnRVChanged -= playerHealthBar.DrawRevengeValue;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace