
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    [SerializeField] private StatsDisplayer statsDisplayers;
    [SerializeField] private Image imageToColorize;
    [SerializeField] private RawImage rawImageMask;

    public event Action<ChooseMaskCard, Mask> OnSelectCard;
    private Mask mask = null;

    void Awake()
    {
        statsPanel.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelectCard?.Invoke(this, mask);
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
        statsDisplayers.SetValue(mask.Stats);
        imageToColorize.color = mask.MainEmotion.Color;
        rawImageMask.texture = Mask3DTextures.Instance.GetTexture(mask.MainEmotion.EmotionType);
    }

}