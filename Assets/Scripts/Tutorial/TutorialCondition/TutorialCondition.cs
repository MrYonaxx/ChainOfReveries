using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace Tutorial
{
	public class TutorialCondition
	{

		public virtual void InitializeCondition(CharacterBase player, CharacterBase dummy)
		{

		}

		public virtual bool UpdateCondition(CharacterBase player, CharacterBase dummy)
		{
			return false;
		}

		public virtual void EndCondition(CharacterBase player, CharacterBase dummy)
		{

		}
	}
}
