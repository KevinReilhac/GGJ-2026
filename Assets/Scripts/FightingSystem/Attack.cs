
using System;
using System.Collections.Generic;
using System.Linq;

public class Attack
{
    private Dictionary<EEmotion, int> statsDict;

    public static Attack Combine(List<Attack> attacks)
    {
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

    public Attack(Attack attack)
    {
        statsDict = new Dictionary<EEmotion, int>(attack.statsDict);
    }

    public bool IsWinOrEqualAgainst(Attack otherAttack)
    {
        foreach (int enumValue in Enum.GetValues(typeof(EEmotion)))
        {
            EEmotion emotionType = (EEmotion)enumValue;
            if (GetEmotionStat(emotionType) < otherAttack.GetEmotionStat(emotionType))
            {
                return false;
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

}