using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing 
{
    public class AITask_SleightDecision : Action
    {
        [SerializeField]
        public SharedAIController sharedAIController;
        //[SerializeField]
        //public SharedSleightData sleightData;

        // Si null on prend la database par défaut
        [SerializeField]
        SleightData[] sleightDatabase = null;

        // On peut pas serializer des Vector2Int là dedans donc je passe par 2 float
        /*[SerializeField]
        int searchRangeMin = -3;
        [SerializeField]
        int searchRangeMax = 3;

        Vector2Int searchRange;*/


        // Rand une sleight
        // Regarde si la carte pour faire une sleight est dans la range
        // Si oui
            // On sauvegarde la sleight
            
        // Sinon on teste la sleight suivante
        // Si toutes les sleight y sont passés on renvois faux

       /* public override void OnStart()
        {
            if (searchRangeMin == searchRangeMax)
                searchRangeMax += 1;
            searchRange = new Vector2Int(searchRangeMin, searchRangeMax);
            base.OnStart();
        }*/

        public override TaskStatus OnUpdate()
        {
            // Fix pour quand l'IA se larde (je ne sais toujours pas pourquoi d'ailleurs)
            if (sharedAIController.Value.Character.SleightController.GetCurrentSleight() == null && 
                sharedAIController.Value.Character.SleightController.CanPlaySleight())
                sharedAIController.Value.PressButton(sharedAIController.Value.InputB);

            if (sharedAIController.Value.SleightData.Active())
                return TaskStatus.Failure;

            if(SleightDecision(sharedAIController.Value.Character))
                return TaskStatus.Success;

            return TaskStatus.Failure;
        }


        public bool SleightDecision(CharacterBase user)
        {
            if (sleightDatabase == null)
                sleightDatabase = user.CharacterData.SleightDatabase;
            else if (sleightDatabase.Length == 0)
                sleightDatabase = user.CharacterData.SleightDatabase;

            if (sleightDatabase == null)
                return false;
            else if (sleightDatabase.Length == 0)
                return false;

            // Sleight sélectionné
            SleightData sleightSelected = sleightDatabase[Random.Range(0, sleightDatabase.Length)];
            sharedAIController.Value.SleightData.Sleight = sleightSelected;
            sharedAIController.Value.SleightData.SetActive();
            return true;
        }





        /*private void SearchCards(List<Card> deck, int initialIndex, Vector2Int range)
        {
            int searchIndex = initialIndex + range.x;
            if (searchIndex < 0)
                searchIndex += deck.Count;
            searchIndex = Mathf.Max(0, searchIndex);

            for (int i = 0; i < (range.y - range.x); i++)
            {
                if (searchIndex >= deck.Count)
                    searchIndex = 0;
                cardsIndex.Add(searchIndex);
                searchIndex += 1;
            }
        }*/



    }
}
