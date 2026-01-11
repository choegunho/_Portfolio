using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

enum RoomType
{
    Normal,
    Boss
}

public class RoomController : MonoBehaviour
{
    [SerializeField] private RoomType _roomType;
    private MonsterSpawner _monsterSpawner;
    private Doors[] _doors;
    private int _currentMonster;
    private bool _currentRoom = false;

    private bool _isEntered = false;
    private bool _isCleared = false;


    private void OnTriggerEnter(Collider other)
    {
        if (_isCleared) return;

        if(other.gameObject.CompareTag("Player") == true)
        {
            EnterRoom();
        }
    }

    private void Awake()
    {
        _monsterSpawner = GetComponentInChildren<MonsterSpawner>();

        _doors = GetComponentsInChildren<Doors>(true);
    }

    public void EnterRoom()
    {
        if (_isEntered) return;
        foreach(var door in _doors)
        {
            door.gameObject.SetActive(true);
        }
        _isEntered = true;
        _currentRoom = true;
        CloseDoors();
        ActivateRoom();
    }

    public void DecreaseMonster()
    {
        _currentMonster--;

        if(_currentMonster == 0)
        {
            ClearRoom();
        }
    }

    private void ActivateRoom()
    {
        if(_roomType == RoomType.Boss)
        {
            _monsterSpawner.IsBoss = true;
            _monsterSpawner.enabled = true;
            _currentMonster = _monsterSpawner.SpawnCount;
        }

        else
        {
            _monsterSpawner.IsBoss = false;
            _monsterSpawner.enabled = true;
            _currentMonster = _monsterSpawner.SpawnCount;
        }

    }

    public void ClearRoom()
    {
        if (_isCleared) return;

        if(_currentMonster == 0)
        {
            _isCleared = true;
            OpenDoors();
            _currentRoom = false;
        }
    }

    private void CloseDoors()
    {
        if (_currentRoom)
        {
            foreach(var door in _doors)
            {
                door.CloseDoor();
            }
        }
    }

    public void OpenDoors()
    {
        foreach(var door in _doors)
        {
            door.OpenDoor();
        }
    }
}
