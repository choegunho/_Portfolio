using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerStateController _player;
    [SerializeField] private GameObject _attackEffect;

    public float _damage => _player.Damage; 

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
            if (_hashit) return;
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Instantiate(_attackEffect, hitPoint, Quaternion.identity);
            damaged.GetDamage(_damage);
            _hashit = true;
        }
    }
}
