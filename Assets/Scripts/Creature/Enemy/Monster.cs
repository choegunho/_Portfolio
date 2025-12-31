using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, GetDamage
{
    protected enum State
    {
        Idle,
        Chase,
        Attack,
        Hit,
        Dead
    }
    protected float _chaseRange = 3.5f;   // 추격범위
    protected float _attackRange = 1.0f;  // 공격범위
    protected float _attackAngle = 30.0f;
    private float _rotateSpeed = 5.0f;  // 회전속도
    protected float _attackCoolDown = 2.0f;   // 공격 쿨
    private float _attackTimer; // 공격시간

    private bool _canAttack = false;

    private bool _isDead = false;

    private bool _hasHit = false;

    protected State _currentState = State.Idle;

    private EnemyAttack _enemyAttack;

    // 플레이어 위치
    protected Transform _player;

    protected string _name;

    protected float _health;

    protected float _damage;

    private float _playerDistance = 0.0f;

    NavMeshAgent _nmAgent;
    Animator _animator;
    CapsuleCollider _collider;

    public float Damage
    {
        get { return _damage; }
    }

    protected virtual void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _nmAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemyAttack = GetComponentInChildren<EnemyAttack>();
        _collider = GetComponent<CapsuleCollider>();

        if (_enemyAttack != null)
        {
            _enemyAttack._monster = this;
        }

        _nmAgent.speed = 1.5f;
        _nmAgent.updateRotation = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {

    }


    // Update is called once per frame
     void Update()
    {
        UpdateState();
        CheckState();
    }

    private void Idle()
    {
        if (_currentState == State.Dead) return;

        _animator.SetBool("IsWalk", false);
    }

    private void Chase()
    {
        if (_currentState == State.Dead) return;

        if (_player != null)
        {
            _nmAgent.SetDestination(_player.position);
            _animator.SetBool("IsWalk", true);
        }
    }

    protected virtual void Attack()
    {
        if (_currentState == State.Dead || _player == null) return;

        Vector3 lookDir = (_player.position - transform.position).normalized;
        lookDir.y = 0.0f;

        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(lookDir), _rotateSpeed * Time.deltaTime);

        if (!_canAttack) return;

        _animator.SetTrigger("Attack");
        _canAttack = false;
    }

    private void CanAttack()
    {
        _attackTimer += Time.deltaTime;

        if (_attackTimer >= _attackCoolDown)
        {
            _canAttack = true;
            _attackTimer = 0.0f;
        }
    }

    private void AttackDetect()
    {
        Vector3 center = transform.position + transform.forward * (_attackRange * 0.5f);

        float halfAngleRad = _attackAngle * 0.5f * Mathf.Deg2Rad;
        float cosThreshold = Mathf.Cos(halfAngleRad);

        Collider[] hits = Physics.OverlapSphere(center, _attackRange);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject.CompareTag("Player"))
            {
                Vector3 dir = (hit.transform.position - center).normalized;
                float dot = Vector3.Dot(transform.forward, dir);

                if (dot >= cosThreshold)
                {
                    if (hit.TryGetComponent<GetDamage>(out var damage))
                    {
                        if (_hasHit) return;
                        damage.GetDamage(_damage);
                        _hasHit = true;
                    }
                }
            }
        }
    }

    private void AttackOn()
    {
        AttackDetect();
    }

    private void AttackOff()
    {
        _hasHit = false;
    }

    private void Hit()
    {
        if (_currentState == State.Dead) return;

        _animator.SetTrigger("Damaged");
    }

    private void Dead()
    {
        if(_isDead)
        {
            _animator.SetTrigger("Dead");
            _collider.enabled = false;
            Invoke("DestroyMonster", 2.0f);
        }
    }

    private void DestroyMonster()
    {
        Destroy(gameObject);
    }

    public virtual void GetDamage(float damage)
    {
        if (_currentState == State.Dead) return;

        _health -= damage;

        _health = Mathf.Max(_health, 0);

        Debug.Log($"{_name}: {_health}");

        if (_health == 0)
        {
            _currentState = State.Dead;
        }
        else
        {
            _currentState = State.Hit;
        }
    }

    public void CheckState()
    {
        if (_player != null)
        {
            if (_playerDistance <= _chaseRange && _playerDistance >= _attackRange)
            {
                if(_health == 0)
                {
                    _currentState = State.Dead;
                }
                else
                {
                    _currentState = State.Chase;
                }
            }
            else if (_playerDistance < _attackRange)
            {
                if (_health == 0)
                {
                    _currentState = State.Dead;
                }
                else
                {
                    _currentState = State.Attack;
                }
            }
            else
            {
                if(_health == 0)
                {
                    _currentState = State.Dead;
                }
                else
                {
                    _currentState = State.Idle;
                }
            }
        }
    }

    private void UpdateState()
    {
        if (!_isDead)
        {
            switch (_currentState)
            {
                case State.Idle:
                    _nmAgent.isStopped = true;
                    _animator.SetBool("IsWalk", false);
                    Idle();
                    break;

                case State.Chase:
                    _nmAgent.updateRotation = true;
                    _nmAgent.isStopped = false;
                    Chase();
                    break;

                case State.Attack:
                    _nmAgent.updateRotation = false;
                    _nmAgent.isStopped = true;
                    _animator.SetBool("IsWalk", false);
                    Attack();
                    break;

                case State.Hit:
                    _nmAgent.isStopped = true;
                    _animator.SetBool("IsWalk", false);
                    Hit();
                    break;

                case State.Dead:
                    _animator.SetBool("IsWalk", false);
                    _nmAgent.isStopped = true;
                    _isDead = true;
                    Dead();
                    break;
            }
        }
       
        if (!_canAttack)
        {
            CanAttack();
        }

        _playerDistance = Vector3.Distance(_player.position, transform.position);
    }
}