using UnityEngine;

public class MotorSoundTRASH : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private float _minSoundValue = .05f;
    private float _maxSoundValue = .3f;
    private float _soundChangeSpeed = 1f;
    private float _targetVolume;

    private void Update()
    {
       _targetVolume = Mathf.Clamp(Input.GetAxis("Slope(Vertical)"), _minSoundValue, _maxSoundValue);

       _audioSource.volume = Mathf.Lerp(_audioSource.volume, _targetVolume, _soundChangeSpeed * Time.deltaTime);
       
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
}
