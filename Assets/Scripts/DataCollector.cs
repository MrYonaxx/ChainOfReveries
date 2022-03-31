using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    // Collecte des données de combat, principalement utilisé par le Menu Game Over 
    public class DataCollector : MonoBehaviour
    {

        [SerializeField]
        CardBreakController cardBreakController = null;

        [HideInInspector]
        public int CardBreakTimes;
        [HideInInspector]
        public int EnemyDefeated;
        [HideInInspector]
        public float Timer;

        CharacterBase character = null;
        List<CardData> sleightData = new List<CardData>();
        List<int> sleightUsage = new List<int>();



        public void Initialize(CharacterBase newCharacter)
        {
            character = newCharacter;
            cardBreakController.OnCardBreak += CountCardBreak;
            character.CharacterAction.OnSleightPlayed += CountSleight;
            character.CharacterAction.OnAttackHit += CountKill;
        }

        void OnDestroy()
        {
            cardBreakController.OnCardBreak -= CountCardBreak;
            character.CharacterAction.OnSleightPlayed -= CountSleight;
            character.CharacterAction.OnAttackHit -= CountKill;
        }

        // Update is called once per frame
        void Update()
        {
            Timer += Time.deltaTime;
        }

        void CountSleight(AttackManager attack, Card sleight)
        {
            int i = sleightData.IndexOf(sleight.CardData);
            if (i < 0) 
            {
                sleightData.Add(sleight.CardData);
                sleightUsage.Add(1);
            }
            else
            {
                sleightUsage[i]++;
            }
        }

        void CountKill(AttackController attack, CharacterBase target)
        {
            if(target.CharacterKnockback.IsDead)
            {
                //EnemyDefeated++;
            }
        }

        void CountCardBreak(CharacterBase characterBreaked, List<Card> cardBreaked, CharacterBase characterBreaker, List<Card> cardBreaker)
        {
            if(characterBreaker == character)
            {
                CardBreakTimes++;
            }
        }



        public CardData GetMostUsedSleight()
        {
            int max = 0;
            int maxIndex = -1;

            for (int i = 0; i < sleightUsage.Count; i++)
            {
                if(sleightUsage[i] >= max)
                {
                    maxIndex = i;
                    max = sleightUsage[i];
                }
            }

            if (maxIndex == -1)
                return null;
            return sleightData[maxIndex];
        }
    }
}
