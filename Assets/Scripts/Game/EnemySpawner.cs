using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public int SpawnLimit { get; internal set; }

    private int MeleeLimit;
    private int RangeLimit;

    internal int _curSpawnCount;
    private PlayerController player;
    [SerializeField] private GameObject[] _enemiesList;
    bool Spawning;
    public static Action OnSpawn;
    public static Action OnDeath;

    Vector3 SpawnLocation;
    [SerializeField] private GameObject[] spawnAreas;
    private List<GameObject> AvailableSpawns = new();
    private float SpawnTime;
    private int SpawnAmount;
    internal int curEnemyCount;

    private void Awake()
    {
        AvailableSpawns.AddRange(spawnAreas);
        GameManager.GameStart += CheckGameMode;
        player = FindFirstObjectByType<PlayerController>();
        player.playerOnDeath += OnThisDisable;
    }

    private void OnEnable()
    {
        curEnemyCount = 0;
    }

    private void OnThisDisable()
    {
        enabled = false;
    }

    private void CheckGameMode()
    {
        switch (GameManager.CurGameMode)
        {
            case GameManager.GameMode.Kill:
                SpawnTime = 0;
                SpawnLimit = 10 + GameModifiers.enemyCountModifer;
                SpawnAmount = 1;
                break;
            case GameManager.GameMode.Survival:
                SpawnTime = 2;
                SpawnLimit = 10 + GameModifiers.enemyCountModifer;
                SpawnAmount = 2;
                break;
            default:
                Debug.LogError("Game Mode Not Supported");
                break;
        }

        print(GameManager.CurGameMode);
    }

    private void Start()
    {
        OnDeath += RemoveEnemyCount;
    }

    private void RemoveEnemyCount()
    {
        curEnemyCount--;
        GameManager.ObjectiveUpdate?.Invoke();
        if (GameManager.CurGameMode != GameManager.GameMode.Kill)
        {
            _curSpawnCount--;
        }
        

        if (curEnemyCount == 0 && player.enemiesKilled == _curSpawnCount && _curSpawnCount > 0)
        {
            GameManager.GameEnd?.Invoke();
        }

        print("minus");
    }


    private void Update()
    {
        if (_curSpawnCount < SpawnLimit && !Spawning)
        {
            StartCoroutine(Spawn());
        }

        foreach (var spawnArea in spawnAreas)
        {
            var player = FindFirstObjectByType<PlayerController>().gameObject;

            if (spawnArea.GetComponent<Collider2D>().bounds.Contains(player.transform.position))
            {
                AvailableSpawns.Remove(spawnArea);
            }
            else
            {
                if (!AvailableSpawns.Contains(spawnArea)) AvailableSpawns.Add(spawnArea);
            }
        }
    }

    public bool GetRandPointInArea(out Vector3 result)
    {
        var randomArea = Random.Range(0, AvailableSpawns.Count);
        var Area = AvailableSpawns[randomArea];
        var randomPoint =
            new Vector3(
                Random.Range(Area.GetComponent<Collider2D>().bounds.min.x,
                    Area.GetComponent<Collider2D>().bounds.max.x),
                Random.Range(Area.GetComponent<Collider2D>().bounds.min.y,
                    Area.GetComponent<Collider2D>().bounds.max.y), 0);
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(randomPoint, out navMeshHit, 1.0f, NavMesh.AllAreas))
        {
            result = navMeshHit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private IEnumerator Spawn()
    {
        Spawning = true;
        var CurNumberofSpawns = 0;
        while (CurNumberofSpawns < SpawnAmount)
        {
            var EnemyToSpawn = RandomEnemy();
            if (GetRandPointInArea(out var result))
            {
                ObjectPoolManager.SpawnObject(EnemyToSpawn, result, Quaternion.identity,
                    ObjectPoolManager.PoolType.Enemies);
                CurNumberofSpawns++;
            }
            curEnemyCount++;
            _curSpawnCount++;
            GameManager.ObjectiveUpdate?.Invoke();
        }
        yield return new WaitForSeconds(SpawnTime);
        Spawning = false;
    }

    private GameObject RandomEnemy()
    {
        return _enemiesList[Random.Range(0, _enemiesList.Length)];
    }
}