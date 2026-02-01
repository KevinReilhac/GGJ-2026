using System;
using System.Collections;
using System.Collections.Generic;
using FoxEdit;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightHUD : MonoBehaviour
{
    #region Panel
    [SerializeField] private List<Panel> panels;

    private Dictionary<Type, Panel> panelsDict;
    public Dictionary<Type, Panel> PanelsDict
    {
        get
        {
            if (panelsDict == null)
                panelsDict = GetPanelDict();
            return panelsDict;
        }
    }

    private Dictionary<Type, Panel> GetPanelDict()
    {
        Dictionary<Type, Panel> dict = new Dictionary<Type, Panel>();
        foreach (Panel panel in panels)
        {
            Type type = panel.GetType();
            if (!dict.ContainsKey(type))
                dict.Add(type, panel);
        }

        return dict;
    }

    public T ShowPanel<T>(bool animate = true) where T : Panel
    {
        Panel panel = GetPanel<T>();

        if (panel != null)
            panel.Show();

        return (T)panel;
    }

    public T HidePanel<T>(bool animate = true) where T : Panel
    {
        Panel panel = GetPanel<T>();

        if (panel != null)
            panel.Hide();

        return (T)panel;
    }

    public T GetPanel<T>() where T : Panel
    {
        if (PanelsDict.TryGetValue(typeof(T), out Panel panel))
        {
            return (T)panel;
        }

        return null;
    }

    public void HideAllPanels()
    {
        foreach (Panel panel in panels)
        {
            panel.Hide();
        }
    }
    #endregion

    private void Awake()
    {
        HideAllPanels();

        FightManager.OnStartMaskSelection += OnStartFight;
        FightManager.OnWinFight += OnWinFight;
        FightManager.OnExitFight += HideAllPanels;
        FightManager.OnGameOver += HideAllPanels;
    }


    void OnDestroy()
    {
        FightManager.OnStartMaskSelection -= OnStartFight;
        FightManager.OnWinFight -= OnWinFight;
        FightManager.OnExitFight -= HideAllPanels;
        FightManager.OnGameOver -= HideAllPanels;
    }

    private void OnStartFight()
    {
        GetPanel<StateTextPanel>().SetupAndShow("Début du combat");
        StartMaskSelection(true);
    }


    private void StartMaskSelection(bool firstTurn)
    {
        HideAllPanels();
        PlayerFighter.Instance.ClearMasks();
        if (PlayerFighter.Instance.Masks == null || PlayerFighter.Instance.Masks.Count == 0)
        {
            FightManager.SelectMask(null);
            ShowFightPanels();
        }
        else
        {
            MaskChoicePanel maskChoicePanel = GetPanel<MaskChoicePanel>();

            maskChoicePanel.Setup(PlayerFighter.Instance.Masks, OnMaskSelected);
            maskChoicePanel.Show();
        }

        ShowEnemyPanel();
    }

    private void OnMaskSelected(Mask mask)
    {
        FightManager.SelectMask(mask);
        HidePanel<MaskChoicePanel>();
        ShowFightPanels();
    }
    
    private void ShowEnemyPanel()
    {
        EnemyPanel enemyPanel = GetPanel<EnemyPanel>();
        enemyPanel.ShowAndSetup(FightManager.CurrentEnemyAttack);
    }

    private void ShowFightPanels()
    {
        ChoosenMaskPanel choosenMaskPanel = GetPanel<ChoosenMaskPanel>();
        choosenMaskPanel.Setup(PlayerFighter.Instance, NextTurn, FightManager.PlayerAttack);
        choosenMaskPanel.Show();
    }

    private void NextTurn()
    {
        StartMaskSelection(false);
    }

    private void OnWinFight(Fight fight)
    {
        HidePanel<ChoosenMaskPanel>();
        HidePanel<EnemyPanel>();

        MaskDropPanel maskDropPanel = GetPanel<MaskDropPanel>();
        maskDropPanel.Setup(fight.GetMaskList(), FightManager.SelectDroppedMask);
        maskDropPanel.Show();
    }
}
