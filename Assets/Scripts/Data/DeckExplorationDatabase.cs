using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    [System.Serializable]
    public class DeckExplorationDatabaseProfile
    {
        [SerializeField]
        public string DeckName = null;
        [SerializeField]
        public List<CardExplorationData> DeckDatas = new List<CardExplorationData>();
    }


    [CreateAssetMenu(fileName = "DeckDatabase", menuName = "Data/DeckExplorationDatabase", order = 1)]
    public class DeckExplorationDatabase : ScriptableObject
    {
        [SerializeField]
        DeckExplorationDatabaseProfile[] deckProfiles;
        public DeckExplorationDatabaseProfile[] DeckProfiles
        {
            get { return deckProfiles; }
        }

    }
}
