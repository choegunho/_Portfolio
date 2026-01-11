using UnityEngine;

public class Spider : Monster
{
    protected override void Awake()
    {
        _name = "Spider";
        _health = 120.0f;
        _damage = 25.0f;
        _chaseRange = 4.0f;
        _experience = 30.0f;

        base.Awake();
    }
}
