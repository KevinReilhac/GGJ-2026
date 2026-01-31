
using System.Collections.Generic;
using FoxEdit;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHp = 2;
    [SerializeField] private int difficulty = 3;
    [SerializeField] private Mask droppedMask;
    [Header("References")]
    [SerializeField] private VoxelRenderer maskVoxelRenderer;


    public void Setup(Mask droppedMask, int difficulty)
    {
        this.droppedMask = droppedMask;
        this.difficulty = difficulty;

        maskVoxelRenderer.SetPalette(droppedMask.MainEmotion.ColorPaletteIndex);
    }

    public Attack GetNextAttack()
    {
        Dictionary<EEmotion, int> statsDict = new Dictionary<EEmotion, int>();
        int pointsToGive = difficulty;
        EEmotion tmpEmotion;

        while(pointsToGive > 0)
        {
            tmpEmotion = (EEmotion)Random.Range(0, 4);
            if (!statsDict.ContainsKey(tmpEmotion))
                statsDict.Add(tmpEmotion, 0);
            statsDict[tmpEmotion]++;
            
            pointsToGive--;
        }

        List<EmotionStats> emotionStats = new List<EmotionStats>();
        foreach (KeyValuePair<EEmotion, int> item in statsDict)
            emotionStats.Add(new EmotionStats(item.Key, item.Value));
        
        return new Attack(this, emotionStats);
    }

    public void Hit()
    {
        maxHp--;
        if (maxHp == 0)
            Die();
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}