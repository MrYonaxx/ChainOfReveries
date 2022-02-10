using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing
{
    public class AITask_DeckEquipment : Action
    {
        [SerializeField]
        public SharedAIController aiController;

        [SerializeField]
        List<CardEquipmentData> equipmentDatas;
        [SerializeField]
        float initialWait = 0.1f;
        [SerializeField]
        Vector2 intervalPress = new Vector2(0.1f, 0.2f);

        DeckController deckEquipment;
        bool inDeck = false;
        float t = 0f;
        public override void OnStart()
        {
            base.OnStart();

            aiController.Value.PressButton(aiController.Value.InputPadUp);
            deckEquipment = aiController.Value.Character.CharacterEquipment.DeckEquipmentController;
            t = initialWait;

            // Si il n'y a pas de deck data on utilise celui possédé par le joueur
            if (equipmentDatas == null)
            {
                equipmentDatas.Add(((CardEquipment)deckEquipment.Deck[Random.Range(0, deckEquipment.Deck.Count)]).CardEquipmentData);
            }
            else if (equipmentDatas.Count == 0)
            {
                equipmentDatas.Add(((CardEquipment)deckEquipment.Deck[Random.Range(0, deckEquipment.Deck.Count)]).CardEquipmentData);
            }
        }

        public override TaskStatus OnUpdate()
        {
            t -= Time.deltaTime;
            if (t < 0)
            {
                CardEquipment card = (CardEquipment) deckEquipment.Deck[deckEquipment.currentIndex];
 
                // On check si la carte equipement actuel correspond à une des cartes qu'on veut jouer
                for (int i = 0; i < equipmentDatas.Count; i++)
                {
                    if (equipmentDatas[i] == card.CardEquipmentData)
                    {
                        aiController.Value.PressButton(aiController.Value.InputA);
                        return TaskStatus.Success;
                    }
                }

                // On a pas trouvé la carte on bouge
                deckEquipment.MoveHand(true, false);
                //aiController.Value.PressButton(aiController.Value.InputLB);
                t = Random.Range(intervalPress.x, intervalPress.y);
            }     
            return TaskStatus.Running;
        }

        public override void OnConditionalAbort()
        {
            base.OnConditionalAbort();

            aiController.Value.PressButton(aiController.Value.InputPadUp);
        }
    }
}
