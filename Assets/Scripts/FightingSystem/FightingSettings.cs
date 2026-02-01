using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "FightSettings", menuName = "ScriptableObjects/FightSettings")]
public class FightSettings : ScriptableObject
{
    private static FightSettings instance = null;
    public static FightSettings Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<FightSettings>("FightSettings");
            return instance;
        }
    }

    [SerializeField] private List<Emotion> emotions;

    private Dictionary<EEmotion, Emotion> emotionFromEmotionType;
    public Dictionary<EEmotion, Emotion> EmotionFromEmotionType
    {
        get
        {
            if (emotionFromEmotionType == null)
                emotionFromEmotionType = emotions.ToDictionary(item => item.EmotionType, item => item);
            return emotionFromEmotionType;
        }
    }

    public List<Emotion> GetEmotionsList() => new List<Emotion>();

    public Emotion GetEmotionFromEmotionType(EEmotion emotionType)
    {
        return EmotionFromEmotionType.GetValueOrDefault(emotionType, null);
    }
}
