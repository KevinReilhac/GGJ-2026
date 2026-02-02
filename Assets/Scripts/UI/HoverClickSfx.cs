using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverClickSound : MonoBehaviour,
    IPointerEnterHandler,
    IPointerClickHandler,
    IPointerExitHandler
{
    [SerializeField] AudioClip hoverClip;
    [SerializeField] AudioClip unhoverClip;
    [SerializeField] AudioClip clickClip;

    static AudioSource sharedSource;

    void Awake()
    {
        EnsureAudioSource();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Play(hoverClip);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Play(clickClip);
    }

    void Play(AudioClip clip)
    {
        if (clip == null) return;

        EnsureAudioSource();
        sharedSource.PlayOneShot(clip);
    }

    static void EnsureAudioSource()
    {
        if (sharedSource != null) return;

        GameObject go = new GameObject("UI_AudioSource");
        DontDestroyOnLoad(go);

        sharedSource = go.AddComponent<AudioSource>();
        sharedSource.playOnAwake = false;
        sharedSource.outputAudioMixerGroup = FightSettings.Instance.UIMixerGroup;
        sharedSource.spatialBlend = 0f; // 2D UI
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Play(unhoverClip);
    }
}