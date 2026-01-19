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
    [SerializeField] private Text _levelUpText;
    private AbilityHandler _abilityHandler;

    private List<ChoiceAbility> _abilities = new List<ChoiceAbility>();
    private List<AbilityData> _pool = new List<AbilityData>();

    private void Awake()
    {
        _pool  = new List<AbilityData>(_abilityDatabase._abilities);
        _abilityHandler = _player.GetComponent<AbilityHandler>();
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
        _levelUpText.enabled = true;
    }

    private void OnSelect(AbilityData ability)
    {
        _abilityHandler.AddAbility(ability);
        Close();
    }

    private void Close()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        _levelUpText.enabled = false;
    }

    private void ClearCards()
    {
        foreach (var card in _abilities)
            Destroy(card.gameObject);

        _abilities.Clear();
    }

    private List<AbilityData> GetRandomAbilities(List<AbilityData> abilities, int count)
    {
        List<AbilityData> pool = new List<AbilityData>();

        // 이미 가진 unique 제거
        foreach (var a in abilities)
        {
            if (a.unique && _abilityHandler.HasAbility(a))
                continue;

            pool.Add(a);
        }

        List<AbilityData> result = new List<AbilityData>();

        while (result.Count < count && pool.Count > 0)
        {
            AbilityData ability = GetRandomAbility(pool);
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
