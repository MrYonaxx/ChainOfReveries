using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing 
{
    public class TutorialBuff : MonoBehaviour
    {
        [SerializeField]
        CharacterBase character = null;

        // Start is called before the first frame update
        void Start()
        {
            character.CharacterStat.ReloadAmount.AddStatBonus(100, Stats.StatBonusType.Flat);
            character.OnStateChanged += Regen;
        }
        private void OnDestroy()
        {
            character.OnStateChanged -= Regen;
        }

        void Regen(CharacterState oldState, CharacterState newState)
        {
            if (newState.ID == CharacterStateID.Idle)
            {
                character.CharacterStat.HP += 9999;
                character.CharacterKnockback.AddRevengeValue(-15f);
            }
        }



    }
}
