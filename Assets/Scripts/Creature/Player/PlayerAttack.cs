using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerStateController _player;
    [SerializeField] private GameObject _attackEffect;

    public float _damage; 

    private bool _hashit;

    public bool HasHit
    {
        get { return _hashit; }
        set { _hashit = value; }
    }

    private void Awake()
    {
        _player = GetComponentInParent<PlayerStateController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true) return;

        if(other.TryGetComponent<GetDamage>(out var damaged))
        {
            Debug.Log("hit");
            _damage = _player.SetDamage();
            if (_hashit) return;
            
            bool isBoss = false;
            BossMonster bossMonster = other.GetComponent<BossMonster>();
            if (bossMonster != null)
            {
                isBoss = bossMonster.CheckBoss();
            }
            
            Monster monster = other.GetComponent<Monster>();
            if (monster == null) return;
            
            if (isBoss)
            {
                _damage *= _player.BossDamage;
            }
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Instantiate(_attackEffect, hitPoint, Quaternion.identity);
            _player.AbilityHandler.OnHitMonster(monster);
            damaged.GetDamage(_damage);
            _hashit = true;
        }
    }
}
