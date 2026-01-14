using UnityEngine;

[CreateAssetMenu (menuName = "LevelUp/Ability/DefendUpAbility")]
public class DefendUpAbility : AbilityData
{
    public float _value;

    public override void Apply(PlayerStateController player)
    {
        player.Defense += (player.Defense * _value);
    }
}
