using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private RoomController _roomController;
    private PlayerStat _playerStat;
    private PlayerStateController _player;
    private AbilityHandler _abilityHandler;

    private PlayerStat _savePlayerStat;
    private AbilityHandler _saveAbilityHandler;

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

    public void RegisterAbilityHandler(AbilityHandler abilityHandler)
    {
        _abilityHandler = abilityHandler;
    }

    public void RegisterPlayer(PlayerStateController player)
    {
        _player = player;
    }

    public void RegisterRoomController(RoomController roomController)
    {
        _roomController = roomController;
    }

    private void SaveInformation()
    {
        _playerStat = _player.GetComponent<PlayerStat>();
        _playerStat.SaveStat();
        _savePlayerStat = _playerStat;

        foreach(var ability in _abilityHandler.GetAbilities())
        {
            _abilityHandler._abilities.Add(ability);
        }
        _saveAbilityHandler = _abilityHandler;
    }

    private void ClearStage()
    {
        SaveInformation();
        SceneManager.LoadScene("NextStage");
    }

    public void ApplyStat()
    {
        _playerStat = _savePlayerStat;
        _abilityHandler = _saveAbilityHandler;
        _playerStat.ApplyStat();
        foreach(var ability in _abilityHandler.GetAbilities())
        {
            if(!ability.stat)
            _abilityHandler.AddAbility(ability);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
