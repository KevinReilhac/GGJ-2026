using System;
using System.Collections;
using System.Collections.Generic;
using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using UnityEngine;

public class FightRoom : MonoBehaviour
{
    [SerializeField] private List<Fight> fights;
    [SerializeField] private Transform fightContainer;

    private Fight currentFightInstance;

    void Awake()
    {
        FightManager.OnExitFight += OnExitFight;
    }

    private void OnExitFight()
    {
        if (currentFightInstance != null)
            GameObject.Destroy(currentFightInstance);
    }

    public void StartRandomFight()
    {
        Fight fightPrefab = fights.GetRandom();

        currentFightInstance = Instantiate(fightPrefab);
        currentFightInstance.transform.SetParent(fightContainer);
        currentFightInstance.transform.localPosition = Vector3.zero;

        FightManager.StartFight(currentFightInstance);
    }
}
