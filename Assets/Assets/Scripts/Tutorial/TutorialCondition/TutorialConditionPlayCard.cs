using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace Tutorial
{
	public class TutorialConditionPlayCard : TutorialCondition
	{
		[SerializeField]
		int nbToPlay = 7;
		int nb = 0;

		bool ok = false;

		public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
		{
			player.DeckController.OnCardPlayed += CallbackPlayCard;
			ok = false;
			nb = nbToPlay;
		}

		public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
		{
			return ok;
		}
		public override void EndCondition(CharacterBase player, CharacterBase dummy)
		{
			player.DeckController.OnCardPlayed -= CallbackPlayCard;
		}

		private void CallbackPlayCard(int i, List<Card> deck)
		{
			nb -= 1;
			if(nb <= 0)
				ok = true;
		}

	}
}
