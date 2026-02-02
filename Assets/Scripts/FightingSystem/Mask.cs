using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Mask
{
    [SerializeField] private List<EmotionStat> maskStats;
    private Dictionary<EEmotion, int> _statsDict;
    private Dictionary<EEmotion, int> StatsDict
    {
        get
        {
            if (_statsDict == null)
                _statsDict = maskStats.ToDictionary(ms => ms.emotionType, ms => ms.stat);
            return _statsDict;
        }
    }
    private Emotion mainEmotion = null;

    public Mask(List<EmotionStat> maskStats)
    {
        this.maskStats = maskStats;
    }

    public Mask(int joy = 0, int sad = 0, int angry = 0, int disgust = 0, int scare = 0)
    {
        maskStats = new List<EmotionStat>
        {
            new EmotionStat(EEmotion.Joy, joy),
            new EmotionStat(EEmotion.Sad, sad),
            new EmotionStat(EEmotion.Angry, angry),
            new EmotionStat(EEmotion.Disgust, disgust),
            new EmotionStat(EEmotion.Scare, scare)
        };

    }

    public void DamageMask(EEmotion eEmotion, int value)
    {
        if (StatsDict.ContainsKey(eEmotion))
        {
            Debug.LogFormat("Damage {0} by {1} on mask", eEmotion, value);
            StatsDict[eEmotion] -= value;
        }
    }

    public override string ToString()
    {
        StringBuilder strBuilder = new StringBuilder();

        foreach (var stat in Stats)
            strBuilder.AppendLineFormat("{0} : {1}", stat.emotionType.ToString(), stat.stat);

        return strBuilder.ToString();
    }

    public int GetStat(EEmotion eEmotion)
    {
        if (StatsDict.ContainsKey(eEmotion))
            return StatsDict[eEmotion];
        return 0;
    }

    public void AddStat(EEmotion eEmotion, int value)
    {
        if (StatsDict.ContainsKey(eEmotion))
            StatsDict[eEmotion] -= value;
        else
            StatsDict.Add(eEmotion, value);
    }

    public bool IsMaskEmpty()
    {
        foreach (var item in StatsDict)
        {
            if (item.Value > 0)
                return false;
        }

        return true;
    }

    public Emotion MainEmotion
    {
        get
        {
            if (mainEmotion == null)
                mainEmotion = EmotionStat.GetMainEmotion(maskStats);
            return mainEmotion;
        }
    }
    public List<EmotionStat> Stats
    {
        get
        {
            List<EmotionStat> emotionStats = new List<EmotionStat>();
            foreach (var item in StatsDict)
                emotionStats.Add(new EmotionStat(item.Key, item.Value));
            return emotionStats;
        }
    }
}