using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChoosenMaskPanel : Panel
{
    public event Action<Attack> OnAttack;
    public event Action OnHoverCardsEvent;
    public event Action OnUnhoverCardsEvent;

    [System.Serializable]
    public class AttackStatItem
    {
        public EEmotion emotionType;
        public PlayerAttackStat playerAttackStat;
    }
    [Header("Animation")]
    [SerializeField] private float showTime = 0.5f;
    [SerializeField] private Ease showEase = Ease.OutBack;
    [SerializeField] private float hideTime = 0.5f;
    [SerializeField] private Ease hideEase = Ease.InBack;
    [SerializeField] private float onScreenTime = 2;
    [SerializeField] private HoverEvents hoverCardsEvent;
    [Header("References")]
    [SerializeField] private List<AttackStatItem> attackStatItems;
    [SerializeField] private RawImage maskImage;
    [SerializeField] private Image background;
    [SerializeField] private Button attackButton;
    [SerializeField] private CanvasGroup buttonsCanvasGroup;
    [SerializeField] private RectTransform choosenMask;

    private Dictionary<EEmotion, PlayerAttackStat> attackStatsDict = null;
    private Dictionary<EEmotion, PlayerAttackStat> AttackStatsDict
    {
        get
        {
            if (attackStatsDict == null)
                attackStatsDict = attackStatItems.ToDictionary(a => a.emotionType, a => a.playerAttackStat);
            return attackStatsDict;
        }
    }

    private Vector3 startChoosenPos = Vector3.one * -1;
    public Vector3 StartChoosenPos
    {
        get
        {
            if (startChoosenPos == Vector3.one * -1)
                startChoosenPos = choosenMask.anchoredPosition;
            return startChoosenPos;
        }
    }

    public override void ShowAnimation()
    {
        gameObject.SetActive(true);
        buttonsCanvasGroup.DOFade(1f, showTime)
            .SetEase(showEase);
        choosenMask.DOAnchorPosX(0f, showTime)
            .SetEase(showEase)
            .ChangeStartValue(StartChoosenPos);
        hoverCardsEvent.gameObject.SetActive(true);
    }

    public override void HideAnimation()
    {
        buttonsCanvasGroup.alpha = 0f;
        choosenMask.DOAnchorPosX(StartChoosenPos.x, showTime)
            .SetEase(showEase)
            .OnComplete(() => gameObject.SetActive(false));
        hoverCardsEvent.gameObject.SetActive(false);
    }

    private void Awake()
    {
        attackButton.onClick.AddListener(OnPressAttackButton);
        RegisterPlayerStatsOnValueChanged();

        hoverCardsEvent.OnPointerEnter += OnHoverCard;
        hoverCardsEvent.OnPointerExit += OnUnhoverCard;
    }
    private void OnDestroy()
    {
        UnRegisterPlayerStatsOnValueChanged();
        attackButton.onClick.AddListener(OnPressAttackButton);

        hoverCardsEvent.OnPointerEnter -= OnHoverCard;
        hoverCardsEvent.OnPointerExit -= OnUnhoverCard;
    }

    private void OnHoverCard()
    {
        OnHoverCardsEvent?.Invoke();
    }

    private void OnUnhoverCard()
    {
        OnUnhoverCardsEvent?.Invoke();
    }


    private void RegisterPlayerStatsOnValueChanged()
    {
        foreach (AttackStatItem playerAttackStat in attackStatItems)
        {
            playerAttackStat.playerAttackStat.OnValueChanged += OnPlayerAttackStatChanged;
        }
    }

    private void UnRegisterPlayerStatsOnValueChanged()
    {
        foreach (AttackStatItem playerAttackStat in attackStatItems)
        {
            playerAttackStat.playerAttackStat.OnValueChanged -= OnPlayerAttackStatChanged;
        }
    }

    private void OnPlayerAttackStatChanged(int _)
    {
        int totalValues = GetTotalValue();

        if (totalValues >= FightManager.CurrentEnemyAttack.GetTotal())
            DisableUncheckedToggles();
        else
            EnableUncheckedToggles();
    }

    private void DisableUncheckedToggles()
    {
        foreach (var item in attackStatItems)
            item.playerAttackStat.DisableFalseToggles();
    }

    private void EnableUncheckedToggles()
    {
        foreach (var item in attackStatItems)
            item.playerAttackStat.EnableAllToggles();
    }

    private void OnPressAttackButton()
    {
        OnAttack?.Invoke(GetPlayerAttack());
    }

    private int GetTotalValue()
    {
        int value = 0;
        foreach (AttackStatItem playerAttackStat in attackStatItems)
            value += playerAttackStat.playerAttackStat.Value;
        return value;
    }

    public void SetupAndShow(PlayerFighter playerFighter, Action<Attack> OnPlayerAttackCallback)
    {
        Attack playerAttack = playerFighter.GetPlayerAttackFullStats();

        foreach (EmotionStat emotionStat in playerAttack.GetEmotionsStatsList())
            AttackStatsDict[emotionStat.emotionType].SetToggles(emotionStat.stat, true);
        if (playerFighter.EquipedMask == null)
        {
            maskImage.gameObject.SetActive(false);
            background.color = Color.white;
        }
        else
        {
            Emotion emotion = playerFighter.EquipedMask.MainEmotion;
            maskImage.texture = Mask3DTextures.Instance.GetTexture(emotion.EmotionType);
            background.color = emotion.Color;
        }

        Show();
        OnAttack = OnPlayerAttackCallback;
    }

    public Attack GetPlayerAttack()
    {
        Attack attack = new Attack();

        foreach (var attackStatItem in attackStatsDict)
            attack.AddEmotionStat(attackStatItem.Key, attackStatItem.Value.Value);

        return attack;
    }
}