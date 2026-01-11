using UnityEngine;

public class BossMonster : MonoBehaviour
{
    private float _damageMultiplier = 3.0f;
    private float _healthMultiplier = 5.0f;
    private float _scaleMultiplier = 1.5f;

    private bool _applied = false;

    public void Boss()
    {
        if (_applied) return;
        _applied = true;

        Monster monster = GetComponent<Monster>();

        monster.IsBoss(_damageMultiplier, _healthMultiplier, _scaleMultiplier);
    }
}
