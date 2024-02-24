using UnityEngine;

public interface IPoolable<T> where T : MonoBehaviour
{
    void InitPool(UniversalPool<T> pool);
    
    // этот интерфейс должен быть на всех объектах, которые должны быть в пуле и которые не могут явно проинициализировать тип
    //пула (если обобщенно спавнятся через <Т>)
}
