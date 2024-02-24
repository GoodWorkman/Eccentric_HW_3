using System;
using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour, IDamageable, IPoolable<Bomb>
{
    [SerializeField] private AudioSource _audioSource;

    private bool _isActiveBeforeCollision;
    
    private float _health = 10f;
    private float _defaultHealth = 10f;
    private float _damage = 10f;
    
    private UniversalPool<Bomb> _bombPool;
    
    private Coroutine _returnToPoolAfterSound;

    private void OnEnable()
    {
        _isActiveBeforeCollision = true; // при появлении устанавливаем по дефолту(именно так, поскольку в ондисэйбл флаг приводит к повторной коллизии, пока объект не удален)
        _health = _defaultHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isActiveBeforeCollision) return;

        _isActiveBeforeCollision = false;

        if (other.attachedRigidbody)
        {
            IDamageable damageable = other.attachedRigidbody.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.ApplyDamage(_damage);
                
                DeactivateObject();
            }
        }
    }

    public void ApplyDamage(float damageValue)
    {
        Debug.Log("remaining health: " + _health);

        _health -= damageValue;
        
        _health = Mathf.Clamp(_health, 0f, _defaultHealth);

        if (_health <= 0)
        {
            DeactivateObject();
        }
    }

    private void DeactivateObject()
    {
        _audioSource.Play();

        _returnToPoolAfterSound = StartCoroutine(ReturnToPoolAfterSound());
    }

    private IEnumerator ReturnToPoolAfterSound()
    {
        yield return new WaitForSeconds(_audioSource.clip.length);

        _bombPool.ReturnToPool(gameObject);
    }

    private void OnDisable()
    {
        if (_returnToPoolAfterSound != null) // проверяем, существует ли корутина, выполняется ли она
        {
            StopCoroutine(_returnToPoolAfterSound);
        }
    }

    public void InitPool(UniversalPool<Bomb> pool)
    {
        _bombPool = pool;
    }
}