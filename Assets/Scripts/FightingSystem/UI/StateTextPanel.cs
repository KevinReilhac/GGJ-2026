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
    [Header("References")]
    [SerializeField] private TextMeshProUGUI text;

    public override void ShowAnimation()
    {
        gameObject.SetActive(true);
        transform.DOScaleY(1f, showTime)
            .ChangeStartValue(new Vector3(1f, 0f, 1f))
            .SetEase(showEase);
    }

    public override void HideAnimation()
    {
        transform.DOScaleY(0f, hideTime)
            .SetEase(hideEase)
            .OnComplete(() => gameObject.SetActive(false));
    }

    public void SetupAndShow(string text)
    {
        this.text.text = text;
        Show();
    }
}
