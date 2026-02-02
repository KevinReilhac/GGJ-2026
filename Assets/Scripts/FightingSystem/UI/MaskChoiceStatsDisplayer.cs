
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaskChoiceStatsDisplayer : MonoBehaviour
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

    public void SetValue(Mask mask)
    {
        ResetValues();
        bool isDamaged;
        foreach (EmotionStat emotionStat in mask.Stats)
        {
            if (StatsDisplayerDict.TryGetValue(emotionStat.emotionType, out StatDisplayer statsDisplayer))
            {
                isDamaged = mask.IsEmotionDamaged(emotionStat.emotionType);
                statsDisplayer.SetValue(emotionStat.stat, isDamaged ? FightSettings.Instance.BadColor : null, !isDamaged);
            }
        }
    }

    private void ResetValues()
    {
        foreach (StatDisplayer statDisplayer in statsDisplayers)
            statDisplayer.SetValue(0);
    }
}
