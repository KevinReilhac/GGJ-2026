
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class Emotion
{
    [SerializeField] private EEmotion emotionType;
    [SerializeField] private string name;
    [SerializeField] private int colorPaletteIndex;

    public EEmotion EmotionType => emotionType;
    public string Name => name;
    public int ColorPaletteIndex => colorPaletteIndex;
}