using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaskFactory
{
    private static readonly Dictionary<int, (float majorStat, float otherStats)> majorDistributions = new Dictionary<int, (float majorStat, float otherStats)>()
    {
        { 2, (0.75f, 0.25f) },
        { 3, (0.50f, 0.25f) },
        { 4, (0.40f, 0.20f) },
        { 5, (0.33f, 0.20f) }
    };

    public static Mask GetNewMask(int difficulty)
    {
        int statsTotal = FightUtility.MagicRound(5 + (difficulty * 0.1f));
        int emotionCount = Random.Range(2, EEmotionUtility.EmotionsList.Count + 1);
        int emotionsToRemove = EEmotionUtility.EmotionsList.Count - emotionCount;

        float rnd = Random.Range(0f, 1f);
        if (rnd > 0.5f && majorDistributions.TryGetValue(emotionCount, out (float majorStat, float otherStats) distributions))
        {
            return GetMaskMajorDistribution(statsTotal, emotionsToRemove, distributions.majorStat, distributions.otherStats);
        }

        return GetMaskBalancedDistribution(statsTotal, emotionsToRemove, 1 / (float)emotionCount);
    }

    private static Mask GetMaskBalancedDistribution(int statsTotal, int emotionsToRemove, float statDistribution)
    {
        Mask mask = new Mask();

        List<EEmotion> emotions = new List<EEmotion>(EEmotionUtility.EmotionsList);
        for (int i = 0; i < emotionsToRemove; i++)
            emotions.RemoveAt(Random.Range(0, emotions.Count));

        foreach (EEmotion emotion in emotions)
            mask.AddStat(emotion, FightUtility.MagicRound(statsTotal * statDistribution));
        mask.SetMainEmotion();

        return mask;
    }

    private static Mask GetMaskMajorDistribution(int statsTotal, int emotionsToRemove, float majorStatDistribution, float otherStatDistribution)
    {
        Mask mask = new Mask();
        List<EEmotion> emotions = new List<EEmotion>(EEmotionUtility.EmotionsList);

        for (int i = 0; i < emotionsToRemove; i++)
            emotions.RemoveAt(Random.Range(0, emotions.Count));

        EEmotion majorEmotion = emotions[Random.Range(0, emotions.Count)];
        mask.AddStat(majorEmotion, FightUtility.MagicRound(statsTotal * majorStatDistribution));
        emotions.Remove(majorEmotion);

        foreach (EEmotion emotion in emotions)
            mask.AddStat(emotion, FightUtility.MagicRound(statsTotal * otherStatDistribution));
        mask.SetMainEmotion();

        return mask;
    }

}
