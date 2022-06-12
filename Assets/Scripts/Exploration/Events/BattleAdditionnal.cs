using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VoiceActing
{
    public class BattleAdditionnal: MonoBehaviour
    {
        [SerializeField]
        GameRunData runData = null;

        [SerializeField]
        CardExplorationData emptyRoom = null;

        private void Start()
        {
            runData.RemoveLastRoom(emptyRoom);
        }

    }
}
