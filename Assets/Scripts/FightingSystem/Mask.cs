using System.Collections.Generic;
using System.Linq;

public partial class Mask
{
    private List<EmotionStats> maskStats;
    private Dictionary<EEmotion, int> statsDict;
    private Emotion mainEmotion = null;

    public Mask(List<EmotionStats> maskStats)
    {
        statsDict = maskStats.ToDictionary(ms => ms.emotionType, ms => ms.stat);
        this.maskStats = maskStats;
        mainEmotion = FindMainEmotion();
    }

    private Emotion FindMainEmotion()
    {
        EEmotion highestEmotion = EEmotion.Angry;
        int highestStat = -1;

        foreach (EmotionStats maskStat in maskStats)
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

    public Emotion MainEmotion => mainEmotion;
    public List<EmotionStats> Stats = new List<EmotionStats>();
}