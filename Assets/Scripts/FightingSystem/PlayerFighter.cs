using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighter : MonoBehaviour
{
    public static PlayerFighter Instance { get; private set; } = null;

    void Awake()
    {
        Instance = this;
    }

    public event Action<Mask> OnChangeEquipedMask;
    public event Action<List<EmotionStat>> OnStatsUpdated;
    public List<Mask> Masks = new List<Mask>();
    public List<EmotionStat> BaseStats = new List<EmotionStat>();

    public int EquipedMask = -1;
    public Dictionary<EEmotion, int> AdditionalStats;

    public Attack GetCurrentPlayerAttack()
    {
        Dictionary<EEmotion, int> statsDict = new Dictionary<EEmotion, int>();
        List<EmotionStat> emotionStats = new List<EmotionStat>();

        if (EquipedMask >= 0 && EquipedMask < Masks.Count)
        {
            Mask mask = Masks[EquipedMask];
            foreach (EmotionStat maskStat in mask.Stats)
            {
                if (!statsDict.ContainsKey(maskStat.emotionType))
                    statsDict.Add(maskStat.emotionType, 0);
                statsDict[maskStat.emotionType] += maskStat.stat;
            }
        }
        foreach (EmotionStat baseStat in BaseStats)
        {
            if (!statsDict.ContainsKey(baseStat.emotionType))
                statsDict.Add(baseStat.emotionType, 0);
            statsDict[baseStat.emotionType] += baseStat.stat;
        }
        foreach (var additionalStatsItem in AdditionalStats)
        {
            if (!statsDict.ContainsKey(additionalStatsItem.Key))
                statsDict.Add(additionalStatsItem.Key, 0);
            statsDict[additionalStatsItem.Key] += additionalStatsItem.Value;
        }

            foreach (KeyValuePair<EEmotion, int> statItem in statsDict)
                emotionStats.Add(new EmotionStat(statItem.Key, statItem.Value));

        return new Attack(emotionStats);
    }
}