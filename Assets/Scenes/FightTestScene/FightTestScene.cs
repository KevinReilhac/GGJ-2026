using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTestScene : MonoBehaviour
{
    [SerializeField] private Fight fight;

    void Awake()
    {
        FightManager.StartFight(fight);
    }
}
