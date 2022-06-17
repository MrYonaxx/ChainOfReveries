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

        [SerializeField] // Couleur de la reverie, dépend des cartes
        int[] reverieColor = null;
        public int[] ReverieColor
        {
            get { return reverieColor; }
            set { reverieColor = value; }
        }


        // Info de la run en cours
        [Space]
        [Title("Current Equipment")]
        [SerializeField]
        List<Card> playerDeck;
        public List<Card> PlayerDeck
        {
            get { return playerDeck; }
            set { playerDeck = value; }
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

            reverieColor = new int[3] { 1, 0, 0 };
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

        // Alors y'a un bug big brain, si on récupère une carte et que la prochaine salle imposé est cette même carte,
        // on va la perdre de manière gratos
        public void AddRoomToLayout(CardExplorationData newCard)
        {
            playerExplorationDeck.Remove(newCard);
            levelLayout[room] = newCard;
            room += 1;
            roomExplored += 1;
        }

        public void RemoveLastRoom(CardExplorationData emptyRoom)
        {
            room -= 1;
            levelLayout[room] = emptyRoom;
        }

        public void NextZone()
        {
            floor += 1;
            SetLayout(floorLayout.GetFloor(floor).FloorLayout);
        }


        public void AddCard(Card c)
        {
            AddCard(playerDeck, c);
           /* bool addCard = false;
            bool addCategory = false;

            // On ajoute les cartes 0 tout à la fin
            if (c.baseCardValue == 0)
            {
                // on cherche les cartes zero, opti en parcourant à l'envers puisque généralement on ajoute en fin de deck les zéro
                for (int i = playerDeck.Count-1; i >= 0; i--)
                {
                    if (c.CardData == playerDeck[i].CardData && playerDeck[i].baseCardValue == 0)
                    {
                        playerDeck.Insert(i, c);
                        return;
                    }
                }
                // Si on arrive là on a rien trouvé donc on ajoute le zero à la fin
                playerDeck.Add(c);
                return;
            }

            // On parcoure le deck à la recherche d'une carte similaire
            for (int i = 0; i < playerDeck.Count; i++)
            {
                // On a détecté qu'on possédé déjà cette carte dans le deck
                if (c.CardData == playerDeck[i].CardData && playerDeck[i].baseCardValue != 0)
                {
                    addCard = true;
                    if (playerDeck[i].baseCardValue < c.baseCardValue)
                    {
                        playerDeck.Insert(i, c);
                        return;
                    }
                }
                else if (addCard)
                {
                    playerDeck.Insert(i, c);
                    return;
                }
            }

            // Si on a pas trouvé la carte dans le deck, on cherche cette fois la categorie à laquelle la carte appartient
            // et on ajoute la carte à la suite de cette catégorie
            for (int i = 0; i < playerDeck.Count; i++)
            {
                // On a détecté qu'on possède déjà cette catégorie de carte (attack / magie) dans le deck
                if (c.CardData.CardType == playerDeck[i].CardData.CardType)
                {
                    addCategory = true;
                }
                else if (addCategory)
                {
                    playerDeck.Insert(i, c);
                    return;
                }
            }

            // Et si vraiment on arrive là on ajoute simplement la carte
            playerDeck.Add(c);*/
        }

        public void AddCard(List<Card> deck, Card c)
        {
            bool addCard = false;
            bool addCategory = false;

            // On ajoute les cartes 0 tout à la fin
            if (c.baseCardValue == 0)
            {
                // on cherche les cartes zero, opti en parcourant à l'envers puisque généralement on ajoute en fin de deck les zéro
                for (int i = deck.Count - 1; i >= 0; i--)
                {
                    if (c.CardData == deck[i].CardData && deck[i].baseCardValue == 0)
                    {
                        deck.Insert(i, c);
                        return;
                    }
                }
                // Si on arrive là on a rien trouvé donc on ajoute le zero à la fin
                deck.Add(c);
                return;
            }

            // On parcoure le deck à la recherche d'une carte similaire
            for (int i = 0; i < deck.Count; i++)
            {
                // On a détecté qu'on possédé déjà cette carte dans le deck
                if (c.CardData == deck[i].CardData && deck[i].baseCardValue != 0)
                {
                    addCard = true;
                    if (deck[i].baseCardValue < c.baseCardValue)
                    {
                        deck.Insert(i, c);
                        return;
                    }
                }
                else if (addCard)
                {
                    deck.Insert(i, c);
                    return;
                }
            }

            // Si on a pas trouvé la carte dans le deck, on cherche cette fois la categorie à laquelle la carte appartient
            // et on ajoute la carte à la suite de cette catégorie
            for (int i = 0; i < deck.Count; i++)
            {
                // On a détecté qu'on possède déjà cette catégorie de carte (attack / magie) dans le deck
                if (c.CardData.CardType == deck[i].CardData.CardType)
                {
                    addCategory = true;
                }
                else if (addCategory)
                {
                    deck.Insert(i, c);
                    return;
                }
            }

            // Et si vraiment on arrive là on ajoute simplement la carte
            deck.Add(c);
        }


        // Sort du sheitan, pardon les cours d'algo
        public void SortDeck(List<Card> category)
        {
            SortDeck(ref playerDeck, category);
            /*int index = 0;
            bool categoryFound = false; // petite opti pour tenter de parcourir le deck le moins de fois possible
            List<Card> sortedList = new List<Card>(playerDeck.Count);


            for (int j = 0; j < category.Count; j++)
            {
                categoryFound = false;
                for (int i = 0; i < playerDeck.Count; i++)
                {
                    if (category[j].baseCardValue == 0)
                    {
                        if (categoryFound && (playerDeck[i].CardData != category[j].CardData || playerDeck[i].baseCardValue != 0))
                        {
                            break;
                        }
                        if (playerDeck[i].CardData == category[j].CardData && playerDeck[i].baseCardValue == 0)
                        {
                            categoryFound = true;
                            sortedList.Add(playerDeck[i]);
                        }
                    }
                    else
                    {
                        if (categoryFound && (playerDeck[i].CardData != category[j].CardData || playerDeck[i].baseCardValue == 0))
                        {
                            break;
                        }
                        if (playerDeck[i].CardData == category[j].CardData && playerDeck[i].baseCardValue != 0)
                        {
                            categoryFound = true;
                            sortedList.Add(playerDeck[i]);
                        }
                    }
                }
            }

            playerDeck = sortedList;*/

        }

        public void SortDeck(ref List<Card> deck, List<Card> category)
        {
            bool categoryFound = false; // petite opti pour tenter de parcourir le deck le moins de fois possible
            List<Card> sortedList = new List<Card>(deck.Count);


            for (int j = 0; j < category.Count; j++)
            {
                categoryFound = false;
                for (int i = 0; i < deck.Count; i++)
                {
                    if (category[j].baseCardValue == 0)
                    {
                        if (categoryFound && (deck[i].CardData != category[j].CardData || deck[i].baseCardValue != 0))
                        {
                            break;
                        }
                        if (deck[i].CardData == category[j].CardData && deck[i].baseCardValue == 0)
                        {
                            categoryFound = true;
                            sortedList.Add(deck[i]);
                        }
                    }
                    else
                    {
                        if (categoryFound && (deck[i].CardData != category[j].CardData || deck[i].baseCardValue == 0))
                        {
                            break;
                        }
                        if (deck[i].CardData == category[j].CardData && deck[i].baseCardValue != 0)
                        {
                            categoryFound = true;
                            sortedList.Add(deck[i]);
                        }
                    }
                }
            }
            deck = sortedList;
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

        public void AddReverieColor(int color, int amount)
        {
            reverieColor[color] += amount;
        }

        /// <summary>
        /// Red = 0, Blue = 1, Vert = 2
        /// </summary>
        /// <returns></returns>
        public int GetReverieColor()
        {
            int bestIndex = 0;
            int bestValue = 0;
            for (int i = 0; i < reverieColor.Length; i++)
            {
                if(reverieColor[i]>=bestValue)
                {
                    bestValue = reverieColor[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace