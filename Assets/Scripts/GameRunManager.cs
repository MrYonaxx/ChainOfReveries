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
        GameRunData runData;

        [Title("Init Run")]
        [SerializeField]
        InputController inputController;
        [SerializeField]
        ExplorationManager explorationManager;
        [SerializeField]
        HealthBarDrawer playerHealthBar;


        [Title("Placeholder")]
        [SerializeField]
        CharacterBase player;

        [Title("Debug")]
        [SerializeField]
        BattleManager debugBattle;
        [SerializeField]
        Transform debugParent;
        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        private void Start()
        {
            runData.CreateRunData();
            inputController.SetControllable(player);
            player.SetCharacter(runData.PlayerCharacterData);

            explorationManager.Initialize(player, inputController);

            playerHealthBar.DrawCharacter(runData.PlayerCharacterData, player.CharacterStat);
            player.CharacterStat.OnHPChanged += playerHealthBar.DrawHealth;
            player.CharacterKnockback.OnRVChanged += playerHealthBar.DrawRevengeValue;


            if(debugBattle.isActiveAndEnabled)
            {
                debugBattle.InitializeBattle(player, debugParent.GetComponentsInChildren<AIController>());
            }
            else
                SetExplorationManager();
        }



        public void SetExplorationManager()
        {
            explorationManager.CreateExplorationMenu();
            inputController.SetControllable(explorationManager);
        }

        private void OnDestroy()
        {
            player.CharacterStat.OnHPChanged -= playerHealthBar.DrawHealth;
            player.CharacterKnockback.OnRVChanged -= playerHealthBar.DrawRevengeValue;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace