

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [CreateAssetMenu(fileName = "FloorData", menuName = "FloorData", order = 1)]
    public class FloorLayoutData : ScriptableObject
    {

        [SerializeField]
        private string floorName;
        public string FloorName
        {
            get { return floorName; }
        }

        [SerializeField]
        private int floorLevel;
        public int FloorLevel
        {
            get { return floorLevel; }
        }

        [Title("Layout")]
        [SerializeField]
        CardExplorationData firstRoom = null;
        public CardExplorationData FirstRoom
        {
            get { return firstRoom; }
        }

        [SerializeField]
        List<CardExplorationData> floorLayout = null;
        public List<CardExplorationData> FloorLayout
        {
            get { return floorLayout; }
        }

        [SerializeField]
        FloorLayoutData nextLevel = null;

        [Title("Banlist")]
        [SerializeField]
        List<CardData> cardBanlist;
        [SerializeField]
        List<CardExplorationData> cardExplorationBanlist;

        [SerializeField]
        AudioClip bgm = null;
        public AudioClip Bgm
        {
            get { return bgm; }
        }



        public FloorLayoutData GetFloor(int floor)
        {
            floor -= 1;
            if (floor > 0)
                return nextLevel.GetFloor(floor);
            return this;
        }

    }

} // #PROJECTNAME# namespace
