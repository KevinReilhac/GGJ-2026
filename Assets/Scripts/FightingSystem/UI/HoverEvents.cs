using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEvents : MonoBehaviour
{
    public event Action OnPointerEnter;
    public event Action OnPointerExit;

    [SerializeField] private RectTransform targetRect;
    
    private bool _isInside;

    void OnEnable()
    {
        _isInside = RectTransformUtility.RectangleContainsScreenPoint(
            targetRect,
            Input.mousePosition
        );
    }

    private void Update()
    {
        bool isNowInside = RectTransformUtility.RectangleContainsScreenPoint(
            targetRect,
            Input.mousePosition
        );

        if (isNowInside && !_isInside)
        {
            _isInside = true;
            OnPointerEnter?.Invoke();
        }
        else if (!isNowInside && _isInside)
        {
            _isInside = false;
            OnPointerExit?.Invoke();
        }
    }
}
