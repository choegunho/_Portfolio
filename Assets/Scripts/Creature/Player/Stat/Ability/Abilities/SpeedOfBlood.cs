using UnityEngine;

[CreateAssetMenu (menuName ="LevelUp/Ability/SpeedOfBlood")]
public class SpeedOfBlood : AbilityData
{
    public float _speed = 0.1f;
    
    public override void OnKill(PlayerStateController player, Monster target)
    {
        if(target != null && target.CurrentState == Monster.State.Dead)
        {
            player.SpeedBuff(_speed);
        }
    }
}
