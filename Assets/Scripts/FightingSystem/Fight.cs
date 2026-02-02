using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fight : MonoBehaviour
{
    private List<Enemy> enemies;
    public List<Enemy> Enemies
    {
        get
        {
            if (enemies == null)
                enemies = gameObject.GetComponentsInChildren<Enemy>(true).ToList();
            return enemies;
        }
    }

    public List<Enemy> GetAliveEnemies() => enemies.Where(e => !e.IsDead).ToList();
    public bool IsAllDead()
    {
        foreach (Enemy enemy in enemies)
        {
            if (!enemy.IsDead)
                return false;
        }

        return true;
    }

    public void HitEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.Hit();
        }
    }

    public void StartFight()
    {
        FightManager.StartFight(this);
    }

    public List<Mask> GetMaskList()
    {
        return enemies.Select(e => e.droppedMask).ToList();
    }
}