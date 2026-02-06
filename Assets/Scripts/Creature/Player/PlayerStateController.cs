using System;
using System.Collections;
using System.Collections.Generic;
using Unity.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VFX;


public class PlayerStateController : MonoBehaviour, GetDamage
{
    [SerializeField] private Camera _cam;
    [SerializeField] private GameObject _weapon;
    [SerializeField] private GameObject _healthBarPrefab;
    [SerializeField] private GameObject _expBarPrefab;
    [SerializeField] private Transform worldCanvasTransform;
    [SerializeField] private GameObject _shieldEffect;
    [SerializeField] private GameObject _levelUpUI;
    [SerializeField] private LevelUI _levelUI;
    [SerializeField] private GameObject _skillEffect;
    [SerializeField] private GameObject _projectileSkill;
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
    private int _level = 1;
    private float _levelUpExperience = 80.0f;  // 레벨업 까지 필요한 경험치
    private float _experience = 0.0f;

    private float _skillAttackCoolDown = 5.0f;
    private float _lastSkillAttackTime;

    private float _projectileSkillCoolDown = 9.0f;
    private float _lastProjectileSkillTime;

    private float _attackCoolDown = 0.75f;
    private float _lastAttackTime;

    private float _healthIncrease = 1.3f;
    private float _damageIncrease = 1.2f;
    private float _bossDamage = 1.0f;

    private float _speedBuffTime = 0.0f;
    private float _tempSpeedBonus;     // 일시적 버프


    private Animator _animator;

    private StateMachine _stateMachine;
    private PlayerIdleState _idleState;
    private PlayerMoveState _moveState;
    private PlayerAttackState _attackState;
    private PlayerLongDistanceSkillState _projectileSkillState;
    private PlayerSkillState _skillAttackState;
    private PlayerDefendState _defendState;
    private PlayerDeadState _deadState;
    private CharacterController _characterController;
    private PlayerHealthUI _healthUI;
    private ExpBar _expUI;
    private StatUI[] _statUI;

    public StateMachine StateMachine => _stateMachine;

    public PlayerIdleState IdleState => _idleState;

    public PlayerMoveState MoveState => _moveState;

    public PlayerAttackState AttackState => _attackState;

    public PlayerLongDistanceSkillState ProjectileSkillState => _projectileSkillState;

    public PlayerSkillState SkillAttackState => _skillAttackState;

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
        _skillAttackState = new PlayerSkillState(this);
        _projectileSkillState = new PlayerLongDistanceSkillState(this);
        _defendState = new PlayerDefendState(this);
        _deadState = new PlayerDeadState(this);
        _lastSkillAttackTime = -_skillAttackCoolDown;
        _lastProjectileSkillTime = - _projectileSkillCoolDown;

        _statUI = FindObjectsByType<StatUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach(var stat in _statUI)
        {
            stat.UpdateUI();
        }

        _baseSpeed = _moveSpeed;
    }
    private void Start()
    {
        _stateMachine.ChangeState(_idleState);
        _healthUI = _healthBarPrefab.GetComponent<PlayerHealthUI>();
        _healthUI.Init(_maxHealth);
        _expUI = _expBarPrefab.GetComponent<ExpBar>();
        _expUI.Init(_levelUpExperience);
        _levelUI.Init();

        var data = GameManager.Instance;
    }

    public void ResetPlayer()
    {
        _stateMachine.ChangeState(IdleState);
        _animator.ResetTrigger("Dead");
        _animator.Play("Idle_Battle", 0, 0f); // 강제로 Idle 상태로 전환
        _maxHealth = 100.0f;
        _currentHealth = _maxHealth; 
        _defend = 5.0f;
        _damage = 10.0f;
        _moveSpeed = 2.5f;

        _level = 0;
        _levelUpExperience = 80.0f;
        _experience = 0.0f;
        _bossDamage = 0.0f;
        _healthUI = _healthBarPrefab.GetComponent<PlayerHealthUI>();
        _healthUI.Init(_maxHealth);

        _expUI = _expBarPrefab.GetComponent<ExpBar>();
        _expUI.Init(_levelUpExperience);
        _levelUI.Init();

        _abilityHandler.ResetAbilities();
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

    public void SkillAttack()
    {
        if (Input.GetKeyDown(KeyCode.Q) && CanSkillAttack())
        {
            _lastSkillAttackTime = Time.time;
            _stateMachine.ChangeState(SkillAttackState);
            _canAttack = false;
        }

        if(Input.GetKeyDown(KeyCode.R) && CanProjectileSkillAttack())
        {
            _lastProjectileSkillTime = Time.time;
            _stateMachine.ChangeState(ProjectileSkillState);
            _canAttack = false;
        }
    }

    private void Skill()
    {
        if(StateMachine.GetCurrentState() == _skillAttackState)
        {
            float radius = 1.3f;
            float angle = 120.0f;
            float damage = SetDamage();

            Instantiate(_skillEffect, transform.position, Quaternion.LookRotation(-transform.forward));

            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                radius);

            foreach (var hit in hits)
            {
                Monster monster = hit.GetComponent<Monster>();
                Vector3 dir = (hit.transform.position - transform.position).normalized;

                float dot = Vector3.Dot(transform.forward, dir);
                float limit = Mathf.Cos((angle * 0.5f) * Mathf.Deg2Rad);

                if (dot < limit || monster == null) continue;

                monster.GetDamage(damage);
            }
        }
        else if(StateMachine.GetCurrentState() == _projectileSkillState)
        {
            float damage = SetDamage();
            Vector3 _firepos = transform.position + new Vector3(0.0f, 0.5f, 0.0f);
            GameObject projectile = Instantiate(
                _projectileSkill,
                _firepos + transform.forward,
                transform.rotation * _projectileSkill.transform.rotation
            );
            projectile.GetComponent<ProjectileSkill>().SetDamage(damage);
        }
    }

    public bool CanSkillAttack()
    {
        return Time.time >= _lastSkillAttackTime + _skillAttackCoolDown;
    }

    public bool CanProjectileSkillAttack()
    {
        return Time.time >= _lastProjectileSkillTime + _projectileSkillCoolDown;
    }

    public float GetRemainSkillCoolTime()
    {
        float remain = (_lastSkillAttackTime + _skillAttackCoolDown) - Time.time;
        return Mathf.Max(remain, 0.0f);
    }

    public float GetRemainProjectileSkillCoolTime()
    {
        float remain = (_lastProjectileSkillTime + _projectileSkillCoolDown) - Time.time;
        return Mathf.Max(remain, 0.0f);
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
        if(StateMachine.GetCurrentState() == _skillAttackState)
        {
            return _damage * 1.5f;
        }
        else if(StateMachine.GetCurrentState() == _projectileSkillState)
        {
            return _damage * 1.7f;
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
        }
        else
        {
            if (_stateMachine.GetCurrentState() == _deadState) return;
            _currentHealth -= damage;
            _healthUI.TakeDamage(damage);
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

    public void StartGameOverCoroutine()
    {
        StartCoroutine(GameOverDelay());
    }

    private IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(2.0f);

        GameManager.Instance.GameOver();
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
        if(_stateMachine.GetCurrentState() == _deadState) return;

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

        _maxHealth *= _healthIncrease;
        _currentHealth *= _healthIncrease;
        _damage *= _damageIncrease;

        _levelUI.LevelUp(_level);
        _healthUI.UpdateHealth(_maxHealth, _currentHealth);
        UpdateUI();
    }
    public void SpeedBuff(float speed)
    {
        float duration = 3f;

        _tempSpeedBonus += speed;
        UpdateMoveSpeed();

        _speedBuffTime = Time.time + duration;
    }

    private void UpdateMoveSpeed()
    {
        _moveSpeed = _baseSpeed + _tempSpeedBonus;
        UpdateUI();
    }
    public void IncreaseBaseSpeed(float value)
    {
        _baseSpeed += value;
        UpdateMoveSpeed();
    }


    public void UpdateUI()
    {
        _healthUI.UpdateHealth(_maxHealth, _currentHealth);
        foreach (var stat in _statUI)
        {
            stat.UpdateUI();
        }
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

        if (_tempSpeedBonus > 0.0f && Time.time >= _speedBuffTime)
        {
            _tempSpeedBonus = 0.0f;
            UpdateMoveSpeed();
        }

        _stateMachine.Update();

        if (Input.GetKeyDown(KeyCode.P))
        {
            LevelUp();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            GetDamage(9999.9f);
        }
    }
}
