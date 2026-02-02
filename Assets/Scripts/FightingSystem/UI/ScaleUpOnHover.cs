using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleUpOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Animation")]
    [SerializeField] private float scaleUpTime = 0.5f;
    [SerializeField] private Ease scaleUpEase = Ease.OutBack;
    [SerializeField] private float scaleDownTime = 0.5f;
    [SerializeField] private Ease scaleDownEase = Ease.InBack;
    [SerializeField] private float hoveredScale = 1.2f;

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
}
