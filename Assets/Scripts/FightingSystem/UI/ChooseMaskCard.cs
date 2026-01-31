
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChooseMaskCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Animation")]
    [SerializeField] private float scaleUpTime = 0.5f;
    [SerializeField] private Ease scaleUpEase = Ease.OutBack;
    [SerializeField] private float scaleDownTime = 0.5f;
    [SerializeField] private Ease scaleDownEase = Ease.InBack;
    [SerializeField] private float hoveredScale = 1.2f;
    [Header("References")]
    [SerializeField] private RectTransform statsPanel;
    [SerializeField] private List<StatsDisplayer> statsDisplayers ;

    public event Action<Mask> OnSelectCard;

    private Dictionary<EEmotion, StatsDisplayer> statsDiplayerDict = null;
    private Dictionary<EEmotion, StatsDisplayer> StatsDisplayerDict
    {
        get
        {
            if (statsDiplayerDict == null)
                statsDiplayerDict = statsDisplayers.ToDictionary(s => s.Emotion, s => s);
            return statsDiplayerDict;
        }
    }


    private Mask mask = null;

    void Awake()
    {
        statsPanel.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelectCard?.Invoke(mask);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(Vector3.one * hoveredScale, scaleDownTime).SetEase(scaleUpEase);
        statsPanel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(Vector3.one, scaleDownTime).SetEase(scaleDownEase);
        statsPanel.gameObject.SetActive(false);
    }


    public void SetMask(Mask mask)
    {
        this.mask = mask;
        ResetStats();
        foreach (EmotionStat stat in mask.Stats)
        {
            if (StatsDisplayerDict.TryGetValue(stat.emotionType, out StatsDisplayer statsDisplayer))
                statsDisplayer.SetValue(stat.stat);
        }
    }

    private void ResetStats()
    {
        foreach (StatsDisplayer statsDisplayer in statsDisplayers)
            statsDisplayer.SetValue(0);
    }


}