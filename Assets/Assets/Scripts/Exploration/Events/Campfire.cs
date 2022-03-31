using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class Campfire : MonoBehaviour, IInteractable
    {

        [SerializeField]
        GameRunData gameRunData = null;
        [SerializeField]
        [SuffixLabel("%")]
        float hpRecover = 0.25f;

        [Title("UI")]
        [SerializeField]
        Animator campAnimator = null;
        [SerializeField]
        GameObject buttonPrompt = null;

        bool used = false;

        public void CanInteract(bool b)
        {
            if (used)
                return;
            buttonPrompt.gameObject.SetActive(b);
        }

        public void Interact(CharacterBase character)
        {
            if (used)
                return;

            character.CharacterStat.HP += character.CharacterStat.HPMax.Value * hpRecover;
            campAnimator.SetTrigger("Feedback");

            CanInteract(false);
            used = true;
        }

    }
}
