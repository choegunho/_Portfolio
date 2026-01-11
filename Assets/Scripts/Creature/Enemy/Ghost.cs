using UnityEngine;

public class Ghost : Monster
{
    protected override void Awake()
    {
        _name = "Ghost";
        _health = 35.0f;
        _damage = 8.0f;
        _experience = 15.0f;

        base.Awake();
    }

}
