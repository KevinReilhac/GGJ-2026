
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FilterManager
{
    public delegate void OnFilterChangedDelegate(Filter filter, int filterIndex);
    public static event OnFilterChangedDelegate OnFilterChanged;


    private static List<Filter> _filters;
    private static List<Filter> Filters
    {
        get
        {
            if (_filters == null)
                _filters = FilterSettings.Instance.GetFilters();
            return _filters;
        }
    }

    private static Dictionary<EFilter, Filter> _filterFromFilterType;
    private static Dictionary<EFilter, Filter> FilterFromFilterType
    {
        get
        {
            if (_filterFromFilterType == null)
                _filterFromFilterType = Filters.ToDictionary(item => item.FilterType, item => item);
            return _filterFromFilterType;
        }
    }
    private static Filter _currentFilter;
    public static Filter CurrentFilter
    {
        get => _currentFilter;
        set
        {
            if (_currentFilter == value)
                return;
            _currentFilter = value;
            OnFilterChanged?.Invoke(_currentFilter, Filters.IndexOf(_currentFilter));
        }
    }

    public static int CurrentFilterIndex => Filters.IndexOf(_currentFilter);

    public static void SetFilter(int index)
    {
        if (index < 0 || index >= Filters.Count)
        {
            Debug.LogErrorFormat("There is no filter at index {0}", index);
            return;
        }
        CurrentFilter = Filters[index];
    }

    public static void SetFilter(EFilter filterType)
    {
        if (FilterFromFilterType.TryGetValue(filterType, out Filter filter))
        {
            CurrentFilter = filter;
        }
        else
        {
            Debug.LogErrorFormat("there is no filter with {0} as filterType", filterType);
        }
    }

    public static void SetFilter(Filter filter)
    {
        if (!Filters.Contains(filter))
        {
            Debug.LogError("Out of filter settings filter");
            return;
        }
        CurrentFilter = filter;
    }
}