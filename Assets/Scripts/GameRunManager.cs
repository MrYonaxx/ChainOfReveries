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
            {
#if UNITY_EDITOR
                if (instantExploration)
                {
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
            explorationManager.AutoCreateRoom(runData.FloorLayout.FirstRoom, 1f);
        }

        /*private void SetExplorationManager()
        {
            explorationManager.CreateExplorationMenu();
            inputController.SetControllable(explorationManager);
        }*/

        private void OnDestroy()
        {
            player.CharacterStat.OnHPChanged -= playerHealthBar.DrawHealth;
            player.CharacterKnockback.OnRVChanged -= playerHealthBar.DrawRevengeValue;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace