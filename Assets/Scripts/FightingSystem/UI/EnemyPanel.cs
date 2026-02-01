using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyPanel : Panel
{
    [Header("References")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private StatsDisplayer statsDisplayer;
    [Header("Animation")]
    [SerializeField] private float showTime = 0.5f;
    [SerializeField] private Ease showEase = Ease.OutBack;
    [SerializeField] private float hideTime = 0.5f;
    [SerializeField] private Ease hideEase = Ease.InBack;


    private Vector2 startSize;
    void Awake()
    {
        startSize = rectTransform.sizeDelta;
    }

    public void SetAttack(Attack attack)
    {
        statsDisplayer.SetValue(attack.GetEmotionsStatsList());
    }

    public void ShowAndSetup(Attack attack)
    {
        SetAttack(attack);
        ShowAnimation();
    }
    public override void ShowAnimation()
    {
        gameObject.SetActive(true);
        rectTransform.DOKill();
        rectTransform.DOSizeDelta(startSize, showTime)
            .SetEase(showEase)
            .ChangeStartValue(new Vector2(startSize.x, 0f));
    }

    public override void HideAnimation()
    {
        rectTransform.DOSizeDelta(new Vector2(startSize.x, 0f), showTime)
            .SetEase(showEase)
            .OnComplete(() => gameObject.SetActive(false));
    }
}