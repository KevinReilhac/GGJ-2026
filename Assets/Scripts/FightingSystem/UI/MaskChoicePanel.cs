
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaskChoicePanel : Panel
{
    [Header("References")]
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private ChooseMaskCard cardTemplate;
    private List<ChooseMaskCard> chooseMaskCards = new List<ChooseMaskCard>();
    private Action<Mask> OnMaskSelected;

    void Awake()
    {
        cardTemplate.gameObject.SetActive(false);
    }

    public void Setup(List<Mask> masks, Action<Mask> onMaskSelectedCallback)
    {
        ClearCards();
        masks.ForEach(CreateCard);
        OnMaskSelected = onMaskSelectedCallback;
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

    private void OnSelectMask(Mask mask)
    {
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