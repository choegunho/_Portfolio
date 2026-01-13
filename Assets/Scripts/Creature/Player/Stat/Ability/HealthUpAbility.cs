using UnityEngine;

[CreateAssetMenu(menuName = "LevelUp/Ability/HealthUp")]
public class HealthUpAbility : AbilityData
{
    public float _value;

    public override void Apply(PlayerStateController player)
    {
        player.Health += (player.Health * _value);
        player.UpdateUI();
    }
}
