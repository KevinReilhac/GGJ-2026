using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine;
using JetBrains.Annotations;

[RequireComponent(typeof(Button))]
public class UIButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private TextMeshProUGUI text;
    [Header("Audio")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioMixerGroup menuMixerGroup;
    private AudioSource audioSource;
    public AudioMixer masterMixer;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = menuMixerGroup;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.name == "TextPlay")
        {
            text.text = "> JOUER";
        }
        else if (eventData.pointerEnter.name == "TextQuit")
        {
            text.text = "> QUITTER";
        }
        if (hoverSound != null && GetComponent<Button>().interactable)
            audioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.name == "TextPlay")
        {
            text.text = "JOUER";
        }
        else if (eventData.pointerEnter.name == "TextQuit")
        {
            text.text = "QUITTER";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null && GetComponent<Button>().interactable)
        {
            MuteMusic();
            audioSource.PlayOneShot(clickSound);
        }
    }
    
    public void MuteMusic()
    {
        masterMixer.SetFloat("BGMVolume", -80f);
    }
}