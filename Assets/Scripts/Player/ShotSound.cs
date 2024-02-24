using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotSound : MonoBehaviour
{
    [SerializeField] private AudioSource _shotSound;

    private float _minPitch = 0.6f;
    private float _maxPitch = 0.7f;
    public void PlaySound()
    {
        _shotSound.pitch = Random.Range(_minPitch, _maxPitch);
        
        _shotSound.Play();
    }
}
