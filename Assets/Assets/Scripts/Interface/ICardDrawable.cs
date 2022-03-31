using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing 
{
    public interface ICard
    {
        Sprite GetCardIcon();
        string GetCardName();

        int GetCardType();
        string GetCardDescription();
        int GetCardValue();
    }

    public interface ICardDrawable
    {
        Sprite GetCardIcon();
        string GetCardName();

        int GetCardType();
        string GetCardDescription();
        int GetCardValue();
    }
}
