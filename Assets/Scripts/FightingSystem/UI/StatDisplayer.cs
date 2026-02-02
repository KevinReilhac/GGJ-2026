using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatDisplayer : MonoBehaviour
{
    [SerializeField] private EEmotion emotion;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Image icon;

    public EEmotion Emotion => emotion;

    private Color? defaultColor;
    private Color DefaultColor
    {
        get
        {
            if (!defaultColor.HasValue)
                defaultColor = valueText.color;
            return defaultColor.Value;
        }
    }

    public void SetValue(int value, Color? customColor = null, bool hideIfZero = true)
    {
        valueText.color = customColor.HasValue ? customColor.Value : DefaultColor;
        valueText.text = value.ToString();
        if (hideIfZero)
            gameObject.SetActive(value > 0);
        else
            gameObject.SetActive(true);
    }
}