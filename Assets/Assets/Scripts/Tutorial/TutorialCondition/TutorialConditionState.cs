using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace Tutorial
{
	public class TutorialConditionState : TutorialCondition
	{
		[SerializeField]
		bool isPlayer = false;
		[SerializeField]
		CharacterStateID stateID;
		bool ok = false;

		public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
		{
			if(isPlayer)
				player.OnStateChanged += CallbackState;
			else
				dummy.OnStateChanged += CallbackState;

			ok = false;
		}

		public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
		{
			return ok;
		}

		public override void EndCondition(CharacterBase player, CharacterBase dummy)
		{
			if (isPlayer)
				player.OnStateChanged -= CallbackState;
			else
				dummy.OnStateChanged -= CallbackState;
		}

		private void CallbackState(CharacterState oldState, CharacterState newState)
		{
			if(newState.ID == stateID)
				ok = true;
		}

	}
}
