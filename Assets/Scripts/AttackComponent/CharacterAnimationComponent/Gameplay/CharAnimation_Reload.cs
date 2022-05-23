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
        float reloadAmount = -1;
        [SerializeField]
        bool restoreBanishCard = false;

        public override void Execute(CharacterBase character)
        {
            if (reloadAmount == -1)
            {
                if (restoreBanishCard)
                    character.DeckController.UnbanishCard();

                character.DeckController.ReloadDeck();
            }
            else
            {
                character.DeckController.AddReload(reloadAmount);
            }
        }


    }
}
