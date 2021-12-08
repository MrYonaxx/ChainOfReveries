using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace VoiceActing
{

    // on peut pas serializer des classe generic donc on copie colle pour chaque type de carte 
    [System.Serializable]
    //public class EncounterProba<T>
    public class EncounterProba
    {
        [HorizontalGroup]
        [HideLabel]
        public EncounterData encounter;
        [HorizontalGroup]
        public int probability = 100;
    }

    class CheckInitialize
    {
        private bool isInitialized = false;
        public bool IsInitialized
        {
            get { return isInitialized; }
            set { isInitialized = value; }
        }

    }


    [CreateAssetMenu(fileName = "EncounterDatabase", menuName = "CardDatabase/EncounterDatabase", order = 1)]
    public class EncounterDatabase : ScriptableObject
    {

        [SerializeField]
        int levelEncounter = 1;
        [SerializeField]
        int levelUpCount = 3;
        [SerializeField]
        EncounterDatabase nextLevelEncounter = null;

        [Title("Encounters")]
        [SerializeField]
        List<EncounterProba> encounterDatabase = new List<EncounterProba>();

  

        int battleCount = 0;



        List<EncounterProba> encounterAvailable = new List<EncounterProba>();
        List<EncounterProba> encounterDone = new List<EncounterProba>();

        // Hack pour l'editeur pour auto reset l'objet
        CheckInitialize checkInitialize = null;

        // ! \ Ce truc est jamais appelé si on refait une run donc c'est pas bon, à refaire
        public void Reset()
        {
            battleCount = 0;
            encounterDone.Clear();
            encounterAvailable = new List<EncounterProba>(encounterDatabase.Count);
            for (int i = 0; i < encounterDatabase.Count; i++)
            {
                encounterAvailable.Add(encounterDatabase[i]);
            }
        }


        // On ne peut pas repiocher le même combat
        public EncounterData GachaEncounter(int floor)
        {
            if (checkInitialize == null)
            {
                checkInitialize = new CheckInitialize();
                Reset();
                Debug.Log("I reset");
            }

            // Si on fait suffisament de combat, on pioche au level au dessus
            if (battleCount > levelUpCount)
            {
                return nextLevelEncounter.GachaEncounter(floor);
            }

            // Si on est à un étage supérieur au levelEncounter, on pioche dans le level d'au dessus
            if(levelEncounter < floor)
            {
                return nextLevelEncounter.GachaEncounter(floor);
            }



            if (encounterAvailable.Count == 0) // Si on a épuisé tout les rencontres
                Reset();
            int r = Random.Range(0, CalculateMaxProbability(encounterAvailable));
            int i = 0;
            int sum = encounterAvailable[i].probability;
            while (r >= sum && i < encounterAvailable.Count)
            {
                i += 1;
                sum += encounterAvailable[i].probability;
            }

            EncounterData encounter = encounterAvailable[i].encounter;
            encounterDone.Add(encounterAvailable[i]);
            encounterAvailable.RemoveAt(i);
            battleCount += 1;

            return encounter;
        }

        private int CalculateMaxProbability(List<EncounterProba> encounterPool)
        {
            int sum = 0;
            for (int i = 0; i < encounterPool.Count; i++)
            {
                sum += encounterPool[i].probability;
            }
            return sum;
        }
    }
}
