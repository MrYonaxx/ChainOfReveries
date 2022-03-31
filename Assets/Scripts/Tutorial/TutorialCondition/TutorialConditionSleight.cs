using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace Tutorial
{
	public class TutorialConditionSleight: TutorialCondition
	{
		bool ok = false;

		public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
		{
			player.SleightController.OnSleightUpdate += CallbackSleight;
			ok = false;
		}

		public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
		{
			return ok;
		}
		public override void EndCondition(CharacterBase player, CharacterBase dummy)
		{
			player.SleightController.OnSleightUpdate -= CallbackSleight;
		}

		private void CallbackSleight(SleightData currentSleight, List<Card> sleightCards)
		{
			if(sleightCards.Count == 3)
				ok = true;
		}

	}
}
