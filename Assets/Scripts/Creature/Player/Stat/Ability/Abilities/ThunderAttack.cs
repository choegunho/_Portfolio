using UnityEngine;

[CreateAssetMenu(menuName = "LevelUp/Ability/ThunderAttack")]
public class ThunderAttack : AbilityData
{
    [SerializeField] private float _damage = 5.0f;
    [SerializeField] private float _radius = 1.5f;          // 플레이어 주변 범위
    [SerializeField] private GameObject _ThunderEffect;

    public override void OnHit(PlayerStateController player, Monster target)
    {
        // 플레이어 위치 기준으로 번개 범위
        Vector3 center = player.transform.position;

        Collider[] hits = Physics.OverlapSphere(center, _radius);

        foreach (var hit in hits)
        {
            Monster monster = hit.GetComponent<Monster>();
            if (monster == null || monster == target) continue;

            Instantiate(_ThunderEffect, monster.transform.position, Quaternion.identity);
            monster.GetDamage(_damage);
        }
    }
}