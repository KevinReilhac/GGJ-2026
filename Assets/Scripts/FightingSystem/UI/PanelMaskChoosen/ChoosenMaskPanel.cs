using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChoosenMaskPanel : Panel
{
    public event Action<Attack> OnAttack;
    public event Action OnBackToMaskSelection;

    [System.Serializable]
    public class AttackStatItem
    {
        public EEmotion emotionType;
        public PlayerAttackStat playerAttackStat;
    }
    [Header("References")]
    [SerializeField] private List<AttackStatItem> attackStatItems;
    [SerializeField] private RawImage maskImage;
    [SerializeField] private Image background;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button maskSelectionButton;

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


    private void Awake()
    {
        attackButton.onClick.AddListener(OnPressAttackButton);
        maskSelectionButton.onClick.AddListener(OnPressMaskSelection);
        RegisterPlayerStatsOnValueChanged();
    }

    private void OnDestroy()
    {
        UnRegisterPlayerStatsOnValueChanged();
        attackButton.onClick.AddListener(OnPressAttackButton);
        maskSelectionButton.onClick.AddListener(OnPressMaskSelection);
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

    private void OnPressMaskSelection()
    {
        OnBackToMaskSelection?.Invoke();
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

    public void Setup(PlayerFighter playerFighter, Action OnBackToMaskSelectionCallback, Action<Attack> OnPlayerAttackCallback)
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

        OnAttack = OnPlayerAttackCallback;
        OnBackToMaskSelection = OnBackToMaskSelectionCallback;
    }

    public Attack GetPlayerAttack()
    {
        Attack attack = new Attack();

        foreach (var attackStatItem in attackStatsDict)
            attack.AddEmotionStat(attackStatItem.Key, attackStatItem.Value.Value);

        return attack;
    }
}