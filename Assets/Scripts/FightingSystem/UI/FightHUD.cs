using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightHUD : MonoBehaviour
{
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

    void Start()
    {
        HideAllPanels();
        ShowPanel<MaskChoicePanel>();
    }

}
