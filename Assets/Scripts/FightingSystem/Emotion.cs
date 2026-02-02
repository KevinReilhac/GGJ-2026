
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class Emotion
{
    [SerializeField] private EEmotion emotionType;
    [SerializeField] private string name;
    [SerializeField] private int colorPaletteIndex;
    [SerializeField] private Color color;

    public EEmotion EmotionType => emotionType;
    public string Name => name;
    public int ColorPaletteIndex => colorPaletteIndex;
    public Color Color => color;

}