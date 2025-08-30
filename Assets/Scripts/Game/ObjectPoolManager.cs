using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private static List<PoolObjectInfo> ObjectPool = new List<PoolObjectInfo>();

    private  GameObject _objectPoolHolder;
    
    private static GameObject _gameObjectHolder;
    private static GameObject _enemiesHolder;

    public enum PoolType
    {
        Gameobject,
        Enemies,
        None

    }

    private void Awake()
    {
        ObjectPool.Clear();
        SetUpEmpties();
    }

    private  void SetUpEmpties()
    {
        _objectPoolHolder = new GameObject("Pooled Objects");
        _gameObjectHolder = new GameObject("GameObjects");
        _gameObjectHolder.transform.SetParent(_objectPoolHolder.transform);
        _enemiesHolder = new GameObject("Enemies");
        _enemiesHolder.transform.SetParent(_objectPoolHolder.transform);

    }

    public static GameObject SpawnObject(GameObject objToSpawn, Vector3 SpawnPos, Quaternion SpawnRot, PoolType poolType = PoolType.Gameobject)
    {
        var pool = ObjectPool.Find(p => p.LookupString == objToSpawn.name);

        if (pool == null)
        {
            pool = new PoolObjectInfo(){LookupString = objToSpawn.name};
            ObjectPool.Add(pool);
        }

        var spawnableObj = pool.InactiveObjects.FirstOrDefault(o => o is not null);

        if (spawnableObj is null)
        {
            var parentObj = SetParentObject(poolType);
            spawnableObj = Instantiate(objToSpawn, SpawnPos, SpawnRot);
            if (parentObj is not null)
            {
                spawnableObj.transform.SetParent(parentObj.transform);
            }
        }
        else
        {
            if (spawnableObj == null || spawnableObj.Equals(null))
            {
                pool.InactiveObjects.Remove(spawnableObj);
                return SpawnObject(objToSpawn, SpawnPos, SpawnRot, poolType); // retry
            }
            spawnableObj.transform.position = SpawnPos;
            spawnableObj.transform.rotation = SpawnRot;
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        var goName = obj.name.Replace("(Clone)", "");
        var pool = ObjectPool.Find(p => p.LookupString == goName);
        if (pool == null)
        {
            Debug.LogWarning("Object pool could not be found: " + goName);
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.Gameobject:
               return _gameObjectHolder;
            case PoolType.Enemies:
                return _enemiesHolder;
            default:
               return null;
        }
    }
    public class PoolObjectInfo
    {
        public string LookupString;
        public List<GameObject> InactiveObjects = new List<GameObject>();
    }
    
}
