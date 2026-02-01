using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _amplitude = 1.0f;

    private Vector3 _basePosition = Vector3.zero;
    private void Awake()
    {
        _basePosition = transform.position;
    }

    private void Update()
    {
        transform.position = _basePosition + new Vector3(0.0f, (Mathf.Sin(Time.time * _speed) + 1.0f) * 0.5f * _amplitude, 0.0f);
    }
}
