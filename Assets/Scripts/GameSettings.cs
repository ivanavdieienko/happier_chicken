using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

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

[Serializable]
public struct UiData
{
    public string name;
    public GameObject data;
}

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    [SerializeField] Creature activeCreature;

    public Creature[] Creatures;

    [SerializeField] UiData[] uiPrefabs;

    [SerializeField] SoundData[] sounds;

    [HideInInspector] public int gold;

    private int purchasedCreatures;

    public GameObject GetEggPrefab() => activeCreature.egg;
    public GameObject GetActiveCreaturePrefab() => activeCreature.parent;
    public void SetActiveCreature(string name) => activeCreature = GetCreature(name);
    public int GetCreatureReward() => activeCreature.reward;
    public AudioClip GetSound(string name) => sounds.Where(data => data.name == name).First().sound;
    public Creature GetCreature(string name) => Creatures.Where(data => data.name == name).First();
    public bool IsPurchased(int creatureID) => (purchasedCreatures & creatureID) > 0;
    public GameObject GetScreen(string name) => uiPrefabs.Where(data => data.name == name).First().data;

    void Awake()
    {
        gold = PlayerPrefs.GetInt("gold");
        var name = PlayerPrefs.GetString("activeCreature") ?? "chicken";
        activeCreature = GetCreature(name);
        purchasedCreatures = PlayerPrefs.GetInt("purchased");
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("gold", gold);
        PlayerPrefs.SetString("activeCreature", activeCreature.name);
    }
}