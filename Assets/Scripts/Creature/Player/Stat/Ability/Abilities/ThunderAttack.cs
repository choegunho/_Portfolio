using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelUp/Ability/ThunderAttack")]
public class ThunderAttack : AbilityData
{
    private float _damage = 5.0f;
    private float _radius = 0.2f;
    [SerializeField] private GameObject _ThunderEffect;

    public override void OnHit(PlayerStateController player, Monster target)
    {
        Collider[] hits = Physics.OverlapSphere(
            target.transform.position,
            _radius
            );

        foreach(var hit in hits)
        {
            Monster monster = hit.GetComponent<Monster>();
            if (monster != null && monster != target)
            {
                Instantiate(_ThunderEffect, monster.transform.position, Quaternion.identity);
                monster.GetDamage(_damage);
            }
        }
    }
}
