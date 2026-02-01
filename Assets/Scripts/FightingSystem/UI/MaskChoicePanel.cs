
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
        Setup(new List<Mask>()
        {
            new Mask(joy : 4, sad : 4),
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
        ChooseMaskCard chooseMaskCard = Instantiate(cardTemplate);
        chooseMaskCard.gameObject.SetActive(true);
        chooseMaskCard.transform.SetParent(cardsContainer);
        chooseMaskCard.SetMask(mask);

        chooseMaskCard.OnSelectCard += OnSelectMask;
    }

    private void OnSelectMask(Mask mask)
    {
        Debug.Log(mask);
    }

    private void ClearCards()
    {
        foreach (ChooseMaskCard chooseMaskCard in chooseMaskCards)
        {
            Destroy(chooseMaskCard);
        }
        chooseMaskCards.Clear();
    }
}