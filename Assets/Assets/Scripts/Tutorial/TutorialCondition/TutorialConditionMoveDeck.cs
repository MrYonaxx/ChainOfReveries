using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace Tutorial
{
	public class TutorialConditionMoveDeck : TutorialCondition
	{
		[SerializeField]
		int nbToMove = 7;
		int nb = 0;

		bool ok = false;
		public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
		{
			player.DeckController.OnCardMoved += CallbackMoveCard;
			ok = false;
			nb = nbToMove;
		}

		public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
		{
			return ok;
		}
		public override void EndCondition(CharacterBase player, CharacterBase dummy)
		{
			player.DeckController.OnCardMoved -= CallbackMoveCard;
		}

		private void CallbackMoveCard(int i, List<Card> deck)
		{
			nb -= 1;
			if (nb <= 0)
				ok = true;
		}

	}
}
