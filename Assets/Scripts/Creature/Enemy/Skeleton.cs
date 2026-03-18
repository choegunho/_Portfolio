using UnityEngine;

public class Skeleton : Monster
{
    private Vector3 _scale = new Vector3(0.5f, 0.5f, 0.5f);
    protected override void Awake()
    {
        _name = "Skeleton";
        _health = 120.0f;
        _damage = 25.0f;
        _experience = 50.0f;

        base.Awake();
    }

    public override void ResetScale()
    {
        transform.localScale = _scale;
    }
}
