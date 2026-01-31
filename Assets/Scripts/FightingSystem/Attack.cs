
using System.Collections.Generic;

public class Attack
{
    private Enemy from;
    private List<EmotionStats> emotionStats;

    public Attack(Enemy from, List<EmotionStats> emotionStats)
    {
        this.from = from;
        this.emotionStats = emotionStats;
    }
}