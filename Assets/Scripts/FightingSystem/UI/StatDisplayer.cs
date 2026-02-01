using UnityEngine;
using TMPro;

public class StatDisplayer : MonoBehaviour
{
    [SerializeField] private EEmotion emotion;
    [SerializeField] private TextMeshProUGUI valueText;

    public EEmotion Emotion => emotion;

    public void SetValue(int value)
    {
        valueText.text = value.ToString();
    }
}