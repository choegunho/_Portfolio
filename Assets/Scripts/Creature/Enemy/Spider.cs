using UnityEngine;

public class Spider : Monster
{
    private Vector3 _scale = new Vector3(0.2f, 0.2f, 0.2f);
    protected override void Awake()
    {
        _name = "Spider";
        _health = 1000.0f;
        _damage = 50.0f;
        _chaseRange = 4.0f;
        _experience = 50.0f;

        base.Awake();
    }

    public override void ResetScale()
    {
        transform.localScale = _scale;
    }
}
