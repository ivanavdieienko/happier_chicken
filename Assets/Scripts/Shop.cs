using System;
using UnityEngine;
using UnityEngine.UI;

public enum Creatures
{
    NONE = 0,
    CHICKEN = 1,
    CHICKEN2 = 2,
    DUCK = 4,
    TURTLE = 8,
    OSTRICH = 16,
    PENGUIN = 32,
    DINO = 64,
    ALIEN = 128
}

public class Shop : MonoBehaviour
{
    [SerializeField]
    Button btnClose;

    [SerializeField]
    RectTransform container;

    public void Initialize(GameSettings settings)
    {
        var activeCreature = settings.GetActiveCreaturePrefab();

        foreach (var creature in settings.Creatures)
        {
            var shopItem = GameObject.Instantiate(creature.inShop, container).GetComponent<ShopItem>();

            Enum.TryParse(creature.name.ToUpper(), out Creatures c);

            shopItem.Initialize(creature.price, settings.IsPurchased((int) c), activeCreature.name == creature.name);
        }
    }

    void Start()
    {
        //Fill the shop with shop items

        //make possible to buy item

        //make possible to change active person
        
    }

    void OnEnable()
    {
        btnClose.onClick.AddListener(OnCloseClick);
    }

    void OnDisable()
    {
        btnClose.onClick.RemoveListener(OnCloseClick);
    }

    private void OnCloseClick()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
