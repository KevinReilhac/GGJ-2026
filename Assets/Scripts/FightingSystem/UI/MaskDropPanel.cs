using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskDropPanel : Panel
{
    [Header("References")]
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private DropMaskCard cardTemplate;
    private List<DropMaskCard> dropMaskCards = new List<DropMaskCard>();
    private Action<Mask> OnMaskSelected;

    void Awake()
    {
        cardTemplate.gameObject.SetActive(false);
    }

    public void Setup(List<Mask> masks, Action<Mask> OnMaskSelectedCallback)
    {
        ClearCards();
        masks.ForEach(CreateCard);
        OnMaskSelected = OnMaskSelectedCallback;
    }

    private void CreateCard(Mask mask)
    {
        DropMaskCard dropMaskCard = Instantiate(cardTemplate);
        dropMaskCard.gameObject.SetActive(true);
        dropMaskCard.transform.SetParent(cardsContainer);
        dropMaskCard.SetMask(mask);
        dropMaskCards.Add(dropMaskCard);
        dropMaskCard.transform.localScale = Vector3.one;

        dropMaskCard.OnSelectCard += OnSelectMask;
    }

    private void OnSelectMask(Mask mask)
    {
        OnMaskSelected?.Invoke(mask);
    }

    private void ClearCards()
    {
        foreach (DropMaskCard dropMaskCard in dropMaskCards)
        {
            Destroy(dropMaskCard.gameObject);
        }
        dropMaskCards.Clear();
    }
}