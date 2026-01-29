using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private RoomController _roomController;
    [SerializeField] private Transform _player;
    [SerializeField] private Camera _camera;
    private string _currentStage;
    private Transform _spawnPoint;

    public Transform PlayerTransform
    {
        get { return _player; }
    }

    public Camera Camera
    {
        get { return _camera; }
    }

    public string CurrentStage
    {
        get { return _currentStage; }
        set { _currentStage = value; }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartGame()
    {
        if (string.IsNullOrEmpty(_currentStage))
        {
            RequestLoadStage("Stage3");
        }
    }

    public void RegisterRoomController(RoomController roomController)
    {
        _roomController = roomController;
    }

    public void SetSpawnPoint(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint;
    }

    public void RequestLoadStage(string stage)
    {
        StartCoroutine(LoadStage(stage));
    }

    public void MovePlayerToSpawn(Transform spawnPoint)
    {
        _player.position = spawnPoint.position;
        _player.rotation = spawnPoint.rotation;

    }
    public void ClearStage()
    {
        int num = int.Parse(_currentStage.Replace("Stage", ""));
        if (num >= 3) return;
        string nextStage = $"Stage{num + 1}";

        RequestLoadStage(nextStage);
    }

    public IEnumerator LoadStage(string stage)
    {
        string prevStage = _currentStage;

        if (!string.IsNullOrEmpty(prevStage))
        {
            Scene prevScene = SceneManager.GetSceneByName(prevStage);
            if (prevScene.isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(prevStage);
            }
        }

        yield return SceneManager.LoadSceneAsync(stage, LoadSceneMode.Additive);

        Scene stageScene = SceneManager.GetSceneByName(stage);
        SceneManager.SetActiveScene(stageScene);

        PlayerSpawn spawn = FindAnyObjectByType<PlayerSpawn>();
        if (spawn != null)
        {
            MovePlayerToSpawn(spawn.SpawnPoint);
        }
        else
        {
            Debug.LogError("PlayerSpawn ¸ø Ã£À½!");
        }

        _currentStage = stage;

        if (_spawnPoint != null)
        {
            MovePlayerToSpawn(_spawnPoint);
            _spawnPoint = null;
        }
    }

}
