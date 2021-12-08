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
        }

        public override TaskStatus OnUpdate()
        {
            t -= Time.deltaTime;
            if (t < 0)
            {
                CardEquipment card = (CardEquipment)deckEquipment.Deck[deckEquipment.currentIndex];
                for (int i = 0; i < equipmentDatas.Count; i++)
                {
                    if (equipmentDatas[i] == card.CardEquipmentData)
                    {
                        Debug.Log("Ouais");
                        aiController.Value.PressButton(aiController.Value.InputA);
                        return TaskStatus.Success;
                    }
                }
     
                // On a pas trouvé la carte on bouge
                aiController.Value.PressButton(aiController.Value.InputRB);
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
