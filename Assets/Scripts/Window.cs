using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Window : MonoBehaviour
{
    [SerializeField]
    protected GameSettings settings;

    [SerializeField]
    protected Button btnClose;

    public Action<Window> OnClose;

    protected virtual void OnEnable()
    {
        btnClose.onClick.AddListener(OnCloseClick);
    }

    protected virtual void OnDisable()
    {
        btnClose.onClick.RemoveListener(OnCloseClick);
    }

    protected virtual void OnCloseClick()
    {
        OnClose(this);
        Destroy(this.gameObject);
    }
}