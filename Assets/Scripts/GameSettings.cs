using System;
using System.Linq;
using UnityEngine;

public enum CreatureType
{
    NONE = 0,
    CHICKEN = 1,
    CHICKEN2 = 2,
    DUCK = 4,
    TURTLE = 8,
    OSTRICH = 16,
    PENGUIN = 32,
    DINO = 64,
    ALIEN = 128,
    FROG = 256,
    LIZZARD = 512,
    PLATIPUS = 1024,
    ECHIDNA = 2048,
    SNAKE = 4096,
    SNAIL = 8192,
    OYSTER = 16384,
    BUTTERFLY = 32768
}

[Serializable]
public struct Creature
{
    public string name;
    public int reward;
    public int price;
    public GameObject egg;
    public GameObject parent;
    public GameObject inShop;
}

[Serializable]
public struct SoundData
{
    public string name;
    public AudioClip sound;
}

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    #region Fields

    [SerializeField] Creature activeCreature;

    public Creature[] Creatures;

    [SerializeField] SoundData[] sounds;

    public event Action<int> OnGoldChanged;
    public event Action<Creature> OnActiveCreatureChanged;
    public event Action<CreatureType> OnCreaturePurchased;

    private int purchasedCreatures = (int)CreatureType.CHICKEN;

    private int gold;
    private int highScore;
    private int rateUsShown;

    #endregion

    #region Properties

    public int HighScore => highScore;

    public bool RateUsShown => rateUsShown > 0;

    public int PurchasedCreatures => purchasedCreatures;

    public int Gold { get => gold; set { gold = value; OnGoldChanged?.Invoke(value); } }

    public GameObject GetEggPrefab() => activeCreature.egg;

    public GameObject GetActiveCreaturePrefab() => activeCreature.parent;

    public int GetCreatureReward() => activeCreature.reward;

    public Creature GetCreature(string name) //=> Creatures.First(data => data.name == name);
    {
        for (var i = 0; i < Creatures.Length; i++)
        {
            if (Creatures[i].name == name)
            {
                return Creatures[i];
            }
        }
        return default;
    }

    public bool IsPurchased(int creatureID) => (purchasedCreatures & creatureID) > 0;

    public AudioClip GetSound(string name) //=> sounds.First(data => data.name == name).sound;
    {
        for (var i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                return sounds[i].sound;
            }
        }
        return null;
    }

    #endregion

    public void AddToPurchased(string name)
    {
        Enum.TryParse(name.ToUpper(), out CreatureType creatureID);
        purchasedCreatures |= (int)creatureID;
        OnCreaturePurchased?.Invoke(creatureID);
    }

    public bool IsPurchased(string name)
    {
        Enum.TryParse(name.ToUpper(), out CreatureType creatureID);
        return IsPurchased((int)creatureID);
    }

    public void SetPurchasedCreatures(int data)
    {
        if (data == (int)CreatureType.NONE)
            data = (int)CreatureType.CHICKEN;

        purchasedCreatures = data;
    }

    public void SetActiveCreature(string name)
    {
        if (activeCreature.name == name)
            return;

        var newCreature = GetCreature(name);
        activeCreature = newCreature;
        OnActiveCreatureChanged?.Invoke(activeCreature);
    }

    public void SetRateUsShown(int value, bool savePrefs = false)
    {
        rateUsShown = value;

        if (savePrefs)
            PlayerPrefs.SetInt("rate_us", value);
    }

    public void UpdateHighscore(int score)
    {
        if (score <= highScore)
            return;

        highScore = score;
    }
}
