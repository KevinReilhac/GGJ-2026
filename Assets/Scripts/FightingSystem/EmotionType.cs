using System.Collections;
using System.Collections.Generic;

public enum EEmotion
{
    Joy,
    Sad,
    Angry,
    Disgust,
    Scare,
    Multiple = 255
}

public static class EEmotionUtility
{
    public static readonly List<EEmotion> EmotionsList = new List<EEmotion>()
    {
        EEmotion.Joy,
        EEmotion.Sad,
        EEmotion.Angry,
        EEmotion.Disgust,
        EEmotion.Scare,
    };
}