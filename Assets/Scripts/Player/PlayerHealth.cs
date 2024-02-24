using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth = 100f;

    private float _currentHealth;

    public Action<float> OnHealthChanged;
    public Action OnPlayerDie;

    public float Health => _maxHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void ApplyDamage(float damageValue)
    {
        _currentHealth -= damageValue;
        
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
        
        OnHealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnPlayerDie?.Invoke();
        
        //Camera.main.transform.parent = null; // -вынесено в отдельный класс деактиватор
        //gameObject.SetActive(false); // -вынесено в отдельный класс деактиватор
    }
}
