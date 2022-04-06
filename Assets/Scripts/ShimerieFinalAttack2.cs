using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class ShimerieFinalAttack2 : AttackComponent
    {

        CharacterBase player;
        CharacterBase shimerie;

        [SerializeField]
        GameRunData runData;
        [SerializeField]
        GameObject[] charactersSequence;
        [SerializeField]
        BlinkScript[] blinkScripts;
        [SerializeField]
        AudioClip music;

        [Title("Reload Sequence")]
        [SerializeField]
        CardData cardStar;
        [SerializeField]
        float reloadGain = 30f;
        [SerializeField]
        float intervalBlink = 0.1f;
        [SerializeField]
        Color colorBlink = Color.blue;
        [SerializeField]
        Animator reloadAnimator;
        [SerializeField]
        AttackManager nextAction;

        float reloadAmount = 100;
        float tReload = 0f;
        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            base.StartComponent(character, attack);
            player = character.LockController.TargetLocked;
            shimerie = character;

            player.SpriteRenderer.enabled = false;
            player.SpriteRenderer.color = new Color(player.SpriteRenderer.color.r, player.SpriteRenderer.color.g, player.SpriteRenderer.color.b, 0);

            List<Card> finalDeck = new List<Card>(150);
            for (int i = 0; i < 150; i++)
            {
                finalDeck.Add(new Card(cardStar, 0, true));
            }
            player.DeckController.SetDeck(finalDeck);
            for (int i = 0; i < 150; i++)
            {
                player.DeckController.Remove(1);
            }
            player.DeckController.SetReloadMax(50);
            player.DeckController.AddReload(0);
            player.DeckController.RefreshDeck();

            charactersSequence[runData.CharacterID].SetActive(true);

            StartCoroutine(FinalAttackCoroutine());
        }

        private IEnumerator FinalAttackCoroutine()
        {
            BattleUtils.Instance.BattleParticle.Play();
            yield return new WaitForSeconds(0.5f);
            AudioManager.Instance.PlayMusic(music);
            yield return new WaitForSeconds(5.5f);
            /*charactersSequence[runData.CharacterID].SetActive(false);
            player.SpriteRenderer.enabled = true;*/

            player.CanPlay(true);
            player.DeckController.OnReload += Reload;
            player.DeckController.OnReloadChanged += ReloadChanged;

        }


        private void ReloadChanged(int reloadCurrentLevel, float reloadCurrentAmount, int reloadMaxAmount)
        {
            reloadAmount += reloadGain * Time.deltaTime;
            reloadAmount = Mathf.Min(reloadAmount, 1000);
            player.CharacterStat.ReloadAmount.BaseValue = reloadAmount;

            UpdateReload();

            if (reloadCurrentLevel >= 3)
                reloadAnimator.gameObject.SetActive(true);

        }
        private void UpdateReload()
        {
            tReload += Time.deltaTime;
            if (tReload > intervalBlink)
            {
                blinkScripts[runData.CharacterID].Blink(intervalBlink, colorBlink);
                tReload = 0f;
            }
        }
        private void Reload()
        {
            player.DeckController.OnReload -= Reload;
            player.DeckController.OnReloadChanged -= ReloadChanged;
            shimerie.CharacterAction.CancelAction();
            shimerie.CharacterAction.Action(nextAction);
            player.CanPlay(false);

            BattleFeedbackManager.Instance.CameraController.FocusLevel.transform.localScale = new Vector3(1, 2, 1);
            BattleUtils.Instance.BattleParticle.Stop();
        }
 
    }
}
