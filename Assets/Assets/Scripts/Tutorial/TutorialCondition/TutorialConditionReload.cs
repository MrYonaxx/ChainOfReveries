using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace Tutorial
{
	public class TutorialConditionReload : TutorialCondition
	{
		bool ok = false;

		public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
		{
			player.DeckController.OnReload += CallbackReload;
			ok = false;
		}

		public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
		{
			return ok;
		}
		public override void EndCondition(CharacterBase player, CharacterBase dummy)
		{
			player.DeckController.OnReload -= CallbackReload;
		}

		private void CallbackReload()
		{
			ok = true;
		}

	}
}
