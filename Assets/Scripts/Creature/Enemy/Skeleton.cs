using UnityEngine;

public class Skeleton : Monster
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        _health = 50.0f;
        _damage = 15.0f;

        base.Start();
    }
}
