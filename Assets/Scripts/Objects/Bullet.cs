using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Common Bullet Stats")] 
    [SerializeField] private LayerMask _bulletDestroyer; // не забудь выставить слои в проекте, которые будут уничтожать пулю, иначе она не будет вызывать метод нанесения урона
    [SerializeField] private float _lifeTime = 3f;
    [SerializeField] private BulletType _bulletType;

    [Header("Normal Bullet Stats")] 
    [SerializeField] private float _normalBulletSpeed = 17f;
    [SerializeField] private float _normalBulletDamage = 1f;

    [Header("Physics Bullet Stats")] 
    [SerializeField] private float _physicsBulletSpeed = 20f;
    [SerializeField] private float _physicsBulletDamage = 2f;
    //[SerializeField] private float _physicsBulletGravity = 3f;

    private Rigidbody _rigidbody;
    private float _damage;
    private Coroutine _returnToPoolTimerCoroutine;
    private UniversalPool<Bullet> _bulletPool;

    public enum BulletType
    {
        Normal,
        Physics
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = Vector3.zero; // сбрасываем вращения, чтобы при возвращении пуля не крутилась
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void OnEnable()
    {
        _returnToPoolTimerCoroutine = StartCoroutine(ReturnToPoolAfterTimer());
        
        SetRigidbodyStats(); // выбираем гравитацию для пули в зависимости от вида пули
    }

    private void FixedUpdate()
    {
        if (_bulletType == BulletType.Physics)
        {
            transform.forward = _rigidbody.velocity; // поворот рб пули вперед ?? надо ли
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_bulletDestroyer.value & (1 << other.gameObject.layer)) > 0)
        {
            // spawn particles- optionally
            //play sound hit - optionally
            //Damage obj
            
            Debug.Log(other.gameObject.name);

            IDamageable iDamageable = other.gameObject.GetComponent<IDamageable>();

            if (iDamageable != null)
            {
                iDamageable.ApplyDamage(_damage);
            }
            
            _bulletPool.ReturnToPool(gameObject);
        }
    }

    public void InitPool(UniversalPool<Bullet> pool)
    {
        _bulletPool = pool;
    }

    public void ActivateBullet(Vector3 direction)
    {
        transform.forward = direction; // Устанавливаем направление пули
        
        InitBulletStats(); // Инициализируем скорость и направление движения
    }
    
    private void OnDisable()
    {
        _rigidbody.velocity = Vector3.zero; // сбрасываем вращения, чтобы при возвращении пуля не крутилась
        _rigidbody.angularVelocity = Vector3.zero;
        
        StopCoroutine(_returnToPoolTimerCoroutine);
    }

    private void InitBulletStats()
    {
        if (_bulletType == BulletType.Normal)
        {
            SetStraightVelocity();

            _damage = _normalBulletDamage;
        }
        else if (_bulletType == BulletType.Physics)
        {
            SetPhysicsVelocity();

            _damage = _physicsBulletDamage;
        }
    }

    private IEnumerator ReturnToPoolAfterTimer()
    {
        float currentTime = 0f;

        while (currentTime < _lifeTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        
        _bulletPool.ReturnToPool(gameObject);
    }

    private void SetStraightVelocity()
    {
        _rigidbody.velocity = transform.forward * _normalBulletSpeed;
    }

    private void SetPhysicsVelocity()
    {
        _rigidbody.velocity = transform.forward * _physicsBulletSpeed;
    }

    private void SetRigidbodyStats()
    {
        if (_bulletType == BulletType.Normal)
        {
            _rigidbody.useGravity = false;
        }
        else if (_bulletType == BulletType.Physics)
        {
            _rigidbody.useGravity = true;
        }
    }
}