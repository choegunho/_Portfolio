using UnityEngine;

public class Beholder : Monster
{
    [SerializeField] private GameObject _projectilePref;
    [SerializeField] private Transform _firePoint;
    private float _projectileSpeed = 5.0f;

    protected override void Awake()
    {
        _name = "Beholder";
        _health = 250.0f;
        _damage = 22.0f;
        _experience = 40.0f;
        
        _chaseRange = 5.0f;
        _attackRange = 3.0f;

        base.Awake();
    }

    // 보스 능력치 설정
    public override void IsBoss(float damageMultiplier, float healthMultiplier, float scaleMultiplier, float experienceMultiplier)
    {
        base.IsBoss(damageMultiplier, healthMultiplier, scaleMultiplier, experienceMultiplier);
        transform.localScale = _baseScale;
        _attackRange = 6.0f;
        _projectileSpeed = 7.5f;
    }

    protected override void AttackDetect()
    {
        var projectile = Instantiate(_projectilePref, _firePoint.position, _firePoint.rotation);

        Vector3 _moveDir = (_player.position - projectile.transform.position).normalized;

        _moveDir.y = 0.0f;

        projectile.GetComponent<EnemyProjectile>().Init(_damage, _moveDir, _projectileSpeed);
    }
}
