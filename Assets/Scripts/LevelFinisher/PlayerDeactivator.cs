using UnityEngine;

public class PlayerDeactivator : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private CoinCounter _coinCounter;

    private void Start()
    {
        _playerHealth.OnPlayerDie += DeactivatePlayer;
        _coinCounter.OnAllCoinsCollected += DeactivatePlayer;
    }

    private void DeactivatePlayer()
    {
        Camera.main.transform.parent = null;
        
        _playerHealth.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _playerHealth.OnPlayerDie -= DeactivatePlayer;
        _coinCounter.OnAllCoinsCollected -= DeactivatePlayer;
    }
}
