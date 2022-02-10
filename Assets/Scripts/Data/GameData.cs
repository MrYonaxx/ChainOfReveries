using System.Collections;
using System.Collections.Generic;
using UnityEngine;





// Tout ce qu'on doit sauvegarder est là dedans
[CreateAssetMenu(fileName = "GameData", menuName = "GameSystem/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [SerializeField]
    bool firstTime = true;
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


    [Header("Player Config")]
    [SerializeField]
    int P1ControllerID;
    [SerializeField]
    VoiceActing.InputConfig P1InputConfig;

    [SerializeField]
    int P2ControllerID;
    [SerializeField]
    VoiceActing.InputConfig P2InputConfig;

    public int GetControllerID(int playerID)
    {
        if (playerID == 1)
            return P1ControllerID;
        else
            return P2ControllerID;
    }
    public VoiceActing.InputConfig GetInputConfig(int playerID)
    {
        if (playerID <= 1)
            return P1InputConfig;
        else
            return P2InputConfig;
    }

    public void Save()
    {

    }

    public void Load()
    {

    }
}
