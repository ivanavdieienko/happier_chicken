using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

public class ShopWindow : Window
{
    [SerializeField]
    RectTransform container;

    [SerializeField]
    TextMeshProUGUI errorMessage;

    private TweenerCore<Color, Color, ColorOptions> errorAnimation;

    public override void Show()
    {
        base.Show();
        
        var rect = GetComponent<RectTransform>();
        rect.sizeDelta = Vector2.zero;
    }

    void Awake()
    {
        var activeCreature = settings.GetActiveCreaturePrefab();

        foreach (var creature in settings.Creatures)
        {
            var shopItem = GameObject.Instantiate(creature.inShop, container).GetComponent<ShopItem>();

            shopItem.name = creature.inShop.name;
            shopItem.Initialize(creature.price, settings.IsPurchased(creature.name), activeCreature.name == creature.name, OnItemClick);
        }

        errorMessage.text = Localization.Get(Localization.NoGold);

        ResetErrorMessage();
    }

    override protected void OnDisable()
    {
        errorAnimation.Kill();
    }

    public void OnItemClick(ShopItem item)
    {
        if (settings.GetActiveCreaturePrefab().name == item.name)
        {
            return;
        }

        errorAnimation.Kill();
        ResetErrorMessage();

        if (!settings.IsPurchased(item.name))
        {
            if (settings.Gold >= item.Price)
            {
                settings.Gold -= item.Price;
                settings.AddToPurchased(item.name);
            }
            else
            {
                errorMessage.gameObject.SetActive(true);
                errorAnimation = DOTween.ToAlpha(()=> Color.white * errorMessage.alpha, x => errorMessage.alpha = x.a, 0f, 3f);
                errorAnimation.OnComplete(ResetErrorMessage);
                return;
            }
        }
        settings.SetActiveCreature(item.name);
        
        for (var i = 0; i < container.childCount; i++)
        {
            var child = container.GetChild(i);
            var creature = child.GetComponent<ShopItem>();
            creature.IsPurchased(settings.IsPurchased(child.name));
            creature.IsActive(child.name == item.name);
        }
    }

    private void ResetErrorMessage()
    {
        errorMessage.gameObject.SetActive(false);
        errorMessage.alpha = 1;
    }
}
