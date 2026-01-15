using UnityEngine;

[CreateAssetMenu(menuName = "LevelUp/Ability/Heal")]
public class HealAbility : AbilityData
{
    public float _value;
    private float _maxHealth;

    public override void OnAcquire(PlayerStateController player)
    {
        _maxHealth = player.Health;
        player.CurrentHealth += (_maxHealth * _value);
        player.CurrentHealth = Mathf.Min(player.CurrentHealth, player.Health);        
        player.UpdateUI();
    }
}
