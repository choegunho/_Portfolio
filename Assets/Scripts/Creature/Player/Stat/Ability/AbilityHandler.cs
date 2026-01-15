using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;

public class AbilityHandler : MonoBehaviour
{
    private List<AbilityData> abilities = new();
    private PlayerStateController _player;

    private void Awake()
    {
        _player = GetComponent<PlayerStateController>();
    }

    private void Update()
    {
        foreach(var ability in abilities)
        {
            ability.OnUpdate(_player);
        }
    }

    public bool HasAbility(AbilityData ability)
    {
        return abilities.Contains(ability);
    }

    public void AddAbility(AbilityData ability)
    {
        abilities.Add(ability);
        ability.OnAcquire(_player);
    }

    public void OnHitMonster(Monster monster)
    {
        foreach(var ability in abilities)
        {
            ability.OnHit(_player, monster);
        }
    }

    public void OnKillMonster(Monster monster)
    {
        foreach(var ability in abilities)
        {
            ability.OnKill(_player, monster);
        }
    }
}
