using System.Collections.Generic;
using UnityEngine;

public class FakeTilesPool : MonoBehaviour
{
    public static FakeTilesPool instant = null;

    [SerializeField] private FakeTile _prefab;
    [SerializeField] private Transform _container;

    private List<FakeTile> _pool;

    public void Awake()
    {
        if (instant == null)
            instant = this;

        else if (instant == this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void CreatePool(int size)
    {
        _pool= new List<FakeTile>();
        for (int i = 0; i<size; i++)
            CreateObjectInPool();   
    }

    public FakeTile GetFreeElement()
    {
        foreach (var tile in _pool)
        {
            if (!tile.gameObject.activeInHierarchy)
                return tile;
        }
        return CreateObjectInPool();
    }

    private FakeTile CreateObjectInPool()
    {
        FakeTile tile = Instantiate(_prefab, _container);
        tile.gameObject.SetActive(false);
        _pool.Add(tile);
        return tile;
    }
}
