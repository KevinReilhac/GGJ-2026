using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MaskAnimation : MonoBehaviour
{
    [SerializeField] private float animDuration = 1f;
    [SerializeField] private float deltaY = 10f;

    private void Awake()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(-deltaY, deltaY) + 180.0f, 0);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DORotate(new Vector3(0, deltaY + 180.0f, 0), animDuration).SetEase(Ease.InOutCubic));
        sequence.Append(transform.DORotate(new Vector3(0, -deltaY + 180.0f, 0), animDuration).SetEase(Ease.InOutCubic));
        sequence.SetLoops(-1, LoopType.Yoyo);
    }
}
