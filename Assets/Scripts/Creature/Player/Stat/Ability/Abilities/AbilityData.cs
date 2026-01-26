using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelUp/Ability")]
public abstract class AbilityData : ScriptableObject
{
    public string abilityName;

    public string description;

    public Sprite icon;

    public int weight;

    public bool unique = false;

    public bool stat = true;

    public virtual void OnAcquire(PlayerStateController player) { }
    public virtual void OnHit(PlayerStateController player, Monster target) { }
    public virtual void OnKill(PlayerStateController player, Monster target) { }
    public virtual void OnUpdate(PlayerStateController player) { }
}
