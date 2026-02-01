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
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.DORotate(new Vector3(0, deltaY, 0), animDuration).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo).SetDelay(Random.Range(0, 5));
    }
}
