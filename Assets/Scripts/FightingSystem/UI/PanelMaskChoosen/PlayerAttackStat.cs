using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackStat : MonoBehaviour
{
    public event Action<int> OnValueChanged;
    [SerializeField] private PlayerAttackToggle toggleTemplate;
    [SerializeField] private Color togglesColor;


    private List<PlayerAttackToggle> toggles = new List<PlayerAttackToggle>();
    private int _value = 0;
    public int Value
    {
        get
        {
            int index = 0;
            foreach (PlayerAttackToggle playerAttackToggle in toggles)
            {
                if (playerAttackToggle.Value)
                    index++;
            }
            return index;
        }
        set
        {
            for (int i = 0; i < toggles.Count; i++)
                toggles[i].SetValue(i < value);
            OnValueChanged?.Invoke(value);
        }
    }

    private void Awake()
    {
        toggleTemplate.gameObject.SetActive(false);
    }

    public void SetToggles(int count, bool selectable)
    {
        ClearToggles();
        for (int i = 0; i < count; i++)
            CreateToggle(selectable, false, i);
    }

    private void ClearToggles()
    {
        foreach (PlayerAttackToggle toggle in toggles)
        {
            GameObject.Destroy(toggle.gameObject);
        }

        toggles.Clear();
    }

    private PlayerAttackToggle CreateToggle(bool selectable, bool value, int index)
    {
        PlayerAttackToggle newToggle = Instantiate(toggleTemplate);

        newToggle.SetColor(togglesColor);
        newToggle.Setup(index, value, OnClickToggle);
        newToggle.SetSelectable(selectable);
        newToggle.transform.SetParent(toggleTemplate.transform.parent);
        newToggle.gameObject.SetActive(true);
        newToggle.transform.localScale = Vector3.one;
        toggles.Add(newToggle);

        return newToggle;
    }

    private void OnClickToggle(int index)
    {
        if (index == Value - 1)
        {
            Value = Value - 1;
        }
        else
        {
            Value = index + 1;
        }
    }

    public void DisableFalseToggles()
    {
        foreach (PlayerAttackToggle toggle in toggles)
        {
            toggle.SetSelectable(toggle.Value);
        }
    }

    public void EnableAllToggles()
    {
        foreach (PlayerAttackToggle toggle in toggles)
        {
            toggle.SetSelectable(true);
        }
    }

}