using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    // on peut pas serializer des classe generic donc on copie colle pour chaque type de carte 
    [System.Serializable]
    public class CardExplorationProba
    {
        [HorizontalGroup]
        [HideLabel]
        public CardExplorationData cardExploration = null;
        [HorizontalGroup]
        public int probability = 100;
    }


    [CreateAssetMenu(fileName = "CardExplorationDatabase", menuName = "CardDatabase/CardExplorationDatabase", order = 1)]
    public class CardExplorationDatabase : ScriptableObject
    {

        [Title("Parameter")]
        [SerializeField]
        [ValueDropdown("SelectCardType")]
        int TriggerForceDrawType;

        [SerializeField]
        int numberToTriggerForceDraw = 2;

        [SerializeField]
        [ValueDropdown("SelectCardType")]
        int forceDrawType;


        [Title("Gacha")]
        [SerializeField]
        List<CardExplorationProba> explorationDatabase;

        // A reset en début de run
        int forceDrawCount = 0;


        public void Reset()
        {
            forceDrawCount = 0;
        }

        // Si on pioche trop de carte explorations soutien, on augmente la proba des carte exploration combat
        public CardExplorationData GachaExploration()
        {
            int r = Random.Range(0, CalculateMaxProbability(explorationDatabase));
            int i = 0;
            int sum = explorationDatabase[i].probability;
            while (r >= sum && i < explorationDatabase.Count)
            {
                i += 1;
                sum += explorationDatabase[i].probability;
            }
            /*if(explorationDatabase[i].cardExploration.CardType == TriggerForceDrawType)
            {
                forceDrawCount += 1;
                if(forceDrawCount == numberToTriggerForceDraw)
                    return GachaExploration() 
            }
            else
            {
                forceDrawCount = 0;
            }*/

            return explorationDatabase[i].cardExploration;
        }
        private int CalculateMaxProbability(List<CardExplorationProba> cardPool)
        {
            int sum = 0;
            for (int i = 0; i < cardPool.Count; i++)
            {
                sum += cardPool[i].probability;
            }
            return sum;
        }

        // Pour les run "variantes"
        /* public CardExplorationData GachaExploration(CardCondition )
         {

         }*/

        public List<CardExplorationData> CopyCardDatabase()
        {
            List<CardExplorationData> res = new List<CardExplorationData>(explorationDatabase.Count);
            for (int i = 0; i < explorationDatabase.Count; i++)
            {
                res.Add(explorationDatabase[i].cardExploration);
            }
            return res;
        }



#if UNITY_EDITOR
        private static IEnumerable SelectCardType()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("CardTypesDataExploration")[0]))
                .GetAllTypeID();
        }
#endif 

    }
}
