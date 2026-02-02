using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FoxEdit;
using UnityEngine;
using UnityEngine.Audio;

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
    [SerializeField] private Color badColor = Color.red;
    [SerializeField] private Color goodColor = Color.green;
    public AudioMixerGroup UIMixerGroup;
    public VoxelObject mask;

    public Color GoodColor => goodColor;
    public Color BadColor => badColor;
    public Emotion GetEmotionFromEmotionType(EEmotion emotionType)
    {
        return EmotionFromEmotionType.GetValueOrDefault(emotionType, null);
    }


}
