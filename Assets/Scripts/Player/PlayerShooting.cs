using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletContainer;
    [SerializeField] private ShotSound _shotSound;

    [SerializeField] private float _shootRange = 1f;

    private UniversalPool<Bullet> _bulletPool;
    private int _poolCapacity = 20;
    private float _timer;

    private void Start()
    {
        _bulletPool = new UniversalPool<Bullet>(_bulletPrefab, _poolCapacity, _bulletContainer);
        _bulletPool.ActivateAutoExpand(true);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            if (_timer > _shootRange)
            {
                _timer = 0;

                Bullet newBullet = _bulletPool.TryGetElement(_spawnPoint.position, _spawnPoint.rotation); // вот здесь если вызвать перегруженный метод с направлением, то он не работает корректно, а как будто с небольшим отставанием позиций
                
                newBullet.ActivateBullet(_spawnPoint.forward);
                
                _shotSound.PlaySound();
                
                newBullet.InitPool(_bulletPool); // буллет не наследует IPoolable, поэтому здесь надо явно вызвать инициализацию пула
            }
        }

    }
}
