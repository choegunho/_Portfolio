using UnityEngine.UI;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class ChoiceAbility : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _descriptionText;

    private AbilityData _ability;
    private Action<AbilityData> _onSelect;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Onclick);
    }

    public void Set(AbilityData ability, Action<AbilityData> select)
    {
        _ability = ability;
        _onSelect = select;

        _nameText.text = _ability.abilityName;
        _descriptionText.text = _ability.description;
        _icon.sprite = _ability.icon;
    }

    public void Onclick()
    {
        _onSelect?.Invoke(_ability);
    }
}
