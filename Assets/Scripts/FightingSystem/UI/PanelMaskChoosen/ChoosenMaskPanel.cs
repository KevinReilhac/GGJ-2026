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
    }

    private void OnDestroy()
    {
        attackButton.onClick.AddListener(OnPressAttackButton);
        maskSelectionButton.onClick.AddListener(OnPressMaskSelection);
    }

    private void OnPressMaskSelection()
    {
        OnBackToMaskSelection?.Invoke();
    }

    private void OnPressAttackButton()
    {
        OnAttack?.Invoke(GetPlayerAttack());
    }


    public void Setup(PlayerFighter playerFighter, Attack enemyAttack)
    {
        Attack playerAttack = playerFighter.GetPlayerAttackFullStats();

        foreach (EmotionStat emotionStat in playerAttack.GetEmotionsStatsList())
            AttackStatsDict[emotionStat.emotionType].SetToggles(emotionStat.stat, true);
        if (playerFighter.EquipedMask == null)
            maskImage.gameObject.SetActive(false);
        else
        {
            maskImage.texture = Mask3DTextures.Instance.GetTexture(playerFighter.EquipedMask.MainEmotion.EmotionType);
        }
    }

    public Attack GetPlayerAttack()
    {
        Attack attack = new Attack();

        foreach (var attackStatItem in attackStatsDict)
            attack.AddEmotionStat(attackStatItem.Key, attackStatItem.Value.Value);

        return attack;
    }
}