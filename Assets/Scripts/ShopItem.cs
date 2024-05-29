using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    private Creatures creatureID;

    [SerializeField]
    private GameObject activeIcon;

    [SerializeField]
    private GameObject priceContainer;

    [SerializeField]
    private TextMeshProUGUI priceField;

    public void Initialize(int price, bool isPurchased, bool isActive)
    {
        priceContainer.SetActive(!isPurchased);
        activeIcon.SetActive(isActive);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
