using UnityEngine;

[CreateAssetMenu(menuName = "LevelUp/Ability/DamageUp")]
public class DamageUpAbility : AbilityData
{
    public float _value;

    public override void Apply(PlayerStateController player)
    {
        player.Damage += player.Damage * _value;
    }
}
