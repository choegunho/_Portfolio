using UnityEngine;

public class Bat : Monster
{
    protected override void Awake()
    {
        _name = "Bat";
        _health = 30.0f;
        _damage = 10.0f;
        _experience = 10.0f;

        base.Awake();
    }

}
