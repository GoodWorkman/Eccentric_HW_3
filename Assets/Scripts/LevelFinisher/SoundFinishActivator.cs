using UnityEngine;

public class SoundFinishActivator : MonoBehaviour
{
    [SerializeField] private CoinCounter _coinCounter;
    [SerializeField] private PlayerHealth _playerHealth;

    [SerializeField] private AudioSource _winSound;
    [SerializeField] private AudioSource _loseSound;

    private void Start()
    {
        _coinCounter.OnAllCoinsCollected += PlayWinSound;
        _playerHealth.OnPlayerDie += PlayLoseSound;
    }

    private void PlayWinSound()
    {
        _winSound.Play();
    }

    private void PlayLoseSound()
    {
        _loseSound.Play();
    }

    private void OnDestroy()
    {
        _coinCounter.OnAllCoinsCollected -= PlayWinSound;
        _playerHealth.OnPlayerDie -= PlayLoseSound;
    }
}
