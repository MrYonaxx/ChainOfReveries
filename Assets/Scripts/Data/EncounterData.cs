using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class EncounterData : MonoBehaviour
    {
        [SerializeField]
		[ListDrawerSettings(Expanded = true)]
		private AIController[] encounter;
		public AIController[] Encounter
		{
			get { return encounter; }
		}

		[SerializeField]
		[ListDrawerSettings(Expanded = true)]
		private Sprite[] encounterBackground;
		public Sprite[] EncounterBackground
		{
			get { return encounterBackground; }
		}


		public Sprite GetBackground()
		{
			return encounterBackground[Random.Range(0, encounterBackground.Length)];
		}

		[Button]
		public void GetEncounter()
		{
			encounter = GetComponentsInChildren<AIController>();
		}

	}
}
