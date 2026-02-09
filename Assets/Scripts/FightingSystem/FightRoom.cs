using System.Collections.Generic;
using UnityEngine;

public class FightRoom : MonoBehaviour
{
    public static FightRoom Instance {get; private set;} = null;

    public List<Enemy> enemies;
    [SerializeField] private Transform fightContainer;

    private Fight currentFightInstance;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
        FightManager.OnStartFight += OnStartFight;
        FightManager.OnExitFight += OnExitFight;
    }

    void OnDestroy()
    {
        FightManager.OnStartFight -= OnStartFight;
        FightManager.OnExitFight -= OnExitFight;
    }

    private void OnStartFight(Fight fight)
    {
        gameObject.SetActive(true);
    }

    private void OnExitFight()
    {
        gameObject.SetActive(false);
    }

    public void DisableEnemies()
    {
        foreach (Enemy enemy in enemies)
            enemy.gameObject.SetActive(false);
    }
}
