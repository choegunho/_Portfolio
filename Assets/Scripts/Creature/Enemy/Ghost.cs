using UnityEngine;

public class Ghost : Monster
{
    protected override void Awake()
    {
        _name = "Ghost";
        _health = 45.0f;
        _damage = 8.0f;
        _experience = 20.0f;

        base.Awake();
    }

}
