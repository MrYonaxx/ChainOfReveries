using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace Tutorial
{
	public class TutorialConditionAction : TutorialCondition
	{
		[SerializeField]
		AttackManager action=null;

		bool ok = false;

		public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
		{
			player.CharacterAction.OnAction += CallbackAction;
			ok = false;
		}

		public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
		{
			return ok;
		}

        public override void EndCondition(CharacterBase player, CharacterBase dummy)
        {
			player.CharacterAction.OnAction -= CallbackAction;
		}

        private void CallbackAction(AttackManager attack, Card card)
        {
			if(attack.nameID == action.nameID)
				ok = true;
		}
	}
}
