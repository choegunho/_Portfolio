using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.Multiplayer.Center.Common;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private ChoiceAbility _abilityPrefab;
    [SerializeField] private Transform _choiceParent;
    [SerializeField] private AbilityDatabase _abilityDatabase;
    [SerializeField] private PlayerStateController _player;

    private List<ChoiceAbility> _abilities = new List<ChoiceAbility>();
    private List<AbilityData> _pool = new List<AbilityData>();

    private void Awake()
    {
        _pool  = new List<AbilityData>(_abilityDatabase._abilities);
    }

    public void Show()
    {
        Time.timeScale = 0f;

        ClearCards();

        List<AbilityData> randomAbilities = GetRandomAbilities(_pool, 3);

        foreach (var ability in randomAbilities)
        {
            ChoiceAbility choice = Instantiate(_abilityPrefab, _choiceParent);
            choice.Set(ability, OnSelect);
            _abilities.Add(choice);
        }

        gameObject.SetActive(true);
    }

    private void OnSelect(AbilityData ability)
    {
        ability.Apply(_player);
        Close();
    }

    private void Close()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private void ClearCards()
    {
        foreach (var card in _abilities)
            Destroy(card.gameObject);

        _abilities.Clear();
    }

    private List<AbilityData> GetRandomAbilities(List<AbilityData> abilities, int count)
    {
        List<AbilityData> pool = new List<AbilityData>(abilities);

        List<AbilityData> result = new List<AbilityData>();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            AbilityData ability = GetRandomAbility(pool);

            if (ability == null)
                break;

            result.Add(ability);
            pool.Remove(ability);
        }

        return result;
    }

    public static AbilityData GetRandomAbility(List<AbilityData> abilities)
    {
        int totalWeight = 0;
        foreach (var ability in abilities)
            totalWeight += ability.weight;

        int rand = Random.Range(0, totalWeight);
        int current = 0;

        foreach (var ability in abilities)
        {
            current += ability.weight;
            if (rand < current)
                return ability;
        }

        return null;
    }
}
