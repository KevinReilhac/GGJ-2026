using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighter : MonoBehaviour
{
    public static PlayerFighter Instance {get; private set;} = null;

    void Awake()
    {
        Instance = this;
    }

    public event Action<Mask> OnChangeEquipedMask;
    public List<Mask> Masks = new List<Mask>();
    public List<EmotionStats> BaseStats = new List<EmotionStats>();

    public int EquipedMask = -1;

    public List<EmotionStats> GetCurrentStats()
    {
        Dictionary<EEmotion, int> statsDict = new Dictionary<EEmotion, int>();
        List<EmotionStats> emotionStats = new List<EmotionStats>();

        if (EquipedMask >= 0 && EquipedMask < Masks.Count)
        {
            Mask mask = Masks[EquipedMask];
            foreach (EmotionStats maskStat in mask.Stats)
            {
                if (!statsDict.ContainsKey(maskStat.emotionType))
                    statsDict.Add(maskStat.emotionType, 0);
                statsDict[maskStat.emotionType]++;
            }
            foreach (EmotionStats baseStat in BaseStats)
            {
                if (!statsDict.ContainsKey(baseStat.emotionType))
                    statsDict.Add(baseStat.emotionType, 0);
                statsDict[baseStat.emotionType]++;
            }
        }

        foreach (KeyValuePair<EEmotion, int> statItem in statsDict)
            emotionStats.Add(new EmotionStats(statItem.Key, statItem.Value));

        return emotionStats;
    }
}