using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOscillation : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _amplitude = 1.0f;

    private float _baseIntensity = 0.0f;
    private Light _light = null;
    private float _timer = 0.0f;
    private float _randomIntensity = 0.0f;

    private void Awake()
    {
        _light = GetComponent<Light>();
        _baseIntensity = _light.intensity;
        _randomIntensity = Random.Range(0.9f, 1.1f);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 0.2f)
        {
            _timer -= 0.2f;
            _randomIntensity = Random.Range(0.9f, 1.1f);
        }
        _light.intensity = _baseIntensity + Mathf.Sin(Time.time * _speed * _randomIntensity) * _amplitude;
    }
}
