using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Stats;

namespace VoiceActing
{
    public class CharAnimation_RemoveEquipmentCards : CharacterAnimationEvent
    {
        [SerializeField]
        CardEquipmentData cardToRemove = null;
        [SerializeField]
        GameRunData gameRunData = null; // y'a un truc à refactor là

        public override void Execute(CharacterBase character)
        {
            character.CharacterEquipment.UnequipCard(cardToRemove, 0);
            gameRunData.RemoveEquipmentCard(cardToRemove);
        }


    }
}
