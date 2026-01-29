using UnityEngine;

[CreateAssetMenu(menuName = "LevelUp/Ability/SpeedUp")]
public class SpeedUpAbility : AbilityData
{
    public float _value;

    public override void OnAcquire(PlayerStateController player)
    {
        float IncreasedSpeed = player.Speed * _value;
        player.IncreaseBaseSpeed(IncreasedSpeed);
    }
}
