using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatDisplayer : MonoBehaviour
{
    [SerializeField] private EEmotion emotion;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Image icon;

    public EEmotion Emotion => emotion;

    public void SetValue(int value)
    {
        valueText.text = value.ToString();
        gameObject.SetActive(value > 0);
    }
}