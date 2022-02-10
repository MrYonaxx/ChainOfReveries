﻿/*****************************************************************
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
    public class BattleManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Title("Game Parameter")]
        [SerializeField]
        GameRunData runData = null;
        [SerializeField]
        bool boss = false;


        [Title("UI")]
        [SerializeField]
        GameObject canvasBattle = null;

        [Title("UI - Player")]
        [SerializeField]
        HealthBarDrawer playerHealthBar = null;
        [SerializeField]
        SleightDrawer playerSleightDrawer = null;
        [SerializeField]
        DeckBattleDrawer playerDeckDrawer = null;
        [SerializeField]
        StatusDrawer playerStatusDrawer = null;

        [Title("UI - Enemies")]
        [SerializeField]
        HealthBarDrawer enemyHealthBar = null;
        [SerializeField]
        SleightDrawer enemySleightDrawer = null;
        [SerializeField]
        DeckBattleDrawer enemyDeckDrawer = null;
        [SerializeField]
        StatusDrawer enemyStatusDrawer = null;


        CharacterBase player = null;
        CharacterBase enemyTarget = null;

        List<CharacterBase> enemiesController;

        [SerializeField]
        CharacterBase zbla;

        public delegate void Action();
        public event Action OnEventBattleEnd;


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

        public void InitializeBattle(CharacterBase character, AIController[] enemies)
        {
            // Initialize Player
            player = character;
            player.DeckController.SetDeck(runData.PlayerDeck);
            player.CharacterEquipment.SetEquipmentDeck(runData.PlayerEquipmentDeck);
            player.SleightController.ResetSleightCard();

            // ON le fait à chaque début de combat, si c'est trop relou, garder une référence des drawer sur la scene
            player.DeckController.OnDeckChanged += playerDeckDrawer.DrawHand;
            player.DeckController.OnCardMoved += playerDeckDrawer.MoveHand;
            player.DeckController.OnCardPlayed += playerDeckDrawer.PlayCard;
            player.DeckController.OnReload += playerDeckDrawer.HideCards;
            player.DeckController.OnReloadChanged += playerDeckDrawer.DrawReload;

            // Note au moi du futur : y'a un peu beaucoup d'event
            player.CharacterEquipment.DeckEquipmentController.OnDeckChanged += playerDeckDrawer.DrawHand;
            player.CharacterEquipment.DeckEquipmentController.OnCardMoved += playerDeckDrawer.MoveHand;

            player.SleightController.OnSleightUpdate += playerSleightDrawer.DrawSleight;

            // Note au moi du futur futur : y'en a vraiment beaucoup
            player.CharacterStatusController.OnStatusChanged += playerStatusDrawer.DrawStatus;

            player.LockController.OnTargetLock += SetTarget;
            player.LockController.StartTargeting();


            // Initialize enemies
            enemiesController = new List<CharacterBase>(enemies.Length);
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].StartBehavior();
                enemiesController.Add(enemies[i].Character);
                enemiesController[i].CharacterKnockback.OnDeath += EnemyDead;
                enemiesController[i].CharacterStatusController.OnStatusChanged += enemyStatusDrawer.DrawStatus;
                enemiesController[i].SetCharacter();

                // à virer après
                enemiesController[i].CharacterEquipment.DeckEquipmentController.OnDeckChanged += enemyDeckDrawer.DrawHand;
                enemiesController[i].CharacterEquipment.DeckEquipmentController.OnCardMoved += enemyDeckDrawer.MoveHand;
            }

            // Si c'est un 1v1 on lock tout le temps
           /* if (enemiesController.Count == 1)
            {
                ForceLock();
            }*/


            if(zbla)
            {
                /*zbla.DeckController.OnDeckChanged += enemyDeckDrawer.DrawHand;
                zbla.DeckController.OnCardMoved += enemyDeckDrawer.MoveHand;
                zbla.DeckController.OnCardPlayed += enemyDeckDrawer.PlayCard;
                zbla.DeckController.OnReload += enemyDeckDrawer.HideCards;
                zbla.DeckController.OnReloadChanged += enemyDeckDrawer.DrawReload;*/

                zbla.CharacterEquipment.DeckEquipmentController.OnDeckChanged += enemyDeckDrawer.DrawHand;
                zbla.CharacterEquipment.DeckEquipmentController.OnCardMoved += enemyDeckDrawer.MoveHand;

                //zbla.SleightController.OnSleightUpdate += enemySleightDrawer.DrawSleight;

                zbla.CharacterStatusController.OnStatusChanged += enemyStatusDrawer.DrawStatus;
            }

            // Repasse en idle, permettant au player de taper
            player.ResetToIdle();
            player.DeckController.ReloadDeck();

            canvasBattle.SetActive(true);
        }

        private void UninitializeBattle()
        {
            if (player != null)
            {
                player.DeckController.OnDeckChanged -= playerDeckDrawer.DrawHand;
                player.DeckController.OnCardMoved -= playerDeckDrawer.MoveHand;
                player.DeckController.OnCardPlayed -= playerDeckDrawer.PlayCard;
                player.DeckController.OnReload -= playerDeckDrawer.HideCards;
                player.DeckController.OnReloadChanged -= playerDeckDrawer.DrawReload;

                player.CharacterEquipment.DeckEquipmentController.OnDeckChanged -= playerDeckDrawer.DrawHand;
                player.CharacterEquipment.DeckEquipmentController.OnCardMoved -= playerDeckDrawer.MoveHand;

                player.SleightController.OnSleightUpdate -= playerSleightDrawer.DrawSleight;
                player.CharacterStatusController.OnStatusChanged -= playerStatusDrawer.DrawStatus;

                SetTarget(null);
                player.LockController.OnTargetLock -= SetTarget;
            }

            for (int i = 0; i < enemiesController.Count; i++)
            {
                enemiesController[i].CharacterKnockback.OnDeath -= EnemyDead;

                // Faudrait le mettre dans event mais grosse flemme là de chercher partout et normalement y'a que les
                // boss qui ont des cartes equipement donc ils sont solo
                enemiesController[i].CharacterStatusController.OnStatusChanged -= enemyStatusDrawer.DrawStatus;
            }

            if (enemyTarget != null)
            {
                enemyTarget.DeckController.OnDeckChanged -= enemyDeckDrawer.DrawHand;
                enemyTarget.DeckController.OnCardMoved -= enemyDeckDrawer.MoveHand;
                enemyTarget.DeckController.OnCardPlayed -= enemyDeckDrawer.PlayCard;
                enemyTarget.DeckController.OnReloadChanged -= enemyDeckDrawer.DrawReload;

                enemyTarget.CharacterStat.OnHPChanged -= enemyHealthBar.DrawHealth;
                enemyTarget.CharacterKnockback.OnRVChanged -= enemyHealthBar.DrawRevengeValue;
                enemyTarget.SleightController.OnSleightUpdate -= enemySleightDrawer.DrawSleight;
            }
        }

        private void OnDestroy()
        {
            UninitializeBattle();
        }

        public void SetTarget(CharacterBase character)
        {
            /*if (enemiesController.Count == 1 && character != null)
            {
                player.LockController.StopTargeting();
            }*/

            if (enemyTarget != null)
            {
                // Désabonne l'ancienne target au drawer
                enemyTarget.DeckController.OnDeckChanged -= enemyDeckDrawer.DrawHand;
                enemyTarget.DeckController.OnCardMoved -= enemyDeckDrawer.MoveHand;
                enemyTarget.DeckController.OnCardPlayed -= enemyDeckDrawer.PlayCard;
                enemyTarget.DeckController.OnReloadChanged -= enemyDeckDrawer.DrawReload;

                enemyTarget.CharacterStat.OnHPChanged -= enemyHealthBar.DrawHealth;
                enemyTarget.CharacterKnockback.OnRVChanged -= enemyHealthBar.DrawRevengeValue;
                enemyTarget.SleightController.OnSleightUpdate -= enemySleightDrawer.DrawSleight;

                BattleFeedbackManager.Instance.CameraController.RemoveTarget(enemyTarget.transform);
            }

            if (character == null)
            {
                enemyDeckDrawer.ShowDeck(false);
                enemyHealthBar.gameObject.SetActive(false);
            }
            else
            {
                // Abonne la nouvelle target à tout les composants d'UI
                enemyDeckDrawer.ShowDeck(true);
                enemyDeckDrawer.DrawHand(character.DeckController.GetCurrentIndex(), character.DeckController.GetDeck());

                enemyHealthBar.gameObject.SetActive(true);
                enemyHealthBar.DrawCharacter(character.CharacterData, character.CharacterStat);

                character.DeckController.OnDeckChanged += enemyDeckDrawer.DrawHand;
                character.DeckController.OnCardMoved += enemyDeckDrawer.MoveHand;
                character.DeckController.OnCardPlayed += enemyDeckDrawer.PlayCard;
                character.DeckController.OnReloadChanged += enemyDeckDrawer.DrawReload;

                character.CharacterStat.OnHPChanged += enemyHealthBar.DrawHealth;
                character.CharacterKnockback.OnRVChanged += enemyHealthBar.DrawRevengeValue;
                character.SleightController.OnSleightUpdate += enemySleightDrawer.DrawSleight;

                BattleFeedbackManager.Instance.CameraController.AddTarget(character.transform, 0);
            }

            enemyTarget = character;

        }

        public void EnemyDead(CharacterBase enemy, DamageMessage dmgMsg)
        {
            enemy.CharacterKnockback.OnDeath -= EnemyDead;
            enemiesController.Remove(enemy);

            if (enemiesController.Count == 0)
            {
                EndBattle();
            }
        }


        public void GameOver()
        {

        }

        public void EndBattle()
        {
            player.CanPlay(false);
            player.EndBattle(); // Appel l'event de fin de combat

            if (boss)
                StartCoroutine(EndBossCoroutine());
            else
                StartCoroutine(EndBattleCoroutine());
        }

        private IEnumerator EndBattleCoroutine()
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
            SetTarget(null);
            player.LockController.Targeting = true;
        }

        private IEnumerator EndBossCoroutine()
        {
            player.LockController.Targeting = false;
            AudioManager.Instance.StopMusicWithScratch(12f);
            BattleFeedbackManager.Instance.RippleScreen(player.ParticlePoint.position.x, player.ParticlePoint.position.y);
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
            BattleFeedbackManager.Instance.RippleScreen(player.ParticlePoint.position.x, player.ParticlePoint.position.y);

            yield return new WaitForSeconds(4);
            SetTarget(null);
            player.LockController.Targeting = true;
            yield return new WaitForSeconds(1);
            canvasBattle.SetActive(false);
            OnEventBattleEnd.Invoke();
            player.CanPlay(true);


        }

        #endregion

    } 

} // #PROJECTNAME# namespace