using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Stats;

namespace VoiceActing
{
    public class CharAnimation_Reload : CharacterAnimationEvent
    {
        [SerializeField]
        bool restoreBanishCard = false;

        public override void Execute(CharacterBase character)
        {
            if(restoreBanishCard)
                character.DeckController.UnbanishCard();

            character.DeckController.ReloadDeck();
        }


    }
}
