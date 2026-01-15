using UnityEngine;

public class BossMonster : MonoBehaviour
{
    private float _damageMultiplier = 3.0f;
    private float _healthMultiplier = 5.0f;
    private float _scaleMultiplier = 1.5f;
    private float _experienceMultiplier = 2.0f;
    private bool _isBoss = false;

    private bool _applied = false;

    public bool IsBoss
    {
        get { return _isBoss; }
        set { _isBoss = value; }
    }

    public bool CheckBoss()
    {
        if (_isBoss) return true;
        return false;
    }

    public void Boss()
    {
        if (_applied) return;
        _applied = true;

        Monster monster = GetComponent<Monster>();

        monster.IsBoss(_damageMultiplier, _healthMultiplier, _scaleMultiplier, _experienceMultiplier);
    }
}
