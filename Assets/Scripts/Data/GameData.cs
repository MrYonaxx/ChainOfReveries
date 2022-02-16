using System.Collections;
using System.Collections.Generic;
using UnityEngine;





// Tout ce qu'on doit sauvegarder est là dedans
[CreateAssetMenu(fileName = "GameData", menuName = "GameSystem/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [SerializeField]
    public bool FirstTime = true;
    [SerializeField]
    public int NbRun = 0;
    [SerializeField]
    public int NbRunCompleted = 0;

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

    public void SetControllerID(int playerID, int controllerID)
    {
        if (playerID == 1)
            P1ControllerID = controllerID;
        else
            P2ControllerID = controllerID;
    }

    public void Save()
    {

    }

    public void Load()
    {

    }
}
