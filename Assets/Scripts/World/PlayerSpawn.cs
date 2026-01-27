using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    public Transform SpawnPoint => _spawnPoint;
}
