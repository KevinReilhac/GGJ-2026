
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
    //[SerializeField] private VoxelRenderer maskVoxelRenderer;
    [SerializeField] private Image hpBarFill;
    [SerializeField] private Gradient gradient;

    private int _hp;
    public int HP
    {
        get => _hp;
        set
        {
            _hp = value;
            float percents =  (float)_hp / maxHp;
            hpBarFill.fillAmount =  percents;
            hpBarFill.color = gradient.Evaluate(percents);
            OnHPChanged?.Invoke(_hp);
            Debug.Log("HP:" + HP);
            if (_hp == 0)
                Die();
        }
    }

    public void Reset()
    {
        HP = maxHp;
        gameObject.SetActive(true);
    }

    public void Setup(Mask droppedMask, int difficulty)
    {
        this.droppedMask = droppedMask;
        this.difficulty = difficulty;

        //maskVoxelRenderer.SetPalette(droppedMask.MainEmotion.ColorPaletteIndex);
    }

    public Attack GetNextAttack()
    {
        Dictionary<EEmotion, int> statsDict = new Dictionary<EEmotion, int>();
        int pointsToGive = difficulty;
        EEmotion tmpEmotion;

        while(pointsToGive > 0)
        {
            tmpEmotion = (EEmotion)UnityEngine.Random.Range(0, 5);
            if (!statsDict.ContainsKey(tmpEmotion))
                statsDict.Add(tmpEmotion, 0);
            statsDict[tmpEmotion]++;
            
            pointsToGive--;
        }

        List<EmotionStat> emotionStats = new List<EmotionStat>();
        foreach (KeyValuePair<EEmotion, int> item in statsDict)
            emotionStats.Add(new EmotionStat(item.Key, item.Value));
        
        return new Attack(emotionStats);
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