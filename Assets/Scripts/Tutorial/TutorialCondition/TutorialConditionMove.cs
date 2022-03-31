using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace Tutorial
{
	public class TutorialConditionMove : TutorialCondition
	{
		[SerializeField]
		float moveTime = 1f;

		float t = 0f;

        public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
        {
			t = 0f;
        }

        public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
		{
			if (player.CharacterMovement.InMovement && player.State.ID == CharacterStateID.Idle)
				t += Time.deltaTime;
			if(t>=moveTime)
				return true;
			return false;
		}

	}
}
