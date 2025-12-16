using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Monster _monster;
    public float _damage => _monster.Damage;

    private bool _hasHit;

    private void OnEnable()
    {
        _hasHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collided with {other.name}, {_damage}");

        if (other.gameObject.CompareTag("Monster")) return;

        if (other.TryGetComponent<GetDamage>(out var damage))
        {
            if (_hasHit) return;
            damage.GetDamage(_damage);
            _hasHit = true;
        }
    }

    public void EnableHitbox()
    {
        _hasHit = false;
        this.GetComponent<BoxCollider>().enabled = true;
        Debug.Log("AttackOn");
    }

    public void DisableHitbox()
    {
        this.GetComponent<BoxCollider>().enabled = false;
        Debug.Log("AttackOff");
    }
}
