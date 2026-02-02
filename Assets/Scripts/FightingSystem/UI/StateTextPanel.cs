using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class StateTextPanel : Panel
{
    [Header("Animation")]
    [SerializeField] private float showTime = 0.5f;
    [SerializeField] private Ease showEase = Ease.OutBack;
    [SerializeField] private float hideTime = 0.5f;
    [SerializeField] private Ease hideEase = Ease.InBack;
    [SerializeField] private float onScreenTime = 2;
    [Header("References")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform rectTransform;

    private Vector2 startSize = Vector2.one * -1;
    public Vector2 StartSize
    {
        get
        {
            if (startSize == Vector2.one * -1)
                startSize = rectTransform.sizeDelta;
            return startSize;
        }
    }

    public override void ShowAnimation()
    {
        gameObject.SetActive(true);
        rectTransform.DOKill();
        rectTransform.DOSizeDelta(StartSize, showTime)
            .SetEase(showEase)
            .ChangeStartValue(new Vector2(StartSize.x, 0f))
            .OnComplete(() => Invoke(nameof(HideAnimation), onScreenTime));
    }

    public override void HideAnimation()
    {
        rectTransform.DOSizeDelta(new Vector2(StartSize.x, 0f), showTime)
            .SetEase(showEase)
            .OnComplete(() => gameObject.SetActive(false));
    }

    public void SetupAndShow(string text)
    {
        this.text.text = text;
        Show();
    }
}
