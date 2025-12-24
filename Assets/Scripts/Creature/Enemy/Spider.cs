using UnityEngine;

public class Spider : Monster
{
    protected override void Awake()
    {
        _name = "Spider";
        _health = 120.0f;
        _damage = 25.0f;

        base.Awake();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }
}
