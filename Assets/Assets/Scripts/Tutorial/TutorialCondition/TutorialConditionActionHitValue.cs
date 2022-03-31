using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace Tutorial
{
	public class TutorialConditionActionHitValue: TutorialCondition
	{
		[SerializeField]
		int cardValue = 1;

		bool ok = false;

		public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
		{
			player.CharacterAction.OnAttackHit += CallbackAction;
			ok = false;
		}

		public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
		{
			return ok;
		}

		public override void EndCondition(CharacterBase player, CharacterBase dummy)
		{
			player.CharacterAction.OnAttackHit -= CallbackAction;
		}

		private void CallbackAction(AttackController attack, CharacterBase character)
		{
			if (attack.Card.GetCardValue() == cardValue)
				ok = true;
		}

	}
}
