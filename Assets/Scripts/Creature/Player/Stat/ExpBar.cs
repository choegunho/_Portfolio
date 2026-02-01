using UnityEngine.UI;
using UnityEngine;

public class ExpBar : MonoBehaviour
{
    [SerializeField] private Image _expFill;

    private float _maxExp;
    private float _currentExp;

    public void Init(float maxExp)
    {
        _maxExp = maxExp;
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
}
