using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerAttackToggle : MonoBehaviour, IPointerClickHandler
{
    public event Action<bool> OnValueChanged;

    [SerializeField] private Image background;
    [SerializeField] private Image fill;

    private bool selectable = false;
    public bool Value {get; private set;} = false;

    public void SetColor(Color color)
    {
        fill.color = color;
    }

    public void SetValue(bool value, bool notify = true)
    {
        Value = value;
        if (notify)
            OnValueChanged?.Invoke(value);
    }

    public void SetSelectable(bool selectable)
    {
        this.selectable = selectable;
        background.enabled = selectable;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectable)
            SetValue(Value);
    }
}
