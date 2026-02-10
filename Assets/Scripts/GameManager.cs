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
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _playerUI;
    [SerializeField] private GameObject _miniMapUI;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _statUI;
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
            RequestLoadStage("Stage1");
        }
    }

    public void ActivateUI()
    {
        _playerUI.SetActive(true);
        _miniMapUI.SetActive(true);
        _statUI.SetActive(true);
    }

    public void DeActivateUI()
    {
        _playerUI.SetActive(false);
        _miniMapUI.SetActive(false);
        _statUI.SetActive(false);
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
        MiniMap.Instance.ResetMiniMap();
    }

    public void MainMenu()
    {
        SceneManager.UnloadSceneAsync(_currentStage);
        _mainMenu.gameObject.SetActive(true);
        _currentStage = null;
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        _player.GetComponent<PlayerStateController>().ResetPlayer();
        _gameOverUI.SetActive(true);
        GameManager.Instance.DeActivateUI();
        Monster[] monsters = UnityEngine.Object.FindObjectsByType<Monster>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        HPControl[] hpBars = UnityEngine.Object.FindObjectsByType<HPControl>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach(var monster in monsters)
        {
            Destroy(monster.gameObject);
        }

        foreach(var hpBar in hpBars)
        {
            Destroy(hpBar.gameObject);
        }
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
