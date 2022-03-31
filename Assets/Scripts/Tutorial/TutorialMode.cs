using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

using Menu;
using VoiceActing;

namespace Tutorial
{
	// Repris de break punch et flemme de changer les noms
	public class TutorialMode : MonoBehaviour, IControllable
	{
		[Title("Logic")]
		[SerializeField]
		InputController inputController = null;
		[SerializeField]
		BattleVersusManager battleManager = null;
		[SerializeField]
		CharacterBase player = null;
		[SerializeField]
		AIController dummy = null;
		[SerializeField]
		Transform[] spawnPoints;
		[SerializeField]
		Textbox textbox = null;

		[Title("UI")]
		[SerializeField]
		TutorialPanelDrawer missionModePanel = null;
		[SerializeField]
		Transform parentMissionMode = null;
		[SerializeField]
		GameObject animatorSuccess = null;

		int textIndex = -1;
		int comboIndex = 0;
		List<CardEquipment> p1Equipments = new List<CardEquipment>();
		List<CardEquipment> p2Equipments = new List<CardEquipment>();

		bool success = false;
		List<TutorialPanelDrawer> missionModePanels = new List<TutorialPanelDrawer>();

		TutorialModeData trialsData = null;
		public event UnityAction OnEnd;

		private void Awake()
		{
			textbox.OnTextEnd += NextText;
		}

		private void OnDestroy()
		{
			EndFailConditions();
			textbox.OnTextEnd -= NextText;
		}

		private void Start()
        {
			dummy.StopBehavior();
			battleManager.InitializeBattle(player, dummy.Character);
		}

        public void InitializeTrial(TutorialModeData newTrials)
		{
			success = false;
			textIndex = 0;
			comboIndex = 0;
			trialsData = newTrials;

			for (int i = 0; i < missionModePanels.Count; i++)
			{
				missionModePanels[i].Hide();
			}
			missionModePanels.Clear();

			InitializeTrial();
		}

		public void InitializeTrial()
		{
			// On prépare le deck équipement (ça sert pour un seul défi du tuto mais bon)
			p1Equipments.Clear();
			for (int i = 0; i < trialsData.DeckPlayer.InitialEquipment.Length; i++)
				p1Equipments.Add(new CardEquipment(trialsData.DeckPlayer.InitialEquipment[i].cardEquipment));

			p2Equipments.Clear();
			for (int i = 0; i < trialsData.DeckEnemy.InitialEquipment.Length; i++)
				p2Equipments.Add(new CardEquipment(trialsData.DeckEnemy.InitialEquipment[i].cardEquipment));
			battleManager.ReinitializeBattle(trialsData.DeckPlayer.CreateDeck(), trialsData.DeckEnemy.CreateDeck(), p1Equipments, p2Equipments);


			dummy.SetNewBehavior(trialsData.DummyBehavior);


			trialsData.Missions[comboIndex].InitializeCondition(player, dummy.Character);

			// UI
			missionModePanels = new List<TutorialPanelDrawer>(trialsData.Missions.Count);
			for (int i = 0; i < trialsData.Missions.Count; i++)
			{
				if(i >= missionModePanels.Count)
					missionModePanels.Add(Instantiate(missionModePanel, parentMissionMode));

				missionModePanels[i].gameObject.SetActive(true);
				missionModePanels[i].DrawButton(trialsData.TrialsDisplay[i].showInput, (int)trialsData.TrialsDisplay[i].input);
				missionModePanels[i].DrawItem(trialsData.ComboNotes[i]);
				missionModePanels[i].DrawSleight(trialsData.TrialsDisplay[i].SleightData);
			}
			animatorSuccess.SetActive(false);

			// Text
			if (trialsData.TextboxStart.Count != 0)
			{
				textIndex = 0;
				textbox.gameObject.SetActive(true);
				textbox.DrawTextbox(trialsData.TextboxStart[textIndex]);
				inputController.SetControllable(textbox, true);
			}
			else
			{
				StartTrial();
			}
		}

		private void StartTrial()
        {
			InitializeFailConditions();
			inputController.SetControllable(this);
			dummy.StartBehavior();
		}




		private void InitializeFailConditions()
		{
			for (int i = 0; i < trialsData.FailConditions.Count; i++)
			{
				trialsData.FailConditions[i].InitializeCondition(player, dummy.Character);
			}
		}

		private void EndFailConditions()
		{
			if (trialsData == null)
				return;
			for (int i = 0; i < trialsData.FailConditions.Count; i++)
			{
				trialsData.FailConditions[i].EndCondition(player, dummy.Character);
			}
		}



        public void UpdateControl(InputController input)
		{
			if (success)
			{
				player.UpdateControl(input);
				return;
			}

			if (inputController.InputStart.Registered) // Skip
			{
				inputController.ResetAllBuffer();
				dummy.StopBehavior();
				EndTrial();
				return;
			}

			player.UpdateControl(input);

			// Missions
			if (trialsData.Missions[comboIndex].UpdateCondition(player, dummy.Character))
			{
				// Sous-mission validé
				missionModePanels[comboIndex].ValidateButton();
				trialsData.Missions[comboIndex].EndCondition(player, dummy.Character);
				comboIndex += 1;

				// Mission validé
				if (comboIndex == trialsData.Missions.Count)
				{
					SuccessTrial();
				}
				else
				{
					trialsData.Missions[comboIndex].InitializeCondition(player, dummy.Character);
				}
			}

			// Fail
			for (int i = 0; i < trialsData.FailConditions.Count; i++)
			{
				if (trialsData.FailConditions[i].UpdateCondition(player, dummy.Character))
				{
					ResetMission();
				}
			}
		}



		public void ResetMission()
		{   
			// UI
			for (int i = 0; i < comboIndex; i++)
			{
				missionModePanels[i].ResetButton();
			}

			// On arrête tout
			if (comboIndex < trialsData.Missions.Count)
				trialsData.Missions[comboIndex].EndCondition(player, dummy.Character);
			EndFailConditions();

			// On initialise
			comboIndex = 0;
			trialsData.Missions[comboIndex].InitializeCondition(player, dummy.Character);
			InitializeFailConditions();

		}

		public void ResetTrial()
		{
			// On reinitialise le combat
			battleManager.ReinitializeBattle(trialsData.DeckPlayer.CreateDeck(), trialsData.DeckEnemy.CreateDeck(), p1Equipments, p2Equipments);

			ResetMission();
		}


		public void SuccessTrial()
		{
			success = true;
			animatorSuccess.SetActive(true);
			EndFailConditions();

			dummy.StopBehavior();


			StartCoroutine(WaitSuccess());

		}

		private IEnumerator WaitSuccess()
		{
			yield return new WaitForSeconds(1.5f);
			player.CharacterMovement.InMovement = false;
			inputController.SetControllable(textbox, true);

			if (trialsData.TextboxEnd.Count != 0)
			{
				textIndex = 0;
				textbox.DrawTextbox(trialsData.TextboxEnd[textIndex]);
			}
			else
			{
				EndTrial();
			}
		}




		public void NextText()
		{
			textIndex += 1;
			if (success == false) // C'est le texte du début 
			{
				if (textIndex >= trialsData.TextboxStart.Count)
				{
					textIndex = -1;
					StartTrial();
				}
				else
				{
					textbox.DrawTextbox(trialsData.TextboxStart[textIndex]);
				}
			}
			else // C'est le texte de fin
			{
				if (textIndex >= trialsData.TextboxEnd.Count)
				{
					textIndex = -1;
					EndTrial();
				}
				else
				{
					textbox.DrawTextbox(trialsData.TextboxEnd[textIndex]);
				}
			}

		}

		public void EndTrial()
		{
			StartCoroutine(EndTrialCoroutine());
		}

		private IEnumerator EndTrialCoroutine()
		{
			yield return new WaitForSeconds(0.5f);
			OnEnd?.Invoke();
		}




	}
}
