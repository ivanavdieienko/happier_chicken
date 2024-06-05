using System;
using System.Linq;
using UnityEngine;

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
    [SerializeField] int gameTime;

    [SerializeField] Creature activeCreature;

    public Creature[] Creatures;

    [SerializeField] Window[] windows;

    [SerializeField] SoundData[] sounds;

    public event Action<int> OnGoldChanged;
    public event Action<Creature> OnActiveCreatureChanged;
    public event Action<CreatureType> OnCreaturePurchased;

    private int purchasedCreatures = (int) CreatureType.CHICKEN;

    private int gold;

    public int Gold { get => gold; set { gold = value; OnGoldChanged?.Invoke(value); } }

    public GameObject GetEggPrefab() => activeCreature.egg;

    public GameObject GetActiveCreaturePrefab() => activeCreature.parent;

    public void SetActiveCreature(string name)
    {
        if (activeCreature.name == name)
            return;

        var newCreature = GetCreature(name);
        activeCreature = newCreature;
        OnActiveCreatureChanged?.Invoke(activeCreature);
    }

    public int GetCreatureReward() => activeCreature.reward;

    public Creature GetCreature(string name) => Creatures.Where(data => data.name == name).First();

    public void AddToPurchased(string name)
    {
        Enum.TryParse(name.ToUpper(), out CreatureType creatureID);
        purchasedCreatures |= (int) creatureID;
        OnCreaturePurchased?.Invoke(creatureID);
    }

    public bool IsPurchased(string name)
    {
        Enum.TryParse(name.ToUpper(), out CreatureType creatureID);
        return IsPurchased((int) creatureID);
    }

    public bool IsPurchased(int creatureID) => (purchasedCreatures & creatureID) > 0;

    public Window GetScreen(string name) => windows.Where(data => data.name == name).First();

    public AudioClip GetSound(string name) => sounds.Where(data => data.name == name).First().sound;

    public int GetPurchasedCreatures => purchasedCreatures;

    public void SetPurchasedCreatures(int data)
    {
        if (data == (int) CreatureType.NONE)
            data =  (int) CreatureType.CHICKEN;

        purchasedCreatures = data;
    }
}
