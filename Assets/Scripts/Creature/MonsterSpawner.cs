using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _monsterPrefabs;
    [SerializeField] private int _spawnCount = 8;
    private float _spawnRadius = 10.0f;
    private float _minDistanceBetweenMonsters = 1.2f;
    private float minDistanceFromPlayer = 5.0f;
    [SerializeField] private Transform player;

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
            Vector3 _spawnPos = GetRandomSpawnPosition();
            GameObject monster = RandomMonster(); 

            if (_spawnPos != Vector3.zero)
            {
                Instantiate(monster, _spawnPos, Quaternion.identity, transform);

                spawnedPositions.Add(_spawnPos);
            }
        }
    }

    GameObject RandomMonster()
    {
        GameObject monster = _monsterPrefabs[Random.Range(0, _monsterPrefabs.Length)];

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
