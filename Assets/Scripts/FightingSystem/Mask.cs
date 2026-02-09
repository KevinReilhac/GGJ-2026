using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Mask
{
    private Dictionary<EEmotion, int> StatsDict = new Dictionary<EEmotion, int>();
    private Emotion mainEmotion = null;

    private List<EEmotion> damagedEmotion = new List<EEmotion>();

    public void DamageMask(EEmotion emotionType, int value)
    {
        if (damagedEmotion == null)
            damagedEmotion = new List<EEmotion>();
        if (value == 0)
            return;
        if (StatsDict.ContainsKey(emotionType))
        {

            if (!IsEmotionDamaged(emotionType))
                damagedEmotion.Add(emotionType);
            StatsDict[emotionType] -= value;
            //StatsDict[emotionType] = Mathf.Max(0, StatsDict[emotionType] - value);
        }
    }

    public bool IsEmotionDamaged(EEmotion emotionType)
    {
        if (damagedEmotion == null)
            return false;
        return damagedEmotion.Contains(emotionType);
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
            StatsDict[eEmotion] += value;
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

    public List<EmotionStat> GetEmotionStatsList()
    {
        List<EmotionStat> emotionStats = new List<EmotionStat>();

        foreach (var item in StatsDict)
            emotionStats.Add(new EmotionStat(item.Key, item.Value));

        return emotionStats;
    }

    public Emotion MainEmotion
    {
        get
        {
            if (mainEmotion == null)
                mainEmotion = EmotionStat.GetMainEmotion(GetEmotionStatsList());
            return mainEmotion;
        }
    }

    public void SetMainEmotion()
    {
        mainEmotion = EmotionStat.GetMainEmotion(GetEmotionStatsList());
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