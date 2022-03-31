using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using UnityEditor;

namespace VoiceActing
{
    // on peut pas serializer des classe generic donc on copie colle pour chaque type de carte 
    [System.Serializable]
    public class CardEquipmentProba
    {
        [HorizontalGroup]
        [HideLabel]
        public CardEquipmentData cardEquipment = null;
        [HorizontalGroup]
        public int probability = 100;
    }

    [CreateAssetMenu(fileName = "CardEquipmentDatabase", menuName = "CardDatabase/CardEquipmentDatabase", order = 1)]
    public class CardEquipmentDatabase : ScriptableObject
    {

        [SerializeField]
        [ReadOnly]
        private int maxProbability = 0;

        [SerializeField]
        [OnValueChanged("CalculateMaxProbability", true)]
        List<CardEquipmentProba> equipmentDatabase;


        public CardEquipmentData GachaEquipment()
        {
            int r = Random.Range(0, maxProbability);
            int i = 0;
            int sum = equipmentDatabase[i].probability;
            while (r >= sum && i < equipmentDatabase.Count)
            {
                i += 1;
                sum += equipmentDatabase[i].probability;
            }
            return equipmentDatabase[i].cardEquipment;
        }

        private void CalculateMaxProbability()
        {
            int sum = 0;
            for (int i = 0; i < equipmentDatabase.Count; i++)
            {
                sum += equipmentDatabase[i].probability;
            }
            maxProbability = sum;
        }

        public List<CardEquipmentData> CopyCardDatabase(bool all = true)
        {
            List<CardEquipmentData> res = new List<CardEquipmentData>(equipmentDatabase.Count);
            for (int i = 0; i < equipmentDatabase.Count; i++)
            {
                if (!all && equipmentDatabase[i].probability == 0)
                    continue;
                res.Add(equipmentDatabase[i].cardEquipment);
            }
            return res;
        }

        public List<Card> CopyCardDatabaseListCard()
        {
            List<Card> res = new List<Card>(equipmentDatabase.Count);
            for (int i = 0; i < equipmentDatabase.Count; i++)
            {
                res.Add(new CardEquipment(equipmentDatabase[i].cardEquipment));
            }
            return res;
        }


        [Title("Editor")]
        [SerializeField]
        [FolderPath]
        string pathDatabase;

#if UNITY_EDITOR
        [Button]
        private void AutoAddNewEntry()
        {
            string[] files = Directory.GetFiles(pathDatabase, "*.asset", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                CardEquipmentData equipment = (CardEquipmentData)AssetDatabase.LoadAssetAtPath(file, typeof(CardEquipmentData));
                if (!Contains(equipment.name))
                {            
                    equipmentDatabase.Add(new CardEquipmentProba());
                    equipmentDatabase[equipmentDatabase.Count - 1].cardEquipment = equipment;
                    equipmentDatabase[equipmentDatabase.Count - 1].probability = 100;
                }
            }
            CalculateMaxProbability();
            EditorUtility.SetDirty(this);
        }
#endif

        // Un jour avec un script editor faire des database avec des dictionnaires 
        private bool Contains(string data)
        {
            for (int i = 0; i < equipmentDatabase.Count; i++)
            {
                if (equipmentDatabase[i].cardEquipment.name.Equals(data))
                    return true;
            }
            return false;
        }

    }
}
