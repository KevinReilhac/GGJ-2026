using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTestScene : MonoBehaviour
{
    [SerializeField] private Fight fight;

    private void Start()
    {
        FightManager.StartFight(fight);
    }
}
