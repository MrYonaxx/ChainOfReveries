using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    [System.Serializable]
    public class DeckDatabaseProfile
    {
        [SerializeField]
        public PlayerData PlayerData = null;
        [SerializeField]
        public List<DeckData> DeckDatas = new List<DeckData>();
    }


    [CreateAssetMenu(fileName = "DeckDatabase", menuName = "Data/DeckDatabase", order = 1)]
    public class DeckDatabase : ScriptableObject
    {
        [SerializeField]
        DeckDatabaseProfile[] deckProfiles;
        public DeckDatabaseProfile[] DeckProfiles
        {
            get { return deckProfiles; }
        }

        public List<DeckData> GetDeck(PlayerData player)
        {
            for (int i = 0; i < deckProfiles.Length; i++)
            {
                if (deckProfiles[i].PlayerData == player)
                    return deckProfiles[i].DeckDatas;
            }
            return null;
        }

    }
}
