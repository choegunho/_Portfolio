using UnityEngine;

public class ChestMonster : Monster
{
    protected override void Awake()
    {
        _name = "Mimic";
        _health = 70.0f;
        _damage = 20.0f;

        base.Awake();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }
}
