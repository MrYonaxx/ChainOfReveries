using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace Tutorial
{
	public class TutorialConditionEquip : TutorialCondition
	{

		bool ok = false;

		public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
		{
			player.CharacterEquipment.OnEquipWeapon += CallbackEquipment;
			ok = false;
		}

		public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
		{
			return ok;
		}

        public override void EndCondition(CharacterBase player, CharacterBase dummy)
        {
			player.CharacterEquipment.OnEquipWeapon -= CallbackEquipment;
		}

        private void CallbackEquipment(CardEquipment[] equipments)
        {
			ok = true;
		}
	}
}
