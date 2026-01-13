using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private ChoiceAbility _abilityPrefab;
    [SerializeField] private Transform _choiceParent;
    [SerializeField] private AbilityDatabase _abilityDatabase;
    [SerializeField] private PlayerStateController _player;

    private List<ChoiceAbility> _abilities = new();

    public void Show()
    {
        Time.timeScale = 0f;

        ClearCards();

        List<AbilityData> randomAbilities = GetRandomAbilities(3);

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

    private List<AbilityData> GetRandomAbilities(int count)
    {
        List<AbilityData> pool = new List<AbilityData>(_abilityDatabase._abilities);

        List<AbilityData> result = new();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index); // 중복 방지
        }

        return result;
    }
}
