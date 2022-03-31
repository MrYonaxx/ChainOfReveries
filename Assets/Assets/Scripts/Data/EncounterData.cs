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
		[SerializeField]
		public AudioClip BattleTheme = null;



		// Cinematic intro d'un boss (à bouger dans un éventuel encounterdataboss peut etre)
		[Space]
		[Space]
		[Space]
		[Title("Cinematic")]
		[SerializeField]
		public bool HasIntro = false;

		[ShowIf("HasIntro")]
		[SerializeField]
		Transform playerPos = null;
		[ShowIf("HasIntro")]
		[SerializeField]
		GameObject enemyCinematic = null;
		[ShowIf("HasIntro")]
		[SerializeField]
		MatchIntroDrawer introDrawer = null;



		public Sprite GetBackground()
		{
			return encounterBackground[Random.Range(0, encounterBackground.Length)];
		}


		[Button]
		public void GetEncounter()
		{
			encounter = GetComponentsInChildren<AIController>();
		}


		// Intro par défaut d'un boss
		public virtual IEnumerator IntroCinematic(CharacterBase player)
		{
			// Stop le joueur
			player.CanPlay(false);
			player.Inputs.InputLeftStickX.InputValue = 0;
			player.Inputs.InputLeftStickY.InputValue = 0;

			// Fais marcher le joueur jusqu'au point
			float t = 0;
			float time = 1;
			while (t < time)
			{
				t += Time.deltaTime;
				if (player.CharacterMovement.MoveToPoint(playerPos.position, 2f))
				{
					player.CharacterMovement.InMovement = false;
					player.CharacterMovement.Move(0, 0);
				}
				yield return null;
			}
			player.CharacterMovement.InMovement = false;
			player.CharacterMovement.Move(0, 0);

			yield return new WaitForSeconds(1f);

			// Focus sur le boss
			BattleFeedbackManager.Instance.CameraController.AddTarget(enemyCinematic.transform, 0);
			AudioManager.Instance?.PlayMusic(BattleTheme);
			yield return new WaitForSeconds(3f);

			// Animator du boss
			enemyCinematic.gameObject.SetActive(true);
			yield return new WaitForSeconds(9f);

			// Affiche l'intro style arc sys
			introDrawer.DrawIntro((PlayerData)player.CharacterData, null);
			yield return new WaitForSeconds(3f);

			// Idéalement le combat doit commencer seconde 16
			BattleFeedbackManager.Instance.CameraController.RemoveTarget(enemyCinematic.transform);
			enemyCinematic.gameObject.SetActive(false);
			yield return null;
		}

	}
}
