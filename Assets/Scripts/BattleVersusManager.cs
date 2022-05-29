/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class BattleVersusManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Title("Game Parameter")]
        [SerializeField]
        GameRunData dataP1 = null;
        [SerializeField]
        GameRunData dataP2 = null;

        [Title("UI")]
        [SerializeField]
        GameObject canvasBattle = null;

        [Title("UI - Player")]
        [SerializeField]
        HealthBarDrawer playerHealthBar = null;
        [SerializeField]
        SleightDrawer playerSleightDrawer = null;
        [SerializeField]
        EquipmentDrawer playerEquipmentDrawer = null;
        [SerializeField]
        StatusDrawer playerStatusDrawer = null;
        [SerializeField]
        DeckBattleDrawer[] playerDeckDrawers = null;

        [Title("UI - Enemy")]
        [SerializeField]
        HealthBarDrawer enemyHealthBar = null;
        [SerializeField]
        SleightDrawer enemySleightDrawer = null;
        [SerializeField]
        EquipmentDrawer enemyEquipmentDrawer = null;
        [SerializeField]
        StatusDrawer enemyStatusDrawer = null;
        [SerializeField]
        DeckBattleDrawer[] enemyDeckDrawers = null;


        DeckBattleDrawer playerDeckDrawer = null;
        DeckBattleDrawer enemyDeckDrawer = null;


        CharacterBase player = null;
        CharacterBase enemy = null;

        List<CharacterBase> enemiesController;

        public delegate void ActionCharacter(CharacterBase character);
        public event ActionCharacter OnEventBattleEnd;


        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        private void InitializeDeckDrawer()
        {
            playerDeckDrawer = playerDeckDrawers[GameSettings.DeckLayout];
            enemyDeckDrawer = enemyDeckDrawers[GameSettings.DeckLayout];

            playerDeckDrawer.gameObject.SetActive(true);
            enemyDeckDrawer.gameObject.SetActive(true);
        }

        public void InitializeBattle(CharacterBase p1, CharacterBase p2)
        {
            InitializeDeckDrawer();

            // Initialize Player 1 ==============================================================================
            player = p1;
            player.DeckController.SetDeck(dataP1.PlayerDeck);
            player.CharacterEquipment.SetWeaponDeck(dataP1.PlayerEquipmentDeck);
            player.SleightController.ResetSleightCard();

            // Setup event HP
            playerHealthBar.DrawCharacter(p1.CharacterData, p1.CharacterStat);
            player.CharacterStat.OnHPChanged += playerHealthBar.DrawHealth;
            player.CharacterKnockback.OnRVChanged += playerHealthBar.DrawRevengeValue;

            // Setup event Deck
            player.DeckController.OnDeckChanged += playerDeckDrawer.DrawHand;
            player.DeckController.OnCardMoved += playerDeckDrawer.MoveHand;
            player.DeckController.OnCardPlayed += playerDeckDrawer.PlayCard;
            player.DeckController.OnReload += playerDeckDrawer.HideCards;
            player.DeckController.OnReloadChanged += playerDeckDrawer.DrawReload;

            // Setup event Sleight
            player.SleightController.OnSleightUpdate += playerSleightDrawer.DrawSleight;

            // Setup event equipment
            player.CharacterEquipment.OnEquipWeapon += playerEquipmentDrawer.DrawEquipments;
            player.CharacterStatusController.OnStatusChanged += playerStatusDrawer.DrawStatus;

            player.LockController.StartTargeting();


            // Initialize Player 2 ================================================================================
            enemy = p2;
            enemy.DeckController.SetDeck(dataP2.PlayerDeck);
            enemy.CharacterEquipment.SetWeaponDeck(dataP2.PlayerEquipmentDeck);
            enemy.SleightController.ResetSleightCard();

            // Setup event HP
            enemyHealthBar.DrawCharacter(p2.CharacterData, p2.CharacterStat);
            enemy.CharacterStat.OnHPChanged += enemyHealthBar.DrawHealth;
            enemy.CharacterKnockback.OnRVChanged += enemyHealthBar.DrawRevengeValue;

            // Setup event Deck
            enemy.DeckController.OnDeckChanged += enemyDeckDrawer.DrawHand;
            enemy.DeckController.OnCardMoved += enemyDeckDrawer.MoveHand;
            enemy.DeckController.OnCardPlayed += enemyDeckDrawer.PlayCard;
            enemy.DeckController.OnReload += enemyDeckDrawer.HideCards;
            enemy.DeckController.OnReloadChanged += enemyDeckDrawer.DrawReload;

            // Setup event Sleight
            enemy.SleightController.OnSleightUpdate += enemySleightDrawer.DrawSleight;

            // Setup event equipment
            enemy.CharacterEquipment.OnEquipWeapon += enemyEquipmentDrawer.DrawEquipments;
            enemy.CharacterStatusController.OnStatusChanged += enemyStatusDrawer.DrawStatus;

            enemy.LockController.StartTargeting();

            enemy.CharacterMovement.SetDirection(-1);
            enemy.tag = "Enemy";


            // Plus le deck est petit plus on joue en premier
            player.ResetToIdle();
            player.DeckController.ReloadDeck();
            enemy.ResetToIdle();
            enemy.DeckController.ReloadDeck();

            player.LockController.StopTargeting();
            enemy.LockController.StopTargeting();
            player.LockController.TargetLocked = enemy;
            enemy.LockController.TargetLocked = player;


            player.CharacterKnockback.OnDeath += CharacterDead;
            enemy.CharacterKnockback.OnDeath += CharacterDead;

            canvasBattle.SetActive(true);


            BattleUtils.Instance.Characters.Add(player);
            BattleUtils.Instance.Characters.Add(enemy);
        }

        // Comme initialize mais sans les subscribe d'event
        public void ReinitializeBattle()
        {
            player.DeckController.SetDeck(dataP1.PlayerDeck);
            player.SleightController.ResetSleightCard();

            enemy.DeckController.SetDeck(dataP2.PlayerDeck);
            enemy.SleightController.ResetSleightCard();

            // Plus le deck est petit plus on joue en premier
            player.ResetToIdle();
            player.DeckController.ReloadDeck();
            enemy.ResetToIdle();
            enemy.DeckController.ReloadDeck();

            canvasBattle.SetActive(true);
        }

        public void ReinitializeBattle(List<Card> p1Deck, List<Card> p2Deck, List<CardEquipment> p1Equipment, List<CardEquipment> p2Equipment)
        {
            player.DeckController.SetDeck(p1Deck);
            player.CharacterEquipment.SetWeaponDeck(p1Equipment);
            player.SleightController.ResetSleightCard();

            enemy.DeckController.SetDeck(p2Deck);
            enemy.CharacterEquipment.SetWeaponDeck(p2Equipment);
            enemy.SleightController.ResetSleightCard();

            // Plus le deck est petit plus on joue en premier
            player.ResetToIdle();
            player.DeckController.ReloadDeck();
            enemy.ResetToIdle();
            enemy.DeckController.ReloadDeck();

            canvasBattle.SetActive(true);
        }

        private void UninitializeBattle()
        {
            if (player != null)
            {
                player.CharacterStat.OnHPChanged -= playerHealthBar.DrawHealth;
                player.CharacterKnockback.OnRVChanged -= playerHealthBar.DrawRevengeValue;

                player.DeckController.OnDeckChanged -= playerDeckDrawer.DrawHand;
                player.DeckController.OnCardMoved -= playerDeckDrawer.MoveHand;
                player.DeckController.OnCardPlayed -= playerDeckDrawer.PlayCard;
                player.DeckController.OnReload -= playerDeckDrawer.HideCards;
                player.DeckController.OnReloadChanged -= playerDeckDrawer.DrawReload;

                player.SleightController.OnSleightUpdate -= playerSleightDrawer.DrawSleight;

                player.CharacterEquipment.OnEquipWeapon -= playerEquipmentDrawer.DrawEquipments;
                player.CharacterStatusController.OnStatusChanged -= playerStatusDrawer.DrawStatus;

            }

            if(enemy != null)
            {
                enemy.CharacterStat.OnHPChanged -= enemyHealthBar.DrawHealth;
                enemy.CharacterKnockback.OnRVChanged -= enemyHealthBar.DrawRevengeValue;

                enemy.DeckController.OnDeckChanged -= enemyDeckDrawer.DrawHand;
                enemy.DeckController.OnCardMoved -= enemyDeckDrawer.MoveHand;
                enemy.DeckController.OnCardPlayed -= enemyDeckDrawer.PlayCard;
                enemy.DeckController.OnReload -= enemyDeckDrawer.HideCards;
                enemy.DeckController.OnReloadChanged -= enemyDeckDrawer.DrawReload;

                enemy.SleightController.OnSleightUpdate -= enemySleightDrawer.DrawSleight;

                enemy.CharacterEquipment.OnEquipWeapon -= enemyEquipmentDrawer.DrawEquipments;
                enemy.CharacterStatusController.OnStatusChanged -= enemyStatusDrawer.DrawStatus;
            }

        }

        private void OnDestroy()
        {
            UninitializeBattle();
        }


        public void CharacterDead(CharacterBase character, DamageMessage dmgMsg)
        {
            player.CharacterKnockback.OnDeath -= CharacterDead;
            enemy.CharacterKnockback.OnDeath -= CharacterDead;
            if (character == player)
            {
                // P2 win
            }
            else if (character == enemy)
            {
                // P1 win
            }
            EndBattle(character);
        }


        public void EndBattle(CharacterBase character)
        {
            player.CanPlay(false);
            player.EndBattle(); // Appel l'event de fin de combat
            player.CharacterKnockback.ResetRevengeValue();

            enemy.CanPlay(false);
            enemy.EndBattle(); // Appel l'event de fin de combat
            enemy.CharacterKnockback.ResetRevengeValue();


            BattleUtils.Instance.Characters.Clear();
            StartCoroutine(EndBossCoroutine(character));
        }

        /*private IEnumerator EndBattleCoroutine()
        {
            player.LockController.Targeting = false;
            BattleFeedbackManager.Instance.BloomDeath();
            BattleFeedbackManager.Instance.CameraBigZoom();
            BattleFeedbackManager.Instance.ShakeScreen(0.2f, 25);
            BattleFeedbackManager.Instance.SetBattleMotionSpeed(0f, 0.6f);
            yield return null;
            BattleFeedbackManager.Instance.SetBattleMotionSpeed(0f, 0.6f);
            yield return new WaitForSeconds(0.6f);
            BattleFeedbackManager.Instance.SetBattleMotionSpeed(0.2f, 2f);
            yield return new WaitForSeconds(4);

            canvasBattle.SetActive(false);
            OnEventBattleEnd.Invoke();
            player.CanPlay(true);
            //SetTarget(null);
            player.LockController.Targeting = true;
        }*/

        private IEnumerator EndBossCoroutine(CharacterBase characterDefeated)
        {
            AudioManager.Instance.StopMusicWithScratch(12f);
            BattleFeedbackManager.Instance.RippleScreen(characterDefeated.ParticlePoint.position.x, characterDefeated.ParticlePoint.position.y);
            BattleFeedbackManager.Instance.SetBattleMotionSpeed(0f, 2f);
            BattleFeedbackManager.Instance.BackgroundFlash();
            BattleFeedbackManager.Instance.ShakeScreen(0.2f, 25);
            BattleFeedbackManager.Instance.CameraController.StopZoom();
            yield return null;
            BattleFeedbackManager.Instance.SetBattleMotionSpeed(0f, 1f);

            yield return new WaitForSeconds(1f);

            BattleFeedbackManager.Instance.SetBattleMotionSpeed(0.2f, 3f);
            BattleFeedbackManager.Instance.BackgroundFlash();
            BattleFeedbackManager.Instance.BloomDeathBoss();
            BattleFeedbackManager.Instance.CameraBossZoom();
            BattleFeedbackManager.Instance.RippleScreen(characterDefeated.ParticlePoint.position.x, characterDefeated.ParticlePoint.position.y);

            yield return new WaitForSeconds(4);
            yield return new WaitForSeconds(1);
            canvasBattle.SetActive(false);
            OnEventBattleEnd.Invoke(characterDefeated);
        }



        #endregion

    } 

} // #PROJECTNAME# namespace