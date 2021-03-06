using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VoiceActing
{
    public class BattleInvisible : MonoBehaviour
    {
        [SerializeField]
        ExplorationEventBattle eventBattle = null;

        private void Start()
        {
            for (int i = 0; i < eventBattle.Encounter.Encounter.Length; i++)
            {
                eventBattle.Encounter.Encounter[i].Character.SpriteRenderer.GetComponentInChildren<CharacterCardValueHUD>().enabled = false;
                eventBattle.Encounter.Encounter[i].Character.SpriteRenderer.GetComponentInChildren<CharacterCardValueHUD>().gameObject.SetActive(false);
                eventBattle.Encounter.Encounter[i].Character.SpriteRenderer.material.SetFloat("_Disappear", 1);
            }
        }

    }
}
