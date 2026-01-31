using UnityEngine;

public class SetFilterOnStart : MonoBehaviour
{
    [SerializeField] private EFilter filterType = EFilter.Red;

    public void Start()
    {
        FilterManager.SetFilter(filterType);
    }
}