using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Mask
{
    [SerializeField] private List<EmotionStat> maskStats;
    private Dictionary<EEmotion, int> statsDict;
    private Emotion mainEmotion = null;

    public Mask(List<EmotionStat> maskStats)
    {
        statsDict = maskStats.ToDictionary(ms => ms.emotionType, ms => ms.stat);
        this.maskStats = maskStats;
        mainEmotion = FindMainEmotion();
    }

    public Mask(int joy = 0, int sad = 0, int angry = 0, int disgust = 0, int scare = 0)
    {
        maskStats = new List<EmotionStat>();

        maskStats.Add(new EmotionStat(EEmotion.Joy, joy));
        maskStats.Add(new EmotionStat(EEmotion.Sad, sad));
        maskStats.Add(new EmotionStat(EEmotion.Angry, angry));
        maskStats.Add(new EmotionStat(EEmotion.Disgust, disgust));
        maskStats.Add(new EmotionStat(EEmotion.Scare, scare));

        statsDict = maskStats.ToDictionary(ms => ms.emotionType, ms => ms.stat);
        mainEmotion = FindMainEmotion();
    }

    private Emotion FindMainEmotion()
    {
        EEmotion highestEmotion = EEmotion.Angry;
        int highestStat = -1;

        foreach (EmotionStat maskStat in maskStats)
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

    public override string ToString()
    {
        StringBuilder strBuilder = new StringBuilder();

        foreach (var stat in Stats)
            strBuilder.AppendLineFormat("{0} : {1}", stat.emotionType.ToString(), stat.stat);
        
        return strBuilder.ToString();
    }

    public Emotion MainEmotion => mainEmotion;
    public List<EmotionStat> Stats => new List<EmotionStat>(maskStats);
}