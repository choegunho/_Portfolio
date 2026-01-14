using System;
using Unity.VisualScripting;
using Unity.UI;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateController : MonoBehaviour, GetDamage
{
    [SerializeField] private Camera _cam;
    [SerializeField] private GameObject _weapon;
    [SerializeField] private GameObject _healthBarPrefab;
    [SerializeField] private GameObject _expBarPrefab;
    [SerializeField] private Transform worldCanvasTransform;
    [SerializeField] private GameObject _shieldEffect;
    [SerializeField] private LevelUpUI levelUpUI;
    private float _rotateSpeed = 5.0f;
    private float gravity = -9.81f;
    private float yVelocity = 0f;

    private bool _canAttack = true;

    // 카메라 초기위치 설정 
    Vector3 camOffset = new Vector3(0.0f, 5.0f, -2.5f);

    // 플레이어 스탯
    private float _health = 100.0f;
    private float _currentHealth;
    private float _defend = 5.0f;
    private float _damage = 10.0f;
    private float _moveSpeed = 2.5f;
    private int _level = 0;
    private float _levelUpExperience = 80.0f;  // 레벨업 까지 필요한 경험치
    private float _experience = 0.0f;

    private float _defendAttackCoolDown = 10.0f;
    private float _lastDefendAttackTime;

    private float _healthIncrease = 1.3f;
    private float _damageIncrease = 1.2f;

    private Animator _animator;

    private StateMachine _stateMachine;
    private PlayerIdleState _idleState;
    private PlayerMoveState _moveState;
    private PlayerAttackState _attackState;
    private PlayerDefendAttackState _defendAttackState;
    private PlayerDefendState _defendState;
    private PlayerDeadState _deadState;
    private CharacterController _characterController;
    private HPControl _healthUI;
    private ExpBar _expUI;

    public StateMachine StateMachine => _stateMachine;

    public PlayerIdleState IdleState => _idleState;

    public PlayerMoveState MoveState => _moveState;

    public PlayerAttackState AttackState => _attackState;

    public PlayerDefendAttackState DefendAttackState => _defendAttackState;

    public PlayerDefendState DefendState => _defendState;

    public PlayerDeadState DeadState => _deadState;

    public Animator Animator => _animator;

    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }

    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    public float Defense
    {
        get { return _defend; }
        set { _defend = value; }
    }

    public float CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }


    public float Speed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    private void Awake()
    {
        _currentHealth = _health;
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

        _characterController = GetComponent<CharacterController>();

        _stateMachine = new StateMachine();
        _idleState = new PlayerIdleState(this);
        _moveState = new PlayerMoveState(this);
        _attackState = new PlayerAttackState(this);
        _defendAttackState = new PlayerDefendAttackState(this);
        _defendState = new PlayerDefendState(this);
        _deadState = new PlayerDeadState(this);
        GameObject hb = Instantiate(_healthBarPrefab, worldCanvasTransform);
        _healthUI = hb.GetComponent<HPControl>();
        _healthUI.Init(_health, this.transform);
        GameObject eb = Instantiate(_expBarPrefab, worldCanvasTransform);
        _expUI = eb.GetComponent<ExpBar>();
        _expUI.Init(_levelUpExperience, this.transform);
        _lastDefendAttackTime = -_defendAttackCoolDown;
    }
    private void Start()
    {
        _stateMachine.ChangeState(_idleState);
    }

    /// <summary>
    /// 키보드 입력을 받아옴
    /// </summary>
    /// <returns></returns>
    public Vector2 GetMoveInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        return new Vector2(horizontal, vertical).normalized;
    }

    public void Move(Vector2 input)
    {
        Vector3 moveDir = new Vector3(input.x, 0.0f, input.y).normalized;

        Vector3 move = moveDir * _moveSpeed;

        // 중력적용
        if (_characterController.isGrounded)
        {
            yVelocity = -1f;
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }

        move.y = yVelocity;

        _characterController.Move(move * Time.deltaTime);

    }

    private void AttackStart()
    {
        _weapon.GetComponent<PlayerAttack>().HasHit = false;
        _weapon.GetComponent<BoxCollider>().enabled = true;
    }

    private void AttackEnd()
    {
        _weapon.GetComponent<BoxCollider>().enabled = false;
        _canAttack = true;
    }

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0) && _canAttack)
        {
            _stateMachine.ChangeState(AttackState);
            _canAttack = false;
        }
    }

    public void DefendAttack()
    {
        _lastDefendAttackTime = Time.time;
        _stateMachine.ChangeState(DefendAttackState);
        _canAttack = false;
    }

    public bool CanDefendAttack()
    {
        return Time.time >= _lastDefendAttackTime + _defendAttackCoolDown;
    }

    public float SetDamage()
    {
        if(StateMachine.GetCurrentState() == _defendAttackState)
        {
            return _damage * 1.5f;
        }
        return _damage;
    }

    public void Defend()
    {
        if (Input.GetMouseButton(1))
        {
            _stateMachine.ChangeState(DefendState);
            _shieldEffect.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void DisableShieldEffect()
    {
        _shieldEffect.GetComponent<MeshRenderer>().enabled = false;
    }

    public void GetDamage(float damage)
    {
        if(_stateMachine.GetCurrentState() == _defendState)
        {
            damage -= _defend;
            damage = Mathf.Max(damage, 0);
            _currentHealth -= damage;
            _healthUI.TakeDamage(damage);
            Debug.Log("Defend Success");
            Debug.Log($"{_health}");
        }
        else
        {
            if (_stateMachine.GetCurrentState() == _deadState) return;
            _currentHealth -= damage;
            _healthUI.TakeDamage(damage);
            Debug.Log($"{_health}");
        }
    }

    public bool IsDead()
    {
        if(_currentHealth <= 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 플레이어가 마우스를 바라보게함
    /// </summary>
    private void MouseControll()
    {
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mouseScreenPos = Input.mousePosition;

        Vector3 dir = mouseScreenPos - playerScreenPos;

        float targetAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        Quaternion targetRot = Quaternion.Euler(0f, targetAngle, 0f);

        // 부드러운 회전
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            _rotateSpeed * Time.deltaTime);
    }

    public void GainExperience(float amount)
    {
        Debug.Log($"Get {amount}exp");
        _experience += amount;

        while (_experience >= _levelUpExperience)
        {
            _experience -= _levelUpExperience;
            LevelUp();
        }
        _expUI.GetExp(_levelUpExperience, _experience);
    }

    private void LevelUp()
    {
        EXPManager.instance.LevelUpUI();
        _level++;
        _levelUpExperience += 30.0f;

        _health *= _healthIncrease;
        _currentHealth *= _healthIncrease;
        _damage *= _damageIncrease;

        _healthUI.UpdateHealth(_health, _currentHealth);
        Debug.Log("Level Up!");
        Debug.Log($"Level: {_level}");
    }

    public void UpdateUI()
    {
        _healthUI.UpdateHealth(_health, _currentHealth);
    }

    private void LateUpdate()
    {
        _cam.transform.position = transform.position + camOffset;
    }

    void Update()
    {
        if (StateMachine.GetCurrentState() != _deadState)
        {
            MouseControll();
        }

        _stateMachine.Update();
    }
}
