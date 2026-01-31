using UnityEngine;
using TMPro;

public class StatsDisplayer : MonoBehaviour
{
    [SerializeField] private EEmotion emotion;
    [SerializeField] private TextMeshProUGUI valueText;

    public EEmotion Emotion => emotion;

    public void SetValue(int value)
    {
        valueText.text = value.ToString();
    }
}