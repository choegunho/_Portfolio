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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }
}
