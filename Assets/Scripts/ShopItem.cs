using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShopItem : MonoBehaviour
{
    [SerializeField]
    private CreatureType creatureID;

    [SerializeField]
    private GameObject activeIcon;

    [SerializeField]
    private GameObject priceContainer;

    [SerializeField]
    private TextMesh priceField;

    private Action<ShopItem> OnClickCallback;

    private Button clickArea;

    public int Price{ get; private set; }

    public void Initialize(int price, bool isPurchased, bool isActive, Action<ShopItem> callback)
    {
        IsPurchased(isPurchased);
        IsActive(isActive);

        OnClickCallback = callback;

        Price = price;
        priceField.text = ArabicFixerTool.FixLine(price.ToString());
    }

    internal void IsActive(bool value)
    {
        activeIcon.SetActive(value);
        if (value)
        {
            priceContainer.SetActive(false);
        }
    }

    public void IsPurchased(bool value)
    {
        priceContainer.SetActive(!value);
    }

    public void OnClick() => OnClickCallback(this);

    void Awake()
    {
        clickArea = GetComponent<Button>();
    }

    void OnEnable()
    {
        clickArea.onClick.AddListener(OnClick);
    }

    void OnDisable()
    {
        clickArea.onClick.RemoveListener(OnClick);
    }

    void OnDestroy()
    {
        OnClickCallback = null;
    }
}
