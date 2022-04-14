using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// Settings qui n'utilise pas le player Pref
public static class GameSettings
{
    public static int DeckLayout = 0;
    public static Color BackgroundAttackCard = Color.white;
    public static Color BackgroundMagicCard = Color.white;
}

// Tout ce qu'on doit sauvegarder est là dedans
[CreateAssetMenu(fileName = "GameData", menuName = "GameSystem/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [SerializeField]
    public bool FirstTime = true;
    [SerializeField]
    public float Timer = 0;
    [SerializeField]
    public int NbRun = 0;
    [SerializeField]
    public int NbRunCompleted = 0;
    [SerializeField]
    public int MaxReverieLevel = 0;


    [SerializeField]
    public int[] NbRunCharacters;

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


    [Header("Game Settings")]
    [SerializeField]
    int deckLayout = 0;
    [SerializeField]
    Color backgroundAttackCard = Color.white;
    [SerializeField]
    Color backgroundMagicCard = Color.white;


    string saveFileName = "save";

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

    public void ResetSave()
    {
        FirstTime = true;
        Timer = 0;
        NbRun = 0;
        NbRunCompleted = 0;
        MaxReverieLevel = 0;
        NbRunCharacters = new int[]{ 0, 0, 0, 0, 0 };
    }

    public void Save()
    {
        AddTimer(); // (j'ajoute le temps pour la save puis je l'enlève une fois enregistré pour ne pas qu'on augmente le timer à chaque fois qu'on sauvegarde)

        // Save les GameSettings
        deckLayout = GameSettings.DeckLayout;
        backgroundAttackCard = GameSettings.BackgroundAttackCard;
        backgroundMagicCard = GameSettings.BackgroundMagicCard;


        string json = JsonUtility.ToJson(this);
        string filePath = string.Format("{0}/saves/{1}{2}.json", Application.persistentDataPath, saveFileName, 0);
        Debug.Log("SAVE : " + filePath);
        FileInfo fileInfo = new FileInfo(filePath);

        if (!fileInfo.Directory.Exists)
        {
            Directory.CreateDirectory(fileInfo.Directory.FullName);
        }
        File.WriteAllText(filePath, json);

        SubstractTimer();
    }

    public bool Load()
    {
        string filePath = string.Format("{0}/saves/{1}{2}.json", Application.persistentDataPath, saveFileName, 0);
        Debug.Log("LOAD : " + filePath);
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(dataAsJson, this);

            // Load les game settings
            GameSettings.DeckLayout = deckLayout;
            GameSettings.BackgroundAttackCard = backgroundAttackCard;
            GameSettings.BackgroundMagicCard = backgroundMagicCard;

            return true;
        }
        return false;
    }

    public void AddTimer()
    {
        Timer += Time.time;
    }
    public void SubstractTimer()
    {
        Timer -= Time.time;
    }
}
