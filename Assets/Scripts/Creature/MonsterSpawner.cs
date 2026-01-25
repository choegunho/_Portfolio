using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _monsterPrefabs;
    [SerializeField] private int _spawnCount = 8;
    private float _spawnRadius = 10.0f;
    private float _minDistanceBetweenMonsters = 1.2f;
    private float minDistanceFromPlayer = 5.0f;
    [SerializeField] private Transform player;
    private bool _isBoss = false;
    private BossMonster _boss;

    public bool IsBoss
    {
        set { _isBoss = value; }
    }

    private List<Vector3> spawnedPositions = new List<Vector3>();

    public int SpawnCount { get { return _spawnCount; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnMonster();
    }

    void SpawnMonster()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPosition();
            if (spawnPos == Vector3.zero) continue;

            GameObject monsterPrefab = RandomMonster();
            GameObject monsterInstance =
Instantiate(monsterPrefab, spawnPos, Quaternion.identity, transform);

            spawnedPositions.Add(spawnPos);

            if (_isBoss)
            {
                BossMonster boss = monsterInstance.GetComponent<BossMonster>();
                if (boss != null)
                {
                    Debug.Log("BossMonster Spawn!");
                    boss.IsBoss = true;
                    boss.Boss();
                }
            }
            else
            {
                Monster monster = monsterInstance.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.ResetScale();
                }
            }
        }
    }


    GameObject RandomMonster()
    {
        GameObject monster;
        if (_isBoss)
        {
            monster = _monsterPrefabs[Random.Range(0, _monsterPrefabs.Count)];
            _monsterPrefabs.Remove(monster);
        }
        else
        {
            monster = _monsterPrefabs[Random.Range(0, _monsterPrefabs.Count)];
        }

        return monster;
    }

    Vector3 GetRandomSpawnPosition()
    {
        for(int i = 0; i < 20; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * _spawnRadius;

            randomPoint.y = 0.0f;


            if(NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.5f, NavMesh.AllAreas))
            {
                if (player != null &&
                    Vector3.Distance(hit.position, player.position) < minDistanceFromPlayer)
                {
                    continue;
                }

                bool tooClose = false;
                foreach(var pos in spawnedPositions)
                {
                    if (Vector3.Distance(pos, hit.position) < _minDistanceBetweenMonsters)
                    {
                        tooClose = true;
                        break;
                    }

                }

                if (!tooClose)
                    return hit.position;
            }
        }
        return Vector3.zero;
    }
}
