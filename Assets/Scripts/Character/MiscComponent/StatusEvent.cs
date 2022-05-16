using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace VoiceActing
{

    public class StatusEvent : MonoBehaviour
    {
        [SerializeField]
        CharacterStatusController statusController = null;
        [SerializeField]
        StatusEffectData status = null;

        private void Start()
        {
            statusController.OnStatusChanged += UpdateCall;
            this.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            statusController.OnStatusChanged -= UpdateCall;
        }

        public void UpdateCall(List<Status> statusList)
        {
            gameObject.SetActive(statusController.ContainStatus(status));
        }

    }
}
