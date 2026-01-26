using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    private PlayerStateController _player;
    public float _maxHealth;
    public float _currentHealth;
    public float _defend;
    public float _damage;
    public float _moveSpeed;
    public int _level;
    public float _levelUpExperience;
    public float _experience;
    public float _bossDamage;

    void Awake()
    {
        _player = GetComponent<PlayerStateController>();
    }

    public void InitStat(float maxHealth, float defend, float damage, float moveSpeed, int level, float levelUpExperience, float experience, float bossDamage)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
        _defend = defend;
        _damage = damage;
        _moveSpeed = moveSpeed;
        _level = level;
        _levelUpExperience = levelUpExperience;
        _experience = experience;
        _bossDamage = bossDamage;
    }
    public void SaveStat()
    {
        _maxHealth = _player.Health;
        _currentHealth = _player.CurrentHealth;
        _defend = _player.Defense;
        _damage = _player.Damage;
        _moveSpeed = _player.Speed;
        _level = _player.Level;
        _levelUpExperience = _player.LevelUpExperience;
        _experience = _player.Experience;
        _bossDamage = _player.BossDamage;
    }

    public void ApplyStat()
    {
        _player.Health = _maxHealth;
        _player.CurrentHealth = _currentHealth;
        _player.Defense = _defend;
        _player.Damage = _damage;
        _player.Speed = _moveSpeed;
        _player.Level = _level;
        _player.LevelUpExperience = _levelUpExperience;
        _player.Experience = _experience;
        _player.BossDamage = _bossDamage;
    }
}
