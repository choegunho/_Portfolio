using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateController : MonoBehaviour, GetDamage
{
    [SerializeField] private float _moveSpeed = 2.5f;
    [SerializeField] private Camera _cam;
    [SerializeField] private GameObject _weapon;
    [SerializeField] private GameObject _healthBarPrefab;
    [SerializeField] private Transform worldCanvasTransform;
    [SerializeField] private GameObject _shieldEffect;
    private float _rotateSpeed = 5.0f;
    private float gravity = -9.81f;
    private float yVelocity = 0f;
    private float _defend = 15.0f;
    private bool _canAttack = true;

    // 카메라 초기위치 설정 
    Vector3 camOffset = new Vector3(0.0f, 5.0f, -2.5f);

    private float _health = 100.0f;
    private float _damage = 10.0f;

    private Animator _animator;

    private StateMachine _stateMachine;
    private PlayerIdleState _idleState;
    private PlayerMoveState _moveState;
    private PlayerAttackState _attackState;
    private PlayerDefendState _defendState;
    private PlayerDeadState _deadState;
    private CharacterController _characterController;
    private HPControl _healthUI;

    public StateMachine StateMachine => _stateMachine;

    public PlayerIdleState IdleState => _idleState;

    public PlayerMoveState MoveState => _moveState;

    public PlayerAttackState AttackState => _attackState;

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
    }

    private void Awake()
    {
        if(_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

        _characterController = GetComponent<CharacterController>();

        _stateMachine = new StateMachine();
        _idleState = new PlayerIdleState(this);
        _moveState = new PlayerMoveState(this);
        _attackState = new PlayerAttackState(this);
        _defendState = new PlayerDefendState(this);
        _deadState = new PlayerDeadState(this);
        GameObject hb = Instantiate(_healthBarPrefab, worldCanvasTransform);
        _healthUI = hb.GetComponent<HPControl>();
        _healthUI.Init(_health, this.transform);
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
            _health -= damage;
            _healthUI.TakeDamage(damage);
            Debug.Log("Defend Success");
            Debug.Log($"{_health}");
        }
        else
        {
            if (_stateMachine.GetCurrentState() == _deadState) return;
            _health -= damage;
            _healthUI.TakeDamage(damage);
            Debug.Log($"{_health}");
        }
    }

    public bool IsDead()
    {
        if(_health <= 0)
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
