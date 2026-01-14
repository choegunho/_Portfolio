using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelUp/Ability")]
public abstract class AbilityData : ScriptableObject
{
    public string _abilityName;

    public string _description;

    public Sprite icon;

    public int weight;

    public abstract void Apply(PlayerStateController player);
}
