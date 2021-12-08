using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing 
{
    public interface IInteractable
    {
        void CanInteract(bool b);
        void Interact(CharacterBase character);
    }
}
