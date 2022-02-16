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
        /*[SerializeField]
        float initialWait = 0.1f;
        [SerializeField]
        Vector2 intervalPress = new Vector2(0.1f, 0.2f);

        DeckController deckEquipment;
        bool inDeck = false;
        float t = 0f;*/
        public override void OnStart()
        {
            base.OnStart();

            CharacterEquipment equipment = aiController.Value.Character.CharacterEquipment;

            for (int i = 0; i < equipment.CardsEquipmentWeapon.Length; i++)
            {
                for (int j = 0; j < equipmentDatas.Count; j++)
                {
                    if (equipment.CardsEquipmentWeapon[i] == null)
                        continue;

                    if (equipment.CardsEquipmentWeapon[i].CardEquipmentData == equipmentDatas[j])
                    {
                        switch(i+1)
                        {
                            // Dpad 2 = down, 4 = left, 6 = right, 8 = up
                            case 1:
                                aiController.Value.PressButton(aiController.Value.InputPadDown);
                                break;
                            case 2:
                                aiController.Value.PressButton(aiController.Value.InputPadLeft);
                                break;
                            case 3:
                                aiController.Value.PressButton(aiController.Value.InputPadRight);
                                break;
                            case 4:
                                aiController.Value.PressButton(aiController.Value.InputPadUp);
                                break;
                        }
                        return;
                    }
                }
            }
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;

            /*t -= Time.deltaTime;
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
            return TaskStatus.Running;*/
        }

    }
}
