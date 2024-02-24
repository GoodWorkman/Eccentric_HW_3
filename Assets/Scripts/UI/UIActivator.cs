using TMPro;
using UnityEngine;

public class UIActivator : MonoBehaviour
{
    [SerializeField] private CoinCounter _coinCounter;
    [SerializeField] private PlayerHealth _playerHealth;

    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _healthText;

    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _loseScreen;

    private void Start()
    {
        _coinCounter.OnCoinsCountChanged += ChangeCoinsText;
        _coinCounter.OnAllCoinsCollected += Win;

        _playerHealth.OnHealthChanged += ChangeHealthText;
        _playerHealth.OnPlayerDie += Lose;

        ChangeHealthText(_playerHealth.Health);
        ChangeCoinsText(_coinCounter.TotalCoins);
    }

    private void ChangeHealthText(float health)
    {
        _healthText.text = "Health: " + health;
    }

    private void ChangeCoinsText(int remainingCoins)
    {
        _coinsText.text = " Coins left: " + remainingCoins;
    }

    private void Win()
    {
        //Time.timeScale = 0;

        _winScreen.SetActive(true);
    }

    private void Lose()
    {
        //Time.timeScale = 0;

        _loseScreen.SetActive(true);
    }

    private void OnDestroy()
    {
        _coinCounter.OnCoinsCountChanged -= ChangeCoinsText;
        _coinCounter.OnAllCoinsCollected -= Win;

        _playerHealth.OnHealthChanged -= ChangeHealthText;
        _playerHealth.OnPlayerDie -= Lose;
    }
}