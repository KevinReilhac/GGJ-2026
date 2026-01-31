using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FilterSettings", menuName = "ScriptableObjects/FilterSettings")]
public class FilterSettings : ScriptableObject
{
    private static FilterSettings instance = null;
    public static FilterSettings Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<FilterSettings>("FilterSettings");
            return instance;
        }
    }

    [SerializeField] private List<Filter> filters;

    public List<Filter> GetFilters() => new List<Filter>(filters);
}