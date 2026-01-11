using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class HPControl : MonoBehaviour
{
    [SerializeField] private Image _healthFill;
    [SerializeField] private Vector3 offset = new Vector3(0.0f, 3.0f, 0.0f);

    private Transform _target;
    private float _maxHealth;

    private float _currentHealth;

    public void Init(float maxHealth, Transform target)
    {
        _target = target;
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
        UpdateHealthUI();
    }

    public void UpdateMaxHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
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
        offset.y = transform.y + offset.y;
    }

    private void LateUpdate()
    {
        if (!_target) return;
        // 위치 고정
        transform.position =  _target.position + offset;
        transform.rotation = Quaternion.Euler(70.0f, 0f, 0f);
    }
}
