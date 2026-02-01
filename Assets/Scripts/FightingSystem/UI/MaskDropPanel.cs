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
        Setup(new List<Mask>()
        {
            new Mask(joy : 4),
            new Mask(sad : 4),
            new Mask(angry : 4),
            new Mask(disgust : 4),
        });
    }

    public void Setup(List<Mask> masks)
    {
        ClearCards();
        masks.ForEach(CreateCard);
    }

    private void CreateCard(Mask mask)
    {
        DropMaskCard dropMaskCard = Instantiate(cardTemplate);
        dropMaskCard.gameObject.SetActive(true);
        dropMaskCard.transform.SetParent(cardsContainer);
        dropMaskCard.SetMask(mask);
        dropMaskCards.Add(dropMaskCard);

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
            Destroy(dropMaskCard);
        }
        dropMaskCards.Clear();
    }
}