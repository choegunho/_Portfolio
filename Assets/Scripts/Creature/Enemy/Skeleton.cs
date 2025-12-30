using UnityEngine;

public class Skeleton : Monster
{
    protected override void Awake()
    {
        _name = "Skeleton";
        _health = 50.0f;
        _damage = 15.0f;

        base.Awake();
    }
}
