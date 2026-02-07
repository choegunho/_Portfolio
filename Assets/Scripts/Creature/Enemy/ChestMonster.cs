using UnityEngine;
using UnityEngine.UIElements;

public class ChestMonster : Monster
{
    private Vector3 _scale = new Vector3(0.5f, 0.5f, 0.5f);
    protected override void Awake()
    {
        _name = "Mimic";
        _health = 700.0f;
        _damage = 25.0f;
        _experience = 65.0f;

        base.Awake();
    }

    public override void ResetScale()
    {
        transform.localScale = _scale;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }
}
