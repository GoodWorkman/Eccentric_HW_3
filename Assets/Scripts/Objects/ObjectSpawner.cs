using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private int _coinsCount = 10;
    [SerializeField] private int _bombCount = 10;
    [SerializeField] private float _checkRadius = 1.5f;

    [SerializeField] private Transform _coinContainer;
    [SerializeField] private Transform _bombContainer;

    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private Coin _coinPrefab;

    [SerializeField] private Transform _spawnCornerA;
    [SerializeField] private Transform _spawnCornerB;

    private UniversalPool<Bomb> _bombPool;
    private UniversalPool<Coin> _coinPool;

    private void Awake()
    {
        _bombPool = new UniversalPool<Bomb>(_bombPrefab, _bombCount, _bombContainer);
        _coinPool = new UniversalPool<Coin>(_coinPrefab, _coinsCount, _coinContainer);
        
        SpawnObjects(_coinPool, _coinsCount);
        SpawnObjects(_bombPool, _bombCount);
    }
    
    private void OnDrawGizmos()
    {
        Vector3 center = (_spawnCornerA.position + _spawnCornerB.position) / 2;
        Vector3 size = _spawnCornerB.position - _spawnCornerA.position;
        
        Gizmos.DrawWireCube(center, size);
    }

    private void SpawnObjects<T>(UniversalPool<T> pool, int size) where T: MonoBehaviour
    {
        for (int i = 0; i < size; i++)
        {
            TrySpawnObject(pool);
        }
    }

    private void TrySpawnObject<T>(UniversalPool<T> pool) where T: MonoBehaviour
    {
        int attempts = 10;

        for (int i = 0; i < attempts; i++)
        {
            Vector3 spawnPosition = GeneratePosition();

            Collider[] colliders = Physics.OverlapSphere(spawnPosition, _checkRadius);

            if (colliders.Length == 0)
            {
                pool.TryGetElement(spawnPosition, Quaternion.identity);
                
                break;
            }
        }
    }

    private Vector3 GeneratePosition()
    {
        float coordX = Random.Range(_spawnCornerA.position.x, _spawnCornerB.position.x);
        float coordY = Random.Range(_spawnCornerA.position.y, _spawnCornerB.position.y);
        float coordZ = Random.Range(_spawnCornerA.position.z, _spawnCornerB.position.z);

        return new Vector3(coordX, coordY, coordZ);
    }
}
