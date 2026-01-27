using System;
using Unity.VisualScripting;
using Unity.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerStateController : MonoBehaviour, GetDamage
{
    [SerializeField] private Camera _cam;
    [SerializeField] private GameObject _weapon;
    [SerializeField] private GameObject _healthBarPrefab;
    [SerializeField] private GameObject _expBarPrefab;
    [SerializeField] private Transform worldCanvasTransform;
    [SerializeField] private GameObject _shieldEffect;
    [SerializeField] private GameObject _levelUpUI;
    [SerializeField] private GameObject _gameOverUI;
    private AbilityHandler _abilityHandler;
    private float _rotateSpeed = 5.0f;
    private float gravity = -9.81f;
    private float yVelocity = 0f;

    private bool _canAttack = true;

    // 카메라 초기위치 설정 
    Vector3 camOffset = new Vector3(0.0f, 5.0f, -2.5f);

    // 플레이어 스탯
    private float _maxHealth = 100.0f;
    private float _currentHealth;
    private float _defend = 5.0f;
    private float _damage = 10.0f;
    private float _moveSpeed = 2.5f;
    private float _baseSpeed;
    private int _level = 0;
    private float _levelUpExperience = 80.0f;  // 레벨업 까지 필요한 경험치
    private float _experience = 0.0f;

    private float _defendAttackCoolDown = 10.0f;
    private float _lastDefendAttackTime;

    private float _attackCoolDown = 0.75f;
    private float _lastAttackTime;

    private float _healthIncrease = 1.3f;
    private float _damageIncrease = 1.2f;
    private float _bossDamage = 1.0f;

    private float _speedBuffTime = 0.0f;
    private float _speedMultiplier = 0.0f;

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
    public AbilityHandler AbilityHandler => _abilityHandler;

    public float Health
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
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

    public int Level
    {
        get { return _level; }
        set { _level = value; }
    }

    public float LevelUpExperience
    {
        get { return _levelUpExperience; }
        set { _levelUpExperience = value; }
    }

    public float Experience
    {
        get { return _experience; }
        set { _experience = value; }
    }

    public float BossDamage
    {
        get { return _bossDamage; }
        set { _bossDamage = value; }
    }

    public bool CanAttack
    {
        get { return _canAttack; }
        set { _canAttack = value; }
    }

    private void Awake()
    {
        _currentHealth = _maxHealth;
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

        _characterController = GetComponent<CharacterController>();
        _abilityHandler = GetComponent<AbilityHandler>();

        _stateMachine = new StateMachine();
        _idleState = new PlayerIdleState(this);
        _moveState = new PlayerMoveState(this);
        _attackState = new PlayerAttackState(this);
        _defendAttackState = new PlayerDefendAttackState(this);
        _defendState = new PlayerDefendState(this);
        _deadState = new PlayerDeadState(this);
        _lastDefendAttackTime = -_defendAttackCoolDown;

        _baseSpeed = _moveSpeed;
    }
    private void Start()
    {
        _stateMachine.ChangeState(_idleState);
        GameObject hb = Instantiate(_healthBarPrefab, worldCanvasTransform);
        _healthUI = hb.GetComponent<HPControl>();
        _healthUI.Init(_maxHealth, this.transform);
        GameObject eb = Instantiate(_expBarPrefab, worldCanvasTransform);
        _expUI = eb.GetComponent<ExpBar>();
        _expUI.Init(_levelUpExperience, this.transform);

        var data = GameManager.Instance;
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
    }

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0) && CheckCanAttack())
        {
            _canAttack = false;
            _stateMachine.ChangeState(AttackState);
            _lastAttackTime = Time.time;
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

    public bool CheckCanAttack()
    {
        if(Time.time >= _lastAttackTime + _attackCoolDown)
        {
            _canAttack = true;
            return _canAttack;
        }
        return false;
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
            Debug.Log($"{_maxHealth}");
        }
        else
        {
            if (_stateMachine.GetCurrentState() == _deadState) return;
            _currentHealth -= damage;
            _healthUI.TakeDamage(damage);
            Debug.Log($"{_maxHealth}");
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
        _levelUpUI.gameObject.SetActive(true);
        EXPManager.instance.LevelUpUI();
        _level++;
        _levelUpExperience += 30.0f;

        _maxHealth *= _healthIncrease;
        _currentHealth *= _healthIncrease;
        _damage *= _damageIncrease;

        _healthUI.UpdateHealth(_maxHealth, _currentHealth);
        Debug.Log("Level Up!");
        Debug.Log($"Level: {_level}");
    }

    public void SpeedBuff(float speed)
    {
        float duration = 3f;

        _speedMultiplier += _baseSpeed * speed;
        _moveSpeed = _baseSpeed + _speedMultiplier;

        _speedBuffTime = Time.time + duration;
    }

    public void UpdateUI()
    {
        _healthUI.UpdateHealth(_maxHealth, _currentHealth);
    }

    public void MainMenuScene(){
        Time.timeScale = 0f;
        _gameOverUI.gameObject.SetActive(true);
    }

    public void MenuChangeCount(){
        Invoke("MainMenuScene", 2.0f);
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

        if (_speedMultiplier > 0.0f && Time.time >= _speedBuffTime)
        {
            _speedMultiplier = 0.0f;
            _moveSpeed = _baseSpeed;
        }

        _stateMachine.Update();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _damage += 100.0f;
            _moveSpeed += 6.0f;
        }
    }
}
