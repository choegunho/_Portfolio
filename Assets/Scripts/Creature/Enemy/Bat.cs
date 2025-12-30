using UnityEngine;

public class Bat : Monster
{
    protected override void Awake()
    {
        _health = 30.0f;
        _damage = 10.0f;

        base.Awake();
    }

}
