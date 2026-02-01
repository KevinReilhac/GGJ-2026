using System;
using System.Collections.Generic;
using System.Linq;
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
    private Dictionary<EEmotion, int> baseStatsDict;
    public Dictionary<EEmotion, int> BaseStatsDict
    {
        get
        {
            if (baseStatsDict == null)
                baseStatsDict = BaseStats.ToDictionary(i => i.emotionType, i => i.stat);
            return baseStatsDict;
        }
    }

    public int EquipedMaskIndex = -1;

    public Mask EquipedMask
    {
        get
        {
            if (EquipedMaskIndex < 0 || EquipedMaskIndex >= Masks.Count)
                return null;
            return Masks[EquipedMaskIndex];
        }
    }

    public void SelectMask(Mask mask)
    {
        if (Masks.Contains(mask))
        {
            EquipedMaskIndex = Masks.IndexOf(mask);
        }
    }

    public void ClearMasks()
    {
        List<Mask> masksCopy = new List<Mask>(Masks);

        foreach (Mask mask in masksCopy)
            Masks.Remove(mask);
    }

    public Attack GetPlayerAttackFullStats()
    {
        Dictionary<EEmotion, int> statsDict = new Dictionary<EEmotion, int>();
        List<EmotionStat> emotionStats = new List<EmotionStat>();

        if (EquipedMask != null)
        {
            foreach (EmotionStat maskStat in EquipedMask.Stats)
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

        foreach (KeyValuePair<EEmotion, int> statItem in statsDict)
            emotionStats.Add(new EmotionStat(statItem.Key, statItem.Value));

        return new Attack(emotionStats);
    }

    public int GetBaseStat(EEmotion eEmotion)
    {
        return BaseStatsDict.GetValueOrDefault(eEmotion, 0);
    }

    public void AddBaseStat(EEmotion eEmotion, int value)
    {
        Debug.LogFormat("Add player {0} stat by {0}", eEmotion, value);
        if (BaseStatsDict.ContainsKey(eEmotion))
            BaseStatsDict[eEmotion] -= value;
        else
            BaseStatsDict.Add(eEmotion, value);
    }
}