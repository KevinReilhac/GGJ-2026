using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerAttackToggle : MonoBehaviour, IPointerClickHandler
{
    public event Action<int> OnClick;

    [SerializeField] private Image background;
    [SerializeField] private Image fill;

    private bool selectable = false;
    private int index = 0;
    public bool Value {get; private set;} = false;

    public void SetColor(Color color)
    {
        fill.color = color;
    }

    public void Setup(int index, bool value, Action<int> OnClickCallback)
    {
        Value = value;
        OnClick = OnClickCallback;
        this.index = index;
        SetValue(value);
    }

    public void SetValue(bool value)
    {
        fill.gameObject.SetActive(value);
        Value = value;
    }


    public void SetSelectable(bool selectable)
    {
        this.selectable = selectable;
        background.enabled = selectable;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectable)
            OnClick?.Invoke(index);
    }
}
