using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;

public class AbilityHandler : MonoBehaviour
{
    public List<AbilityData> _abilities = new();
    private PlayerStateController _player;

    private void Awake()
    {
        _player = GetComponent<PlayerStateController>();
    }

    public List<AbilityData> GetAbilities()
    {
        return _abilities;
    }

    private void Update()
    {
        foreach(var ability in _abilities)
        {
            ability.OnUpdate(_player);
        }
    }

    public bool HasAbility(AbilityData ability)
    {
        return _abilities.Contains(ability);
    }

    public void AddAbility(AbilityData ability)
    {
        _abilities.Add(ability);
        ability.OnAcquire(_player);
    }

    public void OnHitMonster(Monster monster)
    {
        foreach(var ability in _abilities)
        {
            ability.OnHit(_player, monster);
        }
    }

    public void OnKillMonster(Monster monster)
    {
        foreach(var ability in _abilities)
        {
            ability.OnKill(_player, monster);
        }
    }
}
