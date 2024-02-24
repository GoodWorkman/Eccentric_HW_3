using System;
using UnityEngine;

public class PropellaBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private Transform _model;

    private AirplaneMover _airplane;

    private float _minSoundValue = .001f;
    private float _maxSoundValue = .2f;
    private float _soundChangeSpeed = 3f;
    private float _targetVolume;

    private void Awake()
    {
        _airplane = GetComponentInParent<AirplaneMover>();
    }

    private void Update()
    {
       ActivateSound();
        Rotate();
    }

    private void ActivateSound()
    {
        _targetVolume = Mathf.Clamp(_airplane.EnginePower, _minSoundValue, _maxSoundValue);

        _audio.volume = Mathf.Lerp(_audio.volume, _targetVolume, _soundChangeSpeed * Time.deltaTime);

        if (_audio.isPlaying == false)
        {
            _audio.Play();
        }
    }

    private void Rotate()
    {
        _model.Rotate(Vector3.forward * _airplane.EnginePower);
    }

    private void OnDisable()
    {
        if (_audio.isPlaying)
        {
            _audio.Stop();
        }
    }
}