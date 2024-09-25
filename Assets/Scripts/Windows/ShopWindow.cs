using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class ShopWindow : Window
{
    [SerializeField]
    RectTransform container;

    [SerializeField]
    TextMesh noGold;

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
            var shopItem = Instantiate(creature.inShop, container).GetComponent<ShopItem>();

            shopItem.name = creature.inShop.name;
            shopItem.Initialize(creature.price, settings.IsPurchased(creature.name), activeCreature.name == creature.name, OnItemClick);
        }

        var hintPosition = noGold.transform.position;
        hintPosition.x = Screen.width * 0.5f;
        noGold.transform.position = hintPosition;

        ResetNoGoldView();
    }

    override protected void OnDisable()
    {
        errorAnimation?.Kill();
        ResetNoGoldView();
    }

    public void OnItemClick(ShopItem item)
    {
        if (settings.GetActiveCreaturePrefab().name == item.name)
        {
            return;
        }

        errorAnimation?.Kill();
        ResetNoGoldView();

        if (!settings.IsPurchased(item.name))
        {
            if (settings.Gold >= item.Price)
            {
                settings.Gold -= item.Price;
                settings.AddToPurchased(item.name);
            }
            else
            {
                noGold.gameObject.SetActive(true);
                errorAnimation = DOTween.ToAlpha(()=> noGold.color, x => noGold.color = x, 0f, 3f);
                errorAnimation.OnComplete(ResetNoGoldView);
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

    private void ResetNoGoldView()
    {
        noGold.gameObject.SetActive(false);
        Color color = noGold.color;
        color.a = 1;
        noGold.color = color;
    }
}
