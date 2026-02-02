
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MaskChoicePanel : Panel
{
    [Header("References")]
    [SerializeField] private RectTransform cardsContainer;
    [SerializeField] private ChooseMaskCard cardTemplate;
    [SerializeField] private HorizontalLayoutGroup cardsContainerLayoutGroup;
    [Header("Animation")]
    [SerializeField] private float showTime = 0.5f;
    [SerializeField] private Ease showEase = Ease.OutBack;
    [SerializeField] private float hideTime = 0.5f;
    [SerializeField] private Ease hideEase = Ease.InBack;
    [SerializeField] private float onScreenTime = 2;
    [SerializeField] private float maximazedLayoutGroupSpace;
    [SerializeField] private float minimizedLayoutGroupSpace;
    private List<ChooseMaskCard> chooseMaskCards = new List<ChooseMaskCard>();
    private Action<Mask> OnMaskSelected;

    private Vector3 startCardsContainerPos = Vector3.one * -1;

    public override void ShowAnimation()
    {
        gameObject.SetActive(true);
        DOVirtual.Float(cardsContainerLayoutGroup.spacing, maximazedLayoutGroupSpace, showTime, (t) => {
            cardsContainerLayoutGroup.spacing = t;
        }).SetEase(showEase);
        cardsContainer.DOAnchorPosY(0f, showTime)
            .SetEase(showEase);
    }

    public override void HideAnimation()
    {
        cardsContainer.DOAnchorPosY(startCardsContainerPos.y, showTime)
            .SetEase(showEase);
        DOVirtual.Float(cardsContainerLayoutGroup.spacing, minimizedLayoutGroupSpace, showTime, (t) => {
            cardsContainerLayoutGroup.spacing = t;
        }).SetEase(showEase);
    }

    void Awake()
    {
        startCardsContainerPos = cardsContainer.anchoredPosition;
        cardTemplate.gameObject.SetActive(false);
    }

    public void SetupAndShow(List<Mask> masks, Action<Mask> onMaskSelectedCallback)
    {
        ClearCards();
        foreach (Mask mask in masks)
        {
            CreateCard(mask);
        }
        OnMaskSelected = onMaskSelectedCallback;
        Show();
    }

    private void CreateCard(Mask mask)
    {
        ChooseMaskCard chooseMaskCard = Instantiate(cardTemplate);
        chooseMaskCard.gameObject.SetActive(true);
        chooseMaskCard.transform.SetParent(cardsContainer);
        chooseMaskCard.SetMask(mask);
        chooseMaskCard.transform.localScale = Vector3.one;
        chooseMaskCards.Add(chooseMaskCard);

        chooseMaskCard.OnSelectCard += OnSelectMask;
    }

    private void OnSelectMask(ChooseMaskCard chooseMaskCard, Mask mask)
    {
        foreach (ChooseMaskCard maskCard in chooseMaskCards)
        {
            maskCard.gameObject.SetActive(maskCard != chooseMaskCard);
        }
        OnMaskSelected?.Invoke(mask);
    }

    private void ClearCards()
    {
        foreach (ChooseMaskCard chooseMaskCard in chooseMaskCards)
        {
            Destroy(chooseMaskCard.gameObject);
        }
        chooseMaskCards.Clear();
    }
}