using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{

    [System.Serializable]
    public class EncounterDataBoss
    {
        public CharacterData Character;
        public EncounterData[] BossEvents;
    }

    [CreateAssetMenu(fileName = "EncounterDatabaseBoss", menuName = "CardDatabase/EncounterDatabaseBoss", order = 1)]
    public class EncounterDatabaseBoss : ScriptableObject
    {
        [SerializeField]
        EncounterDataBoss[] bosses = null;

        public CharacterData LastBossSelected = null;
        List<EncounterDataBoss> bossesPool = null;

        public void Reset()
        {
            bossesPool.Clear();
            bossesPool = new List<EncounterDataBoss>(bosses);
        }

        public EncounterData SelectBoss(int difficultyLevel)
        {
            // Si difficulty level == 0 c'est qu'on pioche un boss pour la première fois
            // Attention c'est possible dans le futur que ce ne soit pas toujours vrai
            if (difficultyLevel == 0)
            {
                Reset();
                Debug.Log("I reset");
            }

            // Choisis un boss random
            EncounterData bossRoom;
            int r = Random.Range(0, bossesPool.Count);
            bossRoom = bossesPool[r].BossEvents[difficultyLevel];
            LastBossSelected = bossesPool[r].Character;

            // Retire ce boss du pool
            bossesPool.Remove(bossesPool[r]);

            return bossRoom;
        }

	}
}
