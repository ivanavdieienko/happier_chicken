using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Window : MonoBehaviour
{
    [SerializeField]
    protected GameSettings settings;

    [SerializeField]
    protected Button btnClose;

    public event Action<Window> OnClose;

    protected virtual float onCloseDelay => 0f;

    private float delayCounter = 0f;

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        btnClose.onClick.AddListener(OnCloseClick);
    }

    protected virtual void Update()
    {
        if (delayCounter >= float.Epsilon)
        {
            delayCounter -= Time.deltaTime;
        }
    }

    protected virtual void OnDisable()
    {
        btnClose.onClick.RemoveListener(OnCloseClick);
    }

    protected virtual void OnCloseClick()
    {
        if (delayCounter < float.Epsilon)
        {
            delayCounter = onCloseDelay;

            OnClose?.Invoke(this);
            Hide();
        }
    }
}