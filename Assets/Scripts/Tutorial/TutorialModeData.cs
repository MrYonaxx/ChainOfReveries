using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

namespace Tutorial
{
	[System.Serializable]
	public class TrialsDisplay
	{
		[HorizontalGroup]
		[HideLabel]
		public bool showInput = true;
		[HorizontalGroup]
		[HideLabel]
		[ShowIf("showInput")]
		public VoiceActing.InputEnum input;
		[HorizontalGroup]
		[HideLabel]
		public VoiceActing.SleightData SleightData;
	}

	[CreateAssetMenu(fileName = "TrialsData_", menuName = "Data/Training/TrialsData", order = 1)]
	public class TutorialModeData : SerializedScriptableObject
	{
		[SerializeField]
		string trialsName = "";
		public string TrialsName
		{
			get { return trialsName; }
		}

		[SerializeField]
		[TextArea]
		string trialsDescription = "";
		public string TrialsDescription
		{
			get { return trialsDescription; }
		}


		[Title("Setup")]

		/*[HorizontalGroup("Player")]
		[SerializeField]
		CharacterData player = null;
		public CharacterData Player
		{
			get { return player; }
		}


		[HorizontalGroup("Enemy")]
		[SerializeField]
		CharacterData dummy = null;
		public CharacterData Dummy
		{
			get { return dummy; }
		}*/

		[SerializeField]
		VoiceActing.DeckData deckPlayer = null;
		public VoiceActing.DeckData DeckPlayer
		{
			get { return deckPlayer; }
		}

		[SerializeField]
		VoiceActing.DeckData deckEnemy = null;
		public VoiceActing.DeckData DeckEnemy
		{
			get { return deckEnemy; }
		}

		[SerializeField]
		BehaviorDesigner.Runtime.ExternalBehavior dummyBehavior = null;
		public BehaviorDesigner.Runtime.ExternalBehavior DummyBehavior
		{
			get { return dummyBehavior; }
		}



		[Space]
		[Title("Texts")]
		[SerializeField]
		[HideLabel]
		private string osef = "";

		[TabGroup("Texts", "TextStart")]
		[SerializeField]
		[ListDrawerSettings(Expanded = true)]
		[TextArea(1, 1)]
		List<string> textboxStart = new List<string>();
		public List<string> TextboxStart
		{
			get { return textboxStart; }
		}

		[TabGroup("Texts", "TextEnd")]
		[SerializeField]
		[ListDrawerSettings(Expanded = true)]
		[TextArea(1, 1)]
		List<string> textboxEnd = new List<string>();
		public List<string> TextboxEnd
		{
			get { return textboxEnd; }
		}


		[Space]
		[Title("Mission")]
		[OdinSerialize]
		[SerializeField]
		private List<TutorialCondition> missions = new List<TutorialCondition>();
		public List<TutorialCondition> Missions
		{
			get { return missions; }
		}

		[HorizontalGroup("Notes")]
		[SerializeField]
		List<string> comboNotes = new List<string>();
		public List<string> ComboNotes
		{
			get { return comboNotes; }
		}

		[HorizontalGroup("Notes")]
		[SerializeField]
		List<TrialsDisplay> trialsDisplay = new List<TrialsDisplay>();
		public List<TrialsDisplay> TrialsDisplay
		{
			get { return trialsDisplay; }
		}

		[Space]
		[Title("Fail Conditions")]

		[SerializeField]
		private List<TutorialCondition> failConditions = new List<TutorialCondition>();
		public List<TutorialCondition> FailConditions
		{
			get { return failConditions; }
		}

	}
}
