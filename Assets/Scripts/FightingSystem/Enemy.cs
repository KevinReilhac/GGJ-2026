
using System;
using System.Collections.Generic;
using FoxEdit;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public event Action<int> OnHPChanged;
    [Header("Stats")]
    [SerializeField] private int maxHp = 2;
    [SerializeField] private int difficulty = 3;
    public Mask droppedMask;
    [Header("References")]
    [SerializeField] private Image hpBarFill;
    [SerializeField] private Gradient gradient;
    [SerializeField] private SimpleVoxelRenderer simpleVoxelRenderer;

    private int _hp;
    public int HP
    {
        get => _hp;
        set
        {
            _hp = value;
            float percents = (float)_hp / (float)maxHp;
            hpBarFill.fillAmount = percents;
            hpBarFill.color = gradient.Evaluate(percents);
            OnHPChanged?.Invoke(_hp);
            if (_hp == 0)
                Die();
        }
    }

    public void SetMaxHP(int maxHp)
    {
        this.maxHp = maxHp;
        HP = this.maxHp;
    }

    private void Awake()
    {
    }

    public void SetupMask(Mask mask)
    {
        droppedMask = mask;
        simpleVoxelRenderer.SetPaletteIndex(droppedMask.MainEmotion.ColorPaletteIndex);
    }

    public void Hit()
    {
        HP--;
        if (HP == 0)
            Die();
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    public bool IsDead => HP <= 0;
}