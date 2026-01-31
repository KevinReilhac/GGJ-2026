
using System;
using System.Collections.Generic;

public static class FightManager
{
    public static event Action<Fight> OnWinFight;
    public static event Action<Fight> OnStartFight;
    public static event Action<List<Attack>> OnNextAttack;
    private static Fight currentFight = null;

    public static void StartFight(Fight fight) 
    {
        currentFight = fight;
        OnStartFight?.Invoke(fight);
    }

    public void 

    public static void WinFight()
    {
        OnWinFight?.Invoke(currentFight);
    }
}