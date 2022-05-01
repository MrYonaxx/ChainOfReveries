using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace VoiceActing
{

    public class AutoAddStatus : MonoBehaviour
    {
        [SerializeField]
        CharacterStatusController statusController = null;
        [SerializeField]
        StatusEffectData[] status = null;

        private void Start()
        {
            for (int i = 0; i < status.Length; i++)
            {
                statusController.ApplyStatus(status[i], 1000);
            }
        }

    }
}
