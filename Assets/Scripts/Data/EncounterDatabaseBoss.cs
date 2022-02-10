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

        List<EncounterDataBoss> bossesPool = null;

        // Hack pour l'editeur pour auto reset l'objet
        CheckInitialize checkInitialize = null;

        public void Reset()
        {
            bossesPool.Clear();
            bossesPool = new List<EncounterDataBoss>(bosses);
        }

        public EncounterData SelectBoss(int difficultyLevel)
        {
            if (checkInitialize == null)
            {
                checkInitialize = new CheckInitialize();
                Reset();
                Debug.Log("I reset");
            }

            // Choisis un boss random
            EncounterData bossRoom;
            int r = Random.Range(0, bossesPool.Count);
            bossRoom = bossesPool[r].BossEvents[difficultyLevel];
            // Retire ce boss du pool
            bossesPool.Remove(bossesPool[r]);
            return bossRoom;
        }

	}
}
