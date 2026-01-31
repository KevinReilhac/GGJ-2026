
using System;
using System.Collections.Generic;

public static class FightManager
{
    public static event Action<Fight> OnWinFight;
    public static event Action<Fight> OnStartFight;
    public static event Action<Attack> OnNextEnemyAttack;
    public static event Action OnStartMaskSelection;
    public static event Action<List<Mask>> OnStartMaskLootSelection;
    private static Fight currentFight = null;
    public static Attack CurrentEnemyAttack;

    public static void StartFight(Fight fight) 
    {
        currentFight = fight;
        OnStartFight?.Invoke(fight);
    }

    public static void StartMaskSelection()
    {
        OnStartMaskSelection?.Invoke();
    }

    public static void SelectMask(int maskIndex)
    {
        PlayerFighter.Instance.EquipedMask = maskIndex;
    }

    public static void PlayerAttack()
    {
        Attack playerAttack = PlayerFighter.Instance.GetCurrentPlayerAttack();

        if (playerAttack.IsWinOrEqualAgainst(CurrentEnemyAttack))
            currentFight.HitEnemies();

        if (currentFight.IsAllDead())
            WinFight();
    }

    public static void NextAttack()
    {
        List<Attack> attacks = new List<Attack>();

        foreach (Enemy enemy in currentFight.GetAliveEnemies())
                attacks.Add(enemy.GetNextAttack());
        Attack combinedAttacks = Attack.Combine(attacks);

        CurrentEnemyAttack = combinedAttacks;
        OnNextEnemyAttack?.Invoke(combinedAttacks);
    }

    public static void WinFight()
    {
        OnStartMaskLootSelection?.Invoke(currentFight.GetMaskList());
        OnWinFight?.Invoke(currentFight);
    }
}