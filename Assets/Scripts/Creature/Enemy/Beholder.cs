using UnityEngine;

public class Beholder : Monster
{
    [SerializeField] private GameObject _projectilePref;
    [SerializeField] private Transform _firePoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        _health = 100;
        _damage = 10.0f;

        _attackRange = 3.0f;

        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();

        var projectile = Instantiate(_projectilePref, _firePoint.position, _firePoint.rotation);

        Vector3 _moveDir = (_targetTr.position - projectile.transform.position).normalized;

        _moveDir.y = 0.0f;

        projectile.GetComponent<EnemyProjectile>().Init(_damage, _moveDir);
    }
}
