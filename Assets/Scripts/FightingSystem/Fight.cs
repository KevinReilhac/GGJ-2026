using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private Transform cameraPosition;

    public List<Enemy> GetEnemyList() => new List<Enemy>();

    public void StartFight()
    {
        FightManager.StartFight(this);
    }
}