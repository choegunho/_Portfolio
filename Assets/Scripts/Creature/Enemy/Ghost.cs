using UnityEngine;

public class Ghost : Monster
{
    protected override void Awake()
    {
        _name = "ghost";
        _health = 35.0f;
        _damage = 8.0f;

        base.Awake();
    }

}
