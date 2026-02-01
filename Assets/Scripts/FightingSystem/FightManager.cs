
using System;
using System.Collections.Generic;
using UnityEngine;

public static class FightManager
{
    public static event Action<Fight> OnWinFight;
    public static event Action<Fight> OnStartFight;
    public static event Action<Attack> OnNextEnemyAttack;
    public static event Action OnStartMaskSelection;
    public static event Action<List<Mask>> OnStartMaskLootSelection;
    public static event Action<List<Mask>> OnStartDropMaskSelection;
    public static event Action OnExitFight;
    public static event Action OnGameOver;
    private static Fight currentFight = null;
    public static Attack CurrentEnemyAttack;
    public static bool IsInFight = false;

    public static void StartFight(Fight fight)
    {
        foreach (Enemy enemy in fight.enemies)
            enemy.Reset();
        currentFight = fight;
        OnStartFight?.Invoke(fight);
        NextAttack();
        StartMaskSelection();
        IsInFight = true;
    }

    public static void StartMaskSelection()
    {
        OnStartMaskSelection?.Invoke();
    }

    public static void SelectMask(int maskIndex)
    {
        PlayerFighter.Instance.EquipedMaskIndex = maskIndex;
    }

    public static void SelectMask(Mask mask)
    {
        PlayerFighter.Instance.SelectMask(mask);
    }

    public static void PlayerAttack(Attack playerAttack)
    {
        VFXManager._instance.GetVFX(CurrentEnemyAttack.GetEmotionsStatsList());
        if (playerAttack.CompareAttacks(CurrentEnemyAttack, out Attack statsDamages))
        {
            Debug.LogFormat("Player damages \n {0}", statsDamages);
            ApplyPlayerStatsDamages(statsDamages);
            currentFight.HitEnemies();
            if (currentFight.IsAllDead())
                WinFight();
            else
            {
                NextAttack();
                StartMaskSelection();
            }
        }
        else
        {
            OnGameOver?.Invoke();
            Debug.Log("GameOver");
        }

    }

    private static void ApplyPlayerStatsDamages(Attack statsDamages)
    {
        int maskStat;
        int playerStat;
        int attackStat;
        int diff;

        foreach (EEmotion eEmotion in EEmotionUtility.EmotionsList)
        {
            maskStat = 0;
            playerStat = PlayerFighter.Instance.GetBaseStat(eEmotion);
            attackStat = statsDamages.GetEmotionStat(eEmotion);

            if (PlayerFighter.Instance.EquipedMask != null)
                maskStat = PlayerFighter.Instance.EquipedMask.GetStat(eEmotion);

            if (maskStat > 0)
            {
                diff = attackStat - maskStat;
                if (diff < 0)
                    PlayerFighter.Instance.EquipedMask.DamageMask(eEmotion, -diff);
                attackStat -= diff;
            }
            if (attackStat > 0)
            {
                diff = attackStat - playerStat;
                if (diff > 0)
                    PlayerFighter.Instance.AddBaseStat(eEmotion, -diff);
            }
        }
    }

    public static void NextAttack()
    {
        List<Attack> attacks = new List<Attack>();

        foreach (Enemy enemy in currentFight.GetAliveEnemies())
            attacks.Add(enemy.GetNextAttack());
        Attack combinedAttacks = Attack.Combine(attacks);

        CurrentEnemyAttack = combinedAttacks;
        Debug.LogFormat("New Enemy attack \n{0}", combinedAttacks);
        OnNextEnemyAttack?.Invoke(combinedAttacks);
    }

    public static void WinFight()
    {
        OnStartMaskLootSelection?.Invoke(currentFight.GetMaskList());
        OnWinFight?.Invoke(currentFight);
    }

    public static void SelectDroppedMask(Mask mask)
    {
        PlayerFighter.Instance.Masks.Add(mask);
        EndFight();
    }

    private static void EndFight()
    {
        OnExitFight?.Invoke();
    }
}