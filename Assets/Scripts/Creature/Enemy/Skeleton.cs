using UnityEngine;

public class Skeleton : Monster
{
    protected override void Awake()
    {
        _name = "Skeleton";
        _health = 300.0f;
        _damage = 30.0f;
        _experience = 35.0f;

        base.Awake();
    }
}
