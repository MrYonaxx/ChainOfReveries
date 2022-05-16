using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing
{
    public class AITask_MoveDeck : Action
    {
        [SerializeField]
        public SharedAIController aiController;
        [SerializeField]
        public SharedInt cardIndex;


        [SerializeField]
        Vector2 intervalPress = new Vector2(0.1f, 0.2f);

        [SerializeField]
        bool checkSleight = false;
        //[SerializeField]
        //public SharedSleightData sleightData;

        DeckController deckController;
        int direction = 0;
        float t = 0f;

        public override void OnStart()
        {
            deckController = aiController.Value.Character.DeckController;

            int leftPath = deckController.CurrentIndex - cardIndex.Value;
            if (leftPath < 0)
                leftPath += deckController.Deck.Count;
            int rightPath = cardIndex.Value - deckController.CurrentIndex;
            if (rightPath < 0)
                rightPath += deckController.Deck.Count;

            if (leftPath < rightPath)
            {
                direction = -leftPath;
            }
            else
            {
                direction = rightPath;
            }
        }

        public override TaskStatus OnUpdate()
        {          
            // Si out of range on dégage
            if (cardIndex.Value >= deckController.Deck.Count)
                return TaskStatus.Failure;

            if (deckController.CurrentIndex == cardIndex.Value)
                return TaskStatus.Success;

            if (t < 0)
            {
                if (direction < 0)
                {
                    direction += 1;
                    deckController.MoveHand(true, false);
                }
                else if (direction > 0)
                {
                    direction -= 1;
                    deckController.MoveHand(false, true);             
                }
                else // Si on est là c'est pas normal et je me suis perdu dans mes calculs
                {
                    deckController.MoveHand(false, true);
                }
                CheckAddSleight(aiController.Value.Character);
                t = Random.Range(intervalPress.x, intervalPress.y);
            }
            else
            {
                t -= Time.deltaTime;
            }
            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            // Reset buffer 
            aiController.Value.ReleaseButton(aiController.Value.InputY);
        }

        private void CheckAddSleight(CharacterBase character)
        {
            aiController.Value.ResetButtonBuffer(aiController.Value.InputY);
            if (deckController.CurrentIndex == cardIndex.Value || checkSleight == false)
                return;

            // On vérifie qu'une sleight a bien été choisi
            if (!aiController.Value.SleightData.Active())
                return;

            // On check si on peut ajouter la carte
            if (aiController.Value.SleightData.CheckAddCard(character.DeckController.GetCurrentCard(), character.SleightController.GetSleightCards()))
            {
                aiController.Value.PressButton(aiController.Value.InputY);
            }

        }
    }
}
