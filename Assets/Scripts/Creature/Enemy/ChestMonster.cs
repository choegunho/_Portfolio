using UnityEngine;

public class ChestMonster : Monster
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        _health = 70.0f;
        _damage = 20.0f;

        base.Start();
    }
}
