using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace Tutorial
{
	public class TutorialConditionInput : TutorialCondition
	{
		[SerializeField]
		InputEnum input;
		[SerializeField]
		int nbInput = 7;

		int nb = 0;

		bool ok = false;
		public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
		{
			ok = false;
			nb = nbInput;
		}

		public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
		{
			if (player.State.ID == CharacterStateID.Idle)
            {
				if(player.Inputs.GetInput(input))
				{
					nb -= 1;
					if (nb < 0)
						ok = true;
				}
			}

			return ok;
		}


	}
}
