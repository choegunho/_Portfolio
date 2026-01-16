using UnityEngine;

[CreateAssetMenu(menuName = "LevelUp/Ability/BloodSucking")]
public class BloodSuckingAbility : AbilityData
{
    public float _heal;

    public override void OnKill(PlayerStateController player, Monster target)
    {
        if(target != null && target.CurrentState == Monster.State.Dead)
        {
            player.CurrentHealth += _heal;
            player.CurrentHealth = Mathf.Min(player.CurrentHealth, player.Health);
            player.UpdateUI();
        }
    }
} 
