using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using TMPro;

namespace VoiceActing
{
    public class MenuRoulette : MonoBehaviour, IControllable
    {
        [SerializeField]
        float rouletteSpeed = 0.06f;
        [SerializeField]
        DeckBattleDrawer deckDrawer = null;

        [Title("UI")]
        [SerializeField]
        GameObject rouletteUI = null;
        [SerializeField]
        Animator animatorRoulette = null;
        [SerializeField]
        TextMeshProUGUI textCardName = null;
        [SerializeField]
        TextMeshProUGUI textCardDescription = null;

        [SerializeField]
        SoundParameter soundShuffle = null;
        [SerializeField]
        SoundParameter soundValidate = null;
        [SerializeField]
        SoundParameter soundQuit = null;

        int index = 0; 

        bool active = false;
        bool reward = false;
        float t = 0f;

        List<Card> deck;

        public delegate void ActionInt(int i);
        public event ActionInt OnCardSelected;


        public void CreateRoulette(List<Card> cards)
        {

            deck = cards;
            rouletteUI.SetActive(true);
            deckDrawer.DrawHand(index, cards);
            deckDrawer.HideCards();
            active = true;
        }

        public void UpdateControl(InputController input)
        {
            if (active == false)
                return;

            if (reward)   // Menu Reward
            {
                if (input.InputA.Registered)
                {
                    input.InputA.ResetBuffer(); 
                    soundQuit.PlaySound();
                    QuitMenu();
                    return;
                }
                return;
            }
            else          // Menu Roulette
            {
                // Stop la roulette
                if (input.InputA.Registered)
                {
                    input.InputA.ResetBuffer();
                    soundValidate.PlaySound();
                    DrawCardSelectedMenu();
                    reward = true;
                    return;
                }


                t -= Time.deltaTime;
                if (t <= 0f)
                {
                    index += 1;
                    if (index >= deck.Count)
                        index = 0;
                    deckDrawer.MoveHand(index, deck);
                    soundShuffle.PlaySound();
                    t = rouletteSpeed;
                }
            }
        }


        private void DrawCardSelectedMenu()
        {
            animatorRoulette.SetTrigger("Feedback");
            textCardName.text = deck[index].GetCardName();
            textCardDescription.text = deck[index].GetCardDescription();
        }

        public void QuitMenu()
        {
            active = false;
            OnCardSelected.Invoke(index);
            rouletteUI.SetActive(false);
        }
    }
}
