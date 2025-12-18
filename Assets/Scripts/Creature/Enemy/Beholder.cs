using UnityEngine;

public class Beholder : Monster
{
    [SerializeField] private GameObject _projectilePref;
    [SerializeField] private Transform _firePoint;

    protected override void Awake()
    {
        _name = "Beholder";
        _health = 100;
        _damage = 10.0f;

        _chaseRange = 5.0f;
        _attackRange = 3.0f;

        base.Awake();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {

    }

    protected void FireProjectile()
    {
        var projectile = Instantiate(_projectilePref, _firePoint.position, _firePoint.rotation);

        Vector3 _moveDir = (_player.position - projectile.transform.position).normalized;

        _moveDir.y = 0.0f;

        projectile.GetComponent<EnemyProjectile>().Init(_damage, _moveDir);
    }
}
