using System;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    public Action OnClick;

    public void OnTriggerExit2D(Collider2D collision)
    {
        OnClick?.Invoke();
    }
}
