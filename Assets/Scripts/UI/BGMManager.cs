using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FoxEdit;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private AudioClip fightMusic;
    [SerializeField] private AudioSource sourceA;
    [SerializeField] private AudioSource sourceB;
    [SerializeField] float fadeTime = 1.5f;

    bool usingA = true;

    private void Awake()
    {
        sourceA.volume = 1f;
        sourceB.volume = 0f;
        sourceA.Play();
        FightManager.OnStartFight += PlayFightMusic;
        FightManager.OnExitFight += PlayBackgroundMusic;
    }

    void OnDestroy()
    {
        FightManager.OnStartFight -= PlayFightMusic;
        FightManager.OnExitFight -= PlayBackgroundMusic;
    }

    private void PlayBackgroundMusic()
    {
        CrossFade(bgMusic);
    }

    private void PlayFightMusic(Fight fight)
    {
        CrossFade(fightMusic);
    }

    public void CrossFade(AudioClip newClip)
    {
        AudioSource from = usingA ? sourceA : sourceB;
        AudioSource to = usingA ? sourceB : sourceA;

        to.clip = newClip;
        to.volume = 0f;
        to.Play();

        from.DOFade(0f, fadeTime);
        to.DOFade(1f, fadeTime);

        usingA = !usingA;
    }
}
