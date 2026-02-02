using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
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
            panel.Hide(animate);

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

    public void HideAllPanels(bool hideTextPanel = false, bool animate = true)
    {
        foreach (Panel panel in panels)
        {
            if (hideTextPanel)
                panel.Hide(animate);
            else if (!panel.GetType().IsAssignableFrom(typeof(StateTextPanel)))
                panel.Hide(animate);
        }
    }
    #endregion

    [SerializeField] private CanvasGroup selectMaskTip;

    private void Awake()
    {
        HideAllPanels(true, false);
        selectMaskTip.gameObject.SetActive(true);
        selectMaskTip.alpha = 0f;

        FightManager.OnStartFight += OnStartFight;
        FightManager.OnWinFight += OnWinFight;
        FightManager.OnExitFight += OnExitOrGameOver;
        FightManager.OnGameOver += OnExitOrGameOver;
        FightManager.OnNextEnemyAttack += OnNextEnemyAttack;
        FightManager.OnSelectMask += OnMaskSelected;
    }


    void OnDestroy()
    {
        FightManager.OnStartFight -= OnStartFight;
        FightManager.OnWinFight -= OnWinFight;
        FightManager.OnExitFight -= OnExitOrGameOver;
        FightManager.OnGameOver -= OnExitOrGameOver;
        FightManager.OnNextEnemyAttack -= OnNextEnemyAttack;
        FightManager.OnSelectMask -= OnMaskSelected;
    }
    private void OnNextEnemyAttack(Attack attack)
    {
        ShowSelectMaskTip();
        GetPanel<EnemyPanel>().ShowAndSetup(attack);
        if (PlayerFighter.Instance.Masks.Count == 0)
        {
            FightManager.SelectMask(null);
        }
        else
        {
            HideChoosenMaskPanel();
            SetupAndShowMaskChoicePanel();
        }
    }

    private void ShowSelectMaskTip()
    {
        selectMaskTip.DOFade(1f, 0.5f).SetEase(Ease.OutQuint);
    }

    private void HideSelectMaskTip()
    {
        selectMaskTip.DOFade(0f, 0.5f).SetEase(Ease.InQuint);
    }

    private void OnExitOrGameOver()
    {
        HideAllPanels();
    }

    private void OnStartFight(Fight fight)
    {
        GetPanel<StateTextPanel>().SetupAndShow("Début du combat");
    }

    private void OnMaskSelected(Mask mask)
    {
        HideSelectMaskTip();
        ShowChoosenMaskPanel();
        HideMaskChoicePanel();
    }

    private void ShowChoosenMaskPanel()
    {
        ChoosenMaskPanel choosenMaskPanel = GetPanel<ChoosenMaskPanel>();

        choosenMaskPanel.SetupAndShow(PlayerFighter.Instance, FightManager.PlayerAttack);
        choosenMaskPanel.OnHoverCardsEvent += ShowMaskChoicePanel;
        choosenMaskPanel.OnUnhoverCardsEvent += HideMaskChoicePanel;
    }


    private void HideChoosenMaskPanel()
    {
        ChoosenMaskPanel choosenMaskPanel = GetPanel<ChoosenMaskPanel>();

        choosenMaskPanel.Hide();
        choosenMaskPanel.OnHoverCardsEvent -= ShowMaskChoicePanel;
        choosenMaskPanel.OnUnhoverCardsEvent -= HideMaskChoicePanel;
    }

    private void ShowMaskChoicePanel()
    {
        ShowPanel<MaskChoicePanel>();
    }
    private void SetupAndShowMaskChoicePanel()
    {
        GetPanel<MaskChoicePanel>().SetupAndShow(PlayerFighter.Instance.Masks, FightManager.SelectMask);
    }

    private void HideMaskChoicePanel()
    {
        HidePanel<MaskChoicePanel>();
    }

    private async void OnWinFight(Fight fight)
    {
        HidePanel<ChoosenMaskPanel>();
        HidePanel<MaskChoicePanel>(false);
        HidePanel<EnemyPanel>();

        GetPanel<StateTextPanel>().SetupAndShow("Victoire");
        await Task.Delay(2000);

        MaskDropPanel maskDropPanel = GetPanel<MaskDropPanel>();
        maskDropPanel.Setup(fight.GetMaskList(), FightManager.SelectDroppedMask);
        maskDropPanel.Show();
    }
}
