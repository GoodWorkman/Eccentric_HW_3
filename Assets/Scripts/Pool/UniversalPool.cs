using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class UniversalPool<T> where T : MonoBehaviour
{
    // Ссылка на префаб, который будет использоваться для создания объектов.
    public T Prefab { get; }

    // Флаг, указывающий, должен ли пул автоматически расширяться при необходимости.
    public bool IsAutoExpand { get; private set; }

    // Контейнер, в котором будут храниться объекты пула.
    public Transform Container { get; }

    // Список для хранения объектов пула.
    private List<T> _pool = new();

    // Конструктор пула, инициализирующий его начальными объектами.
    public UniversalPool(T prefab, int size, Transform container = null)
    {
        Prefab = prefab;
        Container = container;

        // Инициализация пула заданным количеством объектов.
        for (int i = 0; i < size; i++)
        {
            T newObject = CreateObject();
            _pool.Add(newObject);
        }
    }

    // Создание нового объекта пула.
    private T CreateObject(bool isActiveByDefault = false)
    {
        T createdObject = Object.Instantiate(Prefab, Container);
        createdObject.gameObject.SetActive(isActiveByDefault);
        InitializePoolMember(createdObject);
        return createdObject;
    }

    // Получение свободного элемента из пула.
    public T TryGetElement()
    {
        foreach (T element in _pool)
        {
            if (!element.gameObject.activeInHierarchy)
            {
                element.gameObject.SetActive(true);
                return element;
            }
        }
        // Расширение пула, если включен режим авторасширения.
        if (IsAutoExpand)
        {
            T newObject = CreateObject(true);
            _pool.Add(newObject);
            return newObject;
        }
        
        throw new Exception($"Pool is not set to auto-expand. No more elements in pool of type {typeof(T)}.");
    }

    // Получение свободного элемента из пула с установкой его позиции и поворота.
    public T TryGetElement(Vector3 position, Quaternion rotation)
    {
        T element = TryGetElement();
        element.transform.position = position;
        element.transform.rotation = rotation;
        return element;
    }
    
    // Получение свободного элемента из пула с установкой его позиции, поворота, и если надо, то вектора направления.
    public T TryGetElement(Vector3 position, Quaternion rotation, Vector3 forwardDirection)
    {
        T element = TryGetElement();
        element.transform.position = position;
        element.transform.rotation = rotation;
        element.transform.forward = forwardDirection; // Задаем направление
        return element;
    }

    // Возврат элемента обратно в пул.
    public void ReturnToPool(GameObject elementGameObject)
    {
        T element = elementGameObject.GetComponent<T>();
        
        if (_pool.Contains(element) && element != null)
        {
            element.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"Attempted to return an object to the pool that it didn't come from: {element}");
        }
    }
    
    public void InitializePoolMember(T member)
    {
        if (member is IPoolable<T> poolable)
        {
            poolable.InitPool(this);
        }
    }

    public void ActivateAutoExpand(bool autoexpand)
    {
        IsAutoExpand = autoexpand;
    }
}
