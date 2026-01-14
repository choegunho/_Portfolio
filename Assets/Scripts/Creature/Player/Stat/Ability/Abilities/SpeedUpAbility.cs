using UnityEngine;

[CreateAssetMenu(menuName = "LevelUp/Ability/SpeedUp")]
public class SpeedUpAbility : AbilityData
{
    public float _value;

    public override void Apply(PlayerStateController player)
    {
        player.Speed += player.Speed * _value;
    }
}
