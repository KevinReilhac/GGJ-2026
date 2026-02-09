
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class FightManager
{
    public static event Action<Fight> OnWinFight;
    public static event Action<Fight> OnStartFight;
    public static event Action<Attack> OnNextEnemyAttack;
    public static event Action<List<Mask>> OnStartMaskLootSelection;
    public static event Action<List<Mask>> OnStartDropMaskSelection;
    public static event Action<Mask> OnSelectMask;
    public static event Action OnExitFight;
    public static event Action OnGameOver;
    private static Fight currentFight = null;
    public static Attack CurrentEnemyAttack;
    public static bool IsInFight = false;
    
    public static int Difficulty {get; private set;} = 0;

    public static void Reset()
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        Difficulty = 0;
        IsInFight = false;
        currentFight = null;
        CurrentEnemyAttack = null;
    }

    public static void StartFight()
    {
        currentFight = new Fight(Difficulty);
        OnStartFight?.Invoke(currentFight);
        NextAttack();
        IsInFight = true;
    }

    public static void SelectMask(int maskIndex)
    {
        PlayerFighter.Instance.EquipedMaskIndex = maskIndex;
    }

    public static void SelectMask(Mask mask)
    {
        PlayerFighter.Instance.SelectMask(mask);
        OnSelectMask.Invoke(mask);
    }

    public static void PlayVFXAttack()
    {
    }

    public static async void PlayerAttack(Attack playerAttack)
    {
        if (playerAttack.CompareAttacks(CurrentEnemyAttack, out Attack statsDamages))
        {
            VFXManager._instance.GetVFX(CurrentEnemyAttack.GetEmotionsStatsList(), true);
            await Task.Delay(3500);

            Debug.LogFormat("Player damages \n {0}", statsDamages);
            ApplyPlayerStatsDamages(statsDamages);
            currentFight.HitEnemies();
            if (currentFight.IsAllDead())
            {
                WinFight();
                Difficulty++;
            }
            else
            {
                PlayerFighter.Instance.EquipedMaskIndex = -1;
                NextAttack();
            }
        }
        else
        {
            VFXManager._instance.GetVFX(CurrentEnemyAttack.GetEmotionsStatsList(), false);
            await Task.Delay(3000);
            OnGameOver?.Invoke();
            Debug.Log("GameOver");
        }

    }

    private static void ApplyPlayerStatsDamages(Attack statsDamages)
    {
        int maskStat;
        int playerStat;
        int attackStat;

        foreach (EEmotion eEmotion in EEmotionUtility.EmotionsList)
        {
            maskStat = 0;
            playerStat = PlayerFighter.Instance.GetBaseStat(eEmotion);
            attackStat = statsDamages.GetEmotionStat(eEmotion);

            if (attackStat <= 0)
                continue;

            if (PlayerFighter.Instance.EquipedMask != null)
                maskStat = PlayerFighter.Instance.EquipedMask.GetStat(eEmotion);

            if (maskStat > 0)
            {
                int maskDamages = Mathf.Min(maskStat, attackStat);
                PlayerFighter.Instance.EquipedMask.DamageMask(eEmotion, maskDamages);
                attackStat -= maskDamages;
            }
            if (attackStat > 0)
            {
                PlayerFighter.Instance.AddBaseStat(eEmotion, -attackStat);
            }
        }
    }

    public static void NextAttack()
    {
        Attack attack = currentFight.GetNextAttack();

        CurrentEnemyAttack = attack;
        OnNextEnemyAttack?.Invoke(attack);
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

    public static void EndFight()
    {
        OnExitFight?.Invoke();
    }
}