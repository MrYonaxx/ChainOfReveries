using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData", order = 1)]
public class GameData : ScriptableObject
{
    [SerializeField]
    int nbRun = 0;
    [SerializeField]
    int nbRunCompleted = 0;

    [SerializeField]
    List<int> charactersUnlocked;


    [SerializeField]
    List<int> customDecks;
    // int characterID
    // List string cardID
    // List int cardValue
    // List string cardEquipmentID

    [SerializeField]
    List<int> inputConfiguration;

    // Versus Mode
    [SerializeField]
    int player1ID;
    [SerializeField]
    int player2ID;

    public void Save()
    {

    }

    public void Load()
    {

    }
}
