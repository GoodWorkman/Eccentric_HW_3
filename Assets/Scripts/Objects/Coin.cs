using System;
using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour, IDamageable, IPoolable<Coin>
{
    [SerializeField] private AudioSource _audioSource;

    private bool _isActiveBeforeCollision;
    private float _health = 10f;
    private float _defaultHealth = 10f;
    private UniversalPool<Coin> _coinPool;

    public Action<Coin> OnCoinCollected;

    private Coroutine _returnToPoolAfterSound;

    private void OnEnable()
    {
        _isActiveBeforeCollision = true; // при появлении устанавливаем по дефолту(именно так, поскольку в ондисэйбл флаг приводит к повторной коллизии, пока объект не выключен)
        _health = _defaultHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isActiveBeforeCollision) return;

        _isActiveBeforeCollision = false;
        
        Debug.Log(other.gameObject.name);

        if (other.attachedRigidbody)

        {
            PlayerHealth playerHealth = other.attachedRigidbody.GetComponent<PlayerHealth>();
            
            Debug.Log("playerhealt found: "+ playerHealth);

            if (playerHealth != null)
            {
                DeactivateObject();
            }
        }
    }

    public void ApplyDamage(float damageValue)
    {
        Debug.Log("remaining health: " + _health);
        
        _health -= damageValue;

        if (_health <= 0)
        {
            _health = 0;
            
            DeactivateObject();
        }
    }

    private void DeactivateObject()
    {
        OnCoinCollected?.Invoke(this);

        _audioSource.Play();

        _returnToPoolAfterSound = StartCoroutine(ReturnToPoolAfterSound());
    }

    private IEnumerator ReturnToPoolAfterSound()
    {
        yield return new WaitForSeconds(_audioSource.clip.length);
        
        _coinPool.ReturnToPool(gameObject);
    }

    public void InitPool(UniversalPool<Coin> pool)
    {
        _coinPool = pool;
    }

    private void OnDisable()
    {
        if (_returnToPoolAfterSound != null) // проверяем, существует ли корутина, выполняется ли она
        {
            StopCoroutine(_returnToPoolAfterSound);
        }
    }
}