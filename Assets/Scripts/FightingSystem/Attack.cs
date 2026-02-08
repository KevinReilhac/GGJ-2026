
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class Attack
{
    private Dictionary<EEmotion, int> statsDict;

    public static Attack Combine(List<Attack> attacks)
    {
        if (attacks == null || attacks.Count == 0)
            return new Attack();
        if (attacks.Count == 1)
            return attacks[0];
        Attack combinedAttack = new Attack(attacks[0]);
        for (int attackIndex = 1; attackIndex < attacks.Count; attackIndex++)
        {
            foreach (int enumValue in Enum.GetValues(typeof(EEmotion)))
            {
                EEmotion emotionType = (EEmotion)enumValue;
                if (emotionType == EEmotion.Multiple) continue;
                combinedAttack.AddEmotionStat(emotionType, attacks[attackIndex].GetEmotionStat(emotionType));
            }
        }

        return combinedAttack;
    }

    public Attack(List<EmotionStat> emotionStats)
    {
        statsDict = emotionStats.ToDictionary(e => e.emotionType, e => e.stat);
    }

    public Attack()
    {
        statsDict = new Dictionary<EEmotion, int>();
    }

    public Attack(Attack attack)
    {
        statsDict = new Dictionary<EEmotion, int>(attack.statsDict);
    }

    public int GetTotal()
    {
        int value = 0;
        foreach (var item in statsDict)
            value += item.Value;

        return value;
    }

    public bool CompareAttacks(Attack otherAttack, out Attack statsDamages)
    {
        int total = GetTotal();
        int otherTotal = otherAttack.GetTotal();
        statsDamages = new Attack();

        if (total < otherTotal)
            return false;

        if (total >= otherAttack.GetTotal())
        {
            int stat;
            int otherStat;
            foreach (EEmotion eEmotion in EEmotionUtility.EmotionsList)
            {
                stat = GetEmotionStat(eEmotion);
                otherStat = otherAttack.GetEmotionStat(eEmotion);

                if (stat > otherStat)
                    statsDamages.AddEmotionStat(eEmotion, Mathf.Max(stat - otherStat, 0));
            }
        }

        return true;
    }

    public int GetEmotionStat(EEmotion emotion)
    {
        return statsDict.GetValueOrDefault(emotion, 0);
    }

    public void SetEmotionStat(EEmotion emotion, int value)
    {
        if (!statsDict.ContainsKey(emotion))
            statsDict.Add(emotion, 0);
        statsDict[emotion] = value;
    }

    public void AddEmotionStat(EEmotion emotion, int delta)
    {
        if (!statsDict.ContainsKey(emotion))
            statsDict.Add(emotion, 0);
        statsDict[emotion] += delta;
    }

    public List<EmotionStat> GetEmotionsStatsList()
    {
        List<EmotionStat> emotionStats = new List<EmotionStat>();

        foreach (var item in statsDict)
        {
            emotionStats.Add(new EmotionStat(item.Key, item.Value));
        }

        return emotionStats;
    }


    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (EEmotion emotion in EEmotionUtility.EmotionsList)
        {
            stringBuilder.AppendLineFormat("{0} : {1}", emotion, GetEmotionStat(emotion));
        }

        return stringBuilder.ToString();
    }

}