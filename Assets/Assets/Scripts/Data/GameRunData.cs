/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    // Data de la run actuelle
    // C'est dans un SO pour gérer les transitions entre les scènes et garder le même inventaire
    [CreateAssetMenu(fileName = "GameData", menuName = "GameSystem/GameRunData", order = 1)]
    public class GameRunData : ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        private int characterID = 0;
        public int CharacterID
        {
            get { return characterID; }
            set { characterID = value; }
        }

        // Info pour setup la run
        [Title("Players Info")]
        [SerializeField]
        private PlayerData playerCharacterData = null;
        public PlayerData PlayerCharacterData
        {
            get { return playerCharacterData; }
            set { playerCharacterData = value; }
        }
        [SerializeField]
        private DeckData playerDeckData = null;
        public DeckData PlayerDeckData
        {
            get { return playerDeckData; }
            set { playerDeckData = value; }
        }

        /// <summary>
        /// Data pour setup la run
        /// </summary>
        [SerializeField]
        private List<CardEquipmentData> playerEquipmentData = new List<CardEquipmentData>();
        public List<CardEquipmentData> PlayerEquipmentData
        {
            get { return playerEquipmentData; }
            set { playerEquipmentData = value; }
        }

        /// <summary>
        /// Data pour setup la run
        /// </summary>
        [SerializeField]
        private List<CardExplorationData> playerExplorationData = new List<CardExplorationData>();
        public List<CardExplorationData> PlayerExplorationData
        {
            get { return playerExplorationData; }
            set { playerExplorationData = value; }
        }

        // Le floor layout initial de la run
        [SerializeField]
        private FloorLayoutData floorLayout = null;
        public FloorLayoutData FloorLayout // Retourne le floor layout de l'étage actuel
        {
            get { return floorLayout.GetFloor(floor); }
        }

        [SerializeField] // Difficulty Level
        int reverieLevel = 0;
        public int ReverieLevel
        {
            get { return reverieLevel; }
            set { reverieLevel = value; }
        }


        // Info de la run en cours
        [Space]
        [Title("Current Equipment")]
        [SerializeField]
        List<Card> playerDeck;
        public List<Card> PlayerDeck
        {
            get { return playerDeck; }
        }

        [SerializeField]
        List<CardExplorationData> playerExplorationDeck = new List<CardExplorationData>();
        public List<CardExplorationData> PlayerExplorationDeck
        {
            get { return playerExplorationDeck; }
        }

        [SerializeField]
        List<CardEquipment> playerEquipmentDeck = new List<CardEquipment>();
        public List<CardEquipment> PlayerEquipmentDeck
        {
            get { return playerEquipmentDeck; }
        }


        [Space]
        [Title("Current Progression")]
        [SerializeField]
        int floor = 0;
        public int Floor
        {
            get { return floor; }
        }

        [SerializeField]
        int room;
        public int Room
        {
            get { return room; }
        }

        int roomExplored;
        public int RoomExplored
        {
            get { return roomExplored; }
        }
        public int killCount;
        public int KillCount
        {
            get { return killCount; }
            set { killCount = value; }
        }

        [SerializeField]
        List<CardExplorationData> levelLayout = new List<CardExplorationData>();
        public List<CardExplorationData> LevelLayout
        {
            get { return levelLayout; }
        }



        [SerializeField]
        [ReadOnly]
        List<BattleModifiers> battleModifiers;
        public List<BattleModifiers> BattleModifiers
        {
            get { return battleModifiers; }
        }

        int weaponCardCount = 0;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public void CreateRunData()
        {
            CreateRunData(playerCharacterData);

            battleModifiers = new List<BattleModifiers>();
        }

        public void CreateRunData(PlayerData playerInitialData)
        {
            playerCharacterData = playerInitialData;

            playerDeck.Clear();
            playerDeck = playerDeckData.CreateDeck();

            // Setup le deck exploration
            playerExplorationDeck.Clear();
            for (int i = 0; i < playerExplorationData.Count; i++)
            {
                playerExplorationDeck.Add(playerExplorationData[i]);
            }

            weaponCardCount = 0;
            // Setup le deck equipment
            playerEquipmentDeck.Clear();
            for (int i = 0; i < playerEquipmentData.Count; i++)
            {
                playerEquipmentDeck.Add(new CardEquipment(playerEquipmentData[i]));
                if (playerEquipmentData[i].CardType == 0)
                    weaponCardCount += 1;
            }

            roomExplored = 0;
            killCount = 0;
            floor = floorLayout.FloorLevel;
            SetLayout(floorLayout.FloorLayout);
        }

        public void SetLayout(List<CardExplorationData> initialLevelLayout)
        {
            room = 0;
            levelLayout.Clear();
            for (int i = 0; i < initialLevelLayout.Count; i++)
            {
                levelLayout.Add(initialLevelLayout[i]);
            }
        }

        public void AddRoomToLayout(CardExplorationData newCard)
        {
            playerExplorationDeck.Remove(newCard);
            levelLayout[room] = newCard;
            room += 1;
            roomExplored += 1;
        }

        public void NextZone()
        {
            floor += 1;
            SetLayout(floorLayout.GetFloor(floor).FloorLayout);
        }


        public void AddCard(Card c)
        {
            // On ajoute les cartes 0 tout à la fin
            if (c.baseCardValue == 0)
            {
                playerDeck.Add(c);
                return;
            }

            bool addCard = false;
            bool addCategory = false;
            for (int i = 0; i < playerDeck.Count; i++)
            {
                // On a détecté qu'on possédé déjà cette carte dans le deck
                if (addCard == false && c.CardData == playerDeck[i].CardData)
                {
                    addCard = true;
                }
                // On a détecté qu'on possède déjà cette catégorie de carte (attack / magie) dans le deck
                else if (addCategory == false && c.CardData.CardType == playerDeck[i].CardData.CardType)
                {
                    addCategory = true;
                }

                // On ajoute à la suite des cartes et avec une valeur décroissante
                if (addCard && playerDeck[i].CardData != c.CardData)
                {
                    playerDeck.Insert(i, c);
                    return;
                }
                else if (addCard && playerDeck[i].baseCardValue < c.baseCardValue) 
                {
                    playerDeck.Insert(i, c);
                    return;
                }



                if (addCategory && playerDeck[i].CardData.CardType != c.CardData.CardType)
                {
                    playerDeck.Insert(i, c);
                    return;
                }
            }
            playerDeck.Add(c);
        }

        public void AddExplorationCard(CardExplorationData c)
        {
            playerExplorationDeck.Add(c);
            playerExplorationDeck.Sort(SortExplorationCard);
        }

        public bool AddEquipmentCard(CardEquipment c)
        {
            if(c.CardEquipmentData.CardType == 0)
            {
                if (weaponCardCount >= 4)
                    return false;
                weaponCardCount++;
            }

            playerEquipmentDeck.Add(c);
            return true;
        }
        public void RemoveEquipmentCard(CardEquipmentData c)
        {
            for (int i = 0; i < playerEquipmentDeck.Count; i++)
            {
                if (playerEquipmentDeck[i].CardEquipmentData == c)
                    playerEquipmentDeck.RemoveAt(i);
            }
        }



        // Battle Modifiers
        public void AddBattleModifiers(CardExplorationData data)
        {
            for (int i = 0; i < data.BattleModifiers.Count; i++)
            {
                // créer une copie des battle modifiers de la carte
                BattleModifiers bm = new BattleModifiers(data.BattleModifiers[i].statusEffect,
                                                         data.BattleModifiers[i].nb,
                                                         data.BattleModifiers[i].label,
                                                         data.BattleModifiers[i].battleModifierTargets);
                battleModifiers.Add(bm);
            }
        }


        // Appelé à chaque fois qu'on retourne au Menu Exploration
        public void UpdateBattleModifiers()
        {
            for (int i = battleModifiers.Count - 1; i >= 0; i--)
            {
                battleModifiers[i].nb -= 1;
                if (battleModifiers[i].nb <= 0)
                    battleModifiers.RemoveAt(i);
            }
        }


        // Sort par type
        private int SortExplorationCard(CardExplorationData a, CardExplorationData b)
        {
            if (a.CardType < b.CardType)
                return -1;
            else
                return 1;
        }



        #endregion

    } 

} // #PROJECTNAME# namespace