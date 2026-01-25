using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class HPControl : MonoBehaviour
{
    [SerializeField] private Image _healthFill;
    [SerializeField] private Text _healthText;
    [SerializeField] private Vector3 offset = new Vector3(0.0f, 3.0f, 0.0f);
    private SkinnedMeshRenderer _targetRenderer;

    private Transform _target;
    private float _maxHealth;

    private float _currentHealth;

    public void Init(float maxHealth, Transform target)
    {
        _target = target;
        _targetRenderer = target.GetComponent<SkinnedMeshRenderer>();
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
        if(_currentHealth == 0)
        {
            Invoke("DestroyHealthBar", 2.0f);
        }
    }

    private void DestroyHealthBar()
    {
        Destroy(this.gameObject);
    }

    public void ActiveBar()
    {
        this.gameObject.SetActive(true);
    }

    public void DeActiveBar()
    {
        this.gameObject.SetActive(false);
    }

    public void UpdateOffset(Vector3 transform)
    {
        float height = _targetRenderer.bounds.size.y;
        offset.y = height + offset.y;
    }

    private void LateUpdate()
    {
        if (!_target) return;
        // 위치 고정
        transform.position =  _target.position + offset;
        transform.rotation = Quaternion.Euler(70.0f, 0f, 0f);
    }
}
