using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Filter
{
    [SerializeField] private EFilter filterType;
    [SerializeField] private Color color;

    public Color Color => color;
    public EFilter FilterType => filterType;
}