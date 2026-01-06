using UnityEngine;

public class Slime : Monster
{
    protected override void Awake()
    {
        _name = "Slime";
        _health = 50.0f;
        _damage = 5.0f;

        base.Awake();
    }

    public override void GetDamage(float damage)
    {
        if (_currentState == State.Dead) return;

        _health -= damage;

        _healthUI.TakeDamage(damage);

        _health = Mathf.Max(_health, 0);

        Debug.Log($"{_name}: {_health}");

        if (_health == 0)
        {
            _currentState = State.Dead;
        }
    }
}
