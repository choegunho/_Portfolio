using UnityEngine;

[CreateAssetMenu(menuName = "LevelUp/Ability/BossDamageUp")]
public class BossDamageUpAbility : AbilityData
{
    public float _value;

    public override void OnAcquire(PlayerStateController player)
    {
        player.BossDamage += _value;
    }
}
