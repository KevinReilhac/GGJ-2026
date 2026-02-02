using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPanel : Panel
{
    [Header("References")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private StatsDisplayer statsDisplayer;
    [SerializeField] private Image imageToColorize;
    [Header("Animation")]
    [SerializeField] private float showTime = 0.5f;
    [SerializeField] private Ease showEase = Ease.OutBack;
    [SerializeField] private float hideTime = 0.5f;
    [SerializeField] private Ease hideEase = Ease.InBack;


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
        imageToColorize.color = EmotionStat.GetMainEmotion(attack.GetEmotionsStatsList(), false).Color;
    }
    public override void ShowAnimation()
    {
        gameObject.SetActive(true);
        rectTransform.DOKill();
        rectTransform.DOSizeDelta(StartSize, showTime)
            .SetEase(showEase)
            .ChangeStartValue(new Vector2(StartSize.x, 0f));
    }

    public override void HideAnimation()
    {
        rectTransform.DOSizeDelta(new Vector2(StartSize.x, 0f), showTime)
            .SetEase(showEase)
            .OnComplete(() => gameObject.SetActive(false));
    }
}