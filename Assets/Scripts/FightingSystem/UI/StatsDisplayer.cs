using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsDisplayer : MonoBehaviour
{
    [SerializeField] private List<StatDisplayer> statsDisplayers ;
    private Dictionary<EEmotion, StatDisplayer> statsDiplayerDict = null;
    private Dictionary<EEmotion, StatDisplayer> StatsDisplayerDict
    {
        get
        {
            if (statsDiplayerDict == null)
                statsDiplayerDict = statsDisplayers.ToDictionary(s => s.Emotion, s => s);
            return statsDiplayerDict;
        }
    }

    public void SetValue(List<EmotionStat> emotionStats)
    {
        ResetValues();
        foreach (EmotionStat emotionStat in emotionStats)
        {
            if (StatsDisplayerDict.TryGetValue(emotionStat.emotionType, out StatDisplayer statsDisplayer))
                statsDisplayer.SetValue(emotionStat.stat);
        }
    }

    private void ResetValues()
    {
        foreach (StatDisplayer statDisplayer in statsDisplayers)
            statDisplayer.SetValue(0);
    }
}
