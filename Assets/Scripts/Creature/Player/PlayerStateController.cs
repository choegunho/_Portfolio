using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateController : MonoBehaviour, GetDamage
{
    [SerializeField] private float _moveSpeed = 2.5f;
    [SerializeField] private Camera _cam;
    [SerializeField] private GameObject _weapon;
    private float _rotateSpeed = 360.0f;
    private float gravity = -9.81f;
    private float yVelocity = 0f;
    private float _defend = 15.0f;

    // 카메라 초기위치 설정 
    Vector3 camOffset = new Vector3(0.0f, 5.0f, -2.5f);

    private float _health = 100.0f;
    private float _damaga = 10.0f;

    private Animator _animator;

    private StateMachine _stateMachine;
    private PlayerIdleState _idleState;
    private PlayerMoveState _moveState;
    private PlayerAttackState _attackState;
    private PlayerDefendState _defendState;
    private PlayerDeadState _deadState;
    private CharacterController _characterController;

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
        _weapon.GetComponent<BoxCollider>().enabled = true;
    }

    private void AttackEnd()
    {
        _weapon.GetComponent<BoxCollider>().enabled = false;
    }

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _stateMachine.ChangeState(AttackState);
        }
    }

    public void Defend()
    {
        if (Input.GetMouseButton(1))
        {
            _stateMachine.ChangeState(DefendState);
        }
    }

    public void GetDamage(float damage)
    {
        if(_stateMachine.GetCurrentState() == _defendState)
        {
            if(_health > 0)
            {
                damage -= _defend;
                if (damage < 0)
                {
                    damage = 0;
                }
                _health -= damage;
                Debug.Log("Defend Success");
                Debug.Log($"{_health}");
            }
        }
        else
        {
            if(_health > 0)
            {
                _health -= damage;
                Debug.Log($"{_health}");
            }
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
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 lookDir = hit.point - transform.position;
            lookDir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);
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

        _stateMachine.Update();
    }
}
