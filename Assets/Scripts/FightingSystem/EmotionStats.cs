using System.Collections.Generic;

[System.Serializable]
public class EmotionStat
{
    public EEmotion emotionType;
    public int stat;

    public EmotionStat(EEmotion emotionType, int stat)
    {
        this.emotionType = emotionType;
        this.stat = stat;
    }

    public static Emotion GetMainEmotion(List<EmotionStat> emotionStats, bool includeRainbow = true)
    {
        EEmotion highestEmotion = EEmotion.Angry;
        int highestStat = -1;

        if (includeRainbow && emotionStats.Count == EEmotionUtility.EmotionsList.Count)
            return FightSettings.Instance.GetEmotionFromEmotionType(EEmotion.Rainbow);

        foreach (EmotionStat maskStat in emotionStats)
        {
            if (maskStat.stat > highestStat)
            {
                highestStat = maskStat.stat;
                highestEmotion = maskStat.emotionType;
            }
            else if (maskStat.stat == highestStat)
            {
                highestEmotion = EEmotion.Multiple;
            }
        }

        return FightSettings.Instance.GetEmotionFromEmotionType(highestEmotion);
    }
}