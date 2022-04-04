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
        AudioClip music;

        [Title("Reload Sequence")]
        [SerializeField]
        CardData cardStar;
        [SerializeField]
        float reloadGain = 30f;
        [SerializeField]
        AttackManager nextAction;

        float reloadAmount = 100;

        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            base.StartComponent(character, attack);
            player = character.LockController.TargetLocked;
            shimerie = character;

            player.SpriteRenderer.enabled = false;

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
            player.DeckController.RefreshDeck();

            charactersSequence[runData.CharacterID].SetActive(true);

            StartCoroutine(FinalAttackCoroutine());
        }

        private IEnumerator FinalAttackCoroutine()
        {
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
        }

        private void Reload()
        {
            player.DeckController.OnReload -= Reload;
            shimerie.CharacterAction.CancelAction();
            shimerie.CharacterAction.Action(nextAction);
            player.CanPlay(false);
        }
 
    }
}
