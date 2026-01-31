using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AFilterHandler<T> where T : Object
{
    [System.Serializable]
    public class FilterHandlerItem
    {
        public EFilter filterType;
        public T item;
    }

    [SerializeField] private List<FilterHandlerItem> filterHandlerItems;

    private Dictionary<EFilter, T> filterHandlerItemsDict;
    public Dictionary<EFilter, T> FilterHandlerItemsDict
    {
        get
        {
            if (filterHandlerItems == null)
                filterHandlerItemsDict = filterHandlerItems.ToDictionary(item => item.filterType, item => item.item);
            return filterHandlerItemsDict;
        }
    }

    protected virtual void Awake()
    {
        FilterManager.OnFilterChanged += OnFilterChanged;
    }

    protected void OnFilterChanged(Filter filter, int filterIndex)
    {
        if (filterHandlerItemsDict.TryGetValue(filter.FilterType, out T item))
            Apply(item);
    }

    protected abstract void Apply(T item);

    protected void OnDestroy()
    {
        FilterManager.OnFilterChanged -= OnFilterChanged;
    }
}