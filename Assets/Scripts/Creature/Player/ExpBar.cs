using UnityEngine.UI;
using UnityEngine;

public class ExpBar : MonoBehaviour
{
    [SerializeField] private Image _expFill;
    [SerializeField] private Vector3 _offset = new Vector3(0.0f, 2.0f, 0.0f);
    [SerializeField] private Vector3 _scale = new Vector3(0.5f, 0.5f, 0.5f);

    private Transform _target;

    private float _maxExp;
    private float _currentExp;

    private void Awake()
    {
        transform.localScale = _scale;
    }

    public void Init(float maxExp, Transform target)
    {
        _maxExp = maxExp;
        _target = target;
        _currentExp = 0;
        UpdateExp();
    }

    public void GetExp(float maxExp, float currentExp)
    {
        _maxExp = maxExp;
        _currentExp = currentExp;
        UpdateExp();
    }

    public void UpdateExp()
    {
        _expFill.fillAmount = _currentExp / _maxExp;
    }

    private void LateUpdate()
    {
        if (!_target) return;
        // 위치 고정
        transform.position = _target.position + _offset;
        transform.rotation = Quaternion.Euler(70.0f, 0f, 0f);
    }
}
