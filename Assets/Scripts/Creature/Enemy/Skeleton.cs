using UnityEngine;

public class Skeleton : Monster
{
    private Vector3 _scale = new Vector3(0.5f, 0.5f, 0.5f);
    protected override void Awake()
    {
        _name = "Skeleton";
        _health = 300.0f;
        _damage = 30.0f;
        _experience = 35.0f;

        base.Awake();
    }

    public override void ResetScale()
    {
        transform.localScale = _scale;
    }
}
