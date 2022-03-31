using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    public class SoundController_Deck : MonoBehaviour
    {
        [SerializeField]
        DeckController deckController = null;

        [SerializeField]
        SoundParameter soundMove;
        [SerializeField]
        SoundParameter soundReload;

        // Start is called before the first frame update
        void Start()
        {
            deckController.OnCardMoved += SoundCardMove;
            deckController.OnReload += SoundReload;
        }

        // Update is called once per frame
        void OnDestroy()
        {
            deckController.OnCardMoved -= SoundCardMove;
            deckController.OnReload -= SoundReload;
        }

        void SoundCardMove(int i, List<Card> deck)
        {
            soundMove.PlaySound();
        }

        void SoundReload()
        {
            soundReload.PlaySound();
        }
    }
}
