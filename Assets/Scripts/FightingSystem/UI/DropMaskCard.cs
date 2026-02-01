using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropMaskCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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

    public event Action<Mask> OnSelectCard;

    private Mask mask = null;

    void Awake()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelectCard?.Invoke(mask);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(Vector3.one * hoveredScale, scaleDownTime).SetEase(scaleUpEase);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(Vector3.one, scaleDownTime).SetEase(scaleDownEase);
    }


    public void SetMask(Mask mask)
    {
        this.mask = mask;
        statsDisplayers.SetValue(mask.Stats);
        imageToColorize.color = mask.MainEmotion.Color;
        rawImageMask.texture = Mask3DTextures.Instance.GetTexture(mask.MainEmotion.EmotionType);
    }
}
