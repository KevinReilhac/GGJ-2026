using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Ease animEase = Ease.InQuart;
    [SerializeField] private float animTime = 1f;
    [SerializeField] private CanvasGroup mainCanvas;
    [SerializeField] private CanvasGroup gameoverCanvas;
    [SerializeField] private CanvasGroup textCanvas;

    private bool inputsActive = false;

    void Awake()
    {
        FightManager.OnGameOver += StartAnim;

        mainCanvas.alpha = 0;
        mainCanvas.interactable = false;
        gameoverCanvas.alpha = 0;
        textCanvas.alpha = 0;
    }

    void OnDestroy()
    {
        FightManager.OnGameOver -= StartAnim;
    }

    private void StartAnim()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(mainCanvas.DOFade(1f, animTime).SetEase(animEase));
        sequence.Append(gameoverCanvas.DOFade(1f, animTime).SetEase(animEase));
        sequence.Append(textCanvas.DOFade(1f, animTime).SetEase(animEase));
        sequence.JoinCallback(() => inputsActive = true);

        sequence.Play();
    }

    void Update()
    {
        if (!inputsActive)
            return;
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            inputsActive = false;
            textCanvas.alpha = 0;
        }
        else if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("StartMenu");
            inputsActive = false;
            textCanvas.alpha = 0;
        }
        else if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }
    }


}
