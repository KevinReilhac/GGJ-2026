using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fight
{
    private static readonly List<float> ratios = new List<float>()
    {
        0.6f,
        0.3f,
        0.2f
    };
    private int difficulty = 0;
    private FightRoom fightRoom;
    private List<Enemy> enemies;

    public Fight(int difficulty)
    {
        this.difficulty = difficulty;
        fightRoom = FightRoom.Instance;

        fightRoom.DisableEnemies();
        int enemiesCount = Random.Range(1, fightRoom.enemies.Count);

        enemies = fightRoom.enemies.GetRange(0, enemiesCount);
        SetupEnemies();
    }

    private void SetupEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
            enemy.SetMaxHP(GetNewMaxHP());
            enemy.SetupMask(MaskFactory.GetNewMask(difficulty));
        }
    }

    public Attack GetNextAttack()
    {
        Attack attack = new Attack();
        List<Enemy> enemiesAlive = GetAliveEnemies();
        float ratio = ratios[enemies.Count];
        int attackValue = 0;
        List<EEmotion> emotions = new List<EEmotion>(EEmotionUtility.EmotionsList);

        if (enemies.Count > 1)
        {
            foreach (Enemy enemy in enemiesAlive)
            {
                attackValue = FightUtility.MagicRound(1 + (difficulty * ratio));
                
                EEmotion emotion = emotions[Random.Range(0, emotions.Count)];
                emotions.Remove(emotion);

                attack.AddEmotionStat(emotion, attackValue);
            }
        }
        else
        {
            attackValue = FightUtility.MagicRound(1 + (difficulty * ratio));
            while (attackValue > 0)
            {
                attack.AddEmotionStat(emotions[Random.Range(0, emotions.Count)], 1);
                attackValue--;
            }
        }



        return attack;
    }

    private int GetNewMaxHP()
    {
        return FightUtility.MagicRound(1 + (difficulty * 0.1f));
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

    public List<Mask> GetMaskList()
    {
        return enemies.Select(e => e.droppedMask).ToList();
    }


}