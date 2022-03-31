using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

/*namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedSleightData : SharedVariable<AISleightData>
    {
        public static implicit operator SharedSleightData(AISleightData value) { return new SharedSleightData { mValue = value }; }
    }
}*/

namespace VoiceActing 
{
    public class AISleightData
    {
        //[HideInInspector]
        public SleightData Sleight = null;
        public bool active = false;
        //public int size = 0;
        //public List<int> cardsIndex;

        public void SetActive()
        {
            active = true;
        }
        public bool Active()
        {
            return active;
        }
        public void Reset()
        {
            active = false;
        }

        public bool CheckAddCard(Card cardToAdd, List<Card> currentSleight)
        {
            // Pour laisser des possibilités défensives aux ennemis on ne rajoute pas de carte zero
            if (cardToAdd.GetCardValue() == 0)
                return false;

            // On ne peut pas rajouter de carte, la sleight est déjà complète
            int indexRecipe = currentSleight.Count;
            if (indexRecipe == 3)
                return false;

            if(cardToAdd.CardData == Sleight.SleightRecipe[indexRecipe])
            {
                return true;
            }
            return false;
        }
    }
}
