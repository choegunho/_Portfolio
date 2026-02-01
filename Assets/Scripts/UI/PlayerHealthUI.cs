using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image _healthFill;
    [SerializeField] private Text _healthText;

    private float _maxHealth;

    private float _currentHealth;

    public void Init(float maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
        _healthText.text = $"{(int)_currentHealth}/{(int)_maxHealth}";
        UpdateHealthUI();
    }

    public void UpdateHealth(float maxHealth, float currentHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = currentHealth;
        _healthText.text = $"{(int)_currentHealth}/{(int)_maxHealth}";
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        _healthText.text = $"{(int)_currentHealth}/{(int)_maxHealth}";
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        _healthFill.fillAmount = _currentHealth / _maxHealth;
    }

    public void ActiveBar()
    {
        this.gameObject.SetActive(true);
    }

    public void DeActiveBar()
    {
        this.gameObject.SetActive(false);
    }
}
