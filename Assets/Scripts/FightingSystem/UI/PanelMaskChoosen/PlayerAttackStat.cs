using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackStat : MonoBehaviour
{
    public event Action<int> OnValueChanged;
    [SerializeField] private PlayerAttackToggle toggleTemplate;
    [SerializeField] private Color togglesColor;


    private List<PlayerAttackToggle> toggles = new List<PlayerAttackToggle>();
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
        }
    }

    private void Awake()
    {
        toggleTemplate.gameObject.SetActive(false);
    }

    public void SetToggles(int count, bool selectable)
    {
        ClearToggles();
        for (int i = 0; i < count; i ++)
            CreateToggle(selectable, false);
    }

    private void ClearToggles()
    {
        foreach (PlayerAttackToggle toggle in toggles)
        {
            GameObject.Destroy(toggle);
        }

        toggles.Clear();
    }

    private PlayerAttackToggle CreateToggle(bool selectable, bool value)
    {
        PlayerAttackToggle newToggle = Instantiate(toggleTemplate);

        newToggle.SetColor(togglesColor);
        newToggle.SetValue(value, false);
        newToggle.transform.SetParent(toggleTemplate.transform.parent);
        newToggle.gameObject.SetActive(true);
        toggles.Add(newToggle);

        return newToggle;
    }

}