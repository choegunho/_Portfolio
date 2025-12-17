using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Idle,
    Chase,
    Attack,
    Hit,
    Dead
}

public class Monster : MonoBehaviour, GetDamage
{
    protected PlayerStateController _player;

    protected EnemyAttack _enemyAttack;

    protected State _currentState = State.Idle;

    protected bool _isDead = false;

    protected bool _isAttack = false;

    protected bool _isDamaged = false;

    protected float _health = 100.0f;

    protected float _damage;

    protected float _chaseRange = 5.0f;
    protected float _attackRange = 1.0f;

    private float _rotateSpeed = 5.0f;

    protected Transform _targetTr = null;  // 타겟의 위치

    protected float _targetDistance = 0.0f;

    NavMeshAgent _nmAgent;
    Animator _animator;

    public virtual float Damage
    {
        get { return _damage; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        _nmAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemyAttack = GetComponentInChildren<EnemyAttack>();

        if(_enemyAttack != null)
        {
            _enemyAttack._monster = this;
        }

        _nmAgent.speed = 1.5f;

        StartCoroutine(FSM());
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Weapon"))
        {
            if(_health > 0)
            {
                _health -= 10.0f;   // 수정예정
            }
        }
    }

    protected virtual void Idle()
    {
        _animator.SetBool("IsWalk", false);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _chaseRange);
        
        foreach(var col in colliders)
        {
            if (col.gameObject.tag.Contains("Player"))
            {
                _targetTr = col.gameObject.transform;

                break;
            }
        }
    }

    protected virtual void Chase()
    {
        if(_targetTr != null)
        {
            Vector3 lookDir = _targetTr.position - transform.position;
            lookDir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                 Quaternion.LookRotation(lookDir), _rotateSpeed * Time.deltaTime);
            _animator.SetBool("IsWalk", true);
            _nmAgent.isStopped = false;
            _nmAgent.SetDestination(_targetTr.position);
        }
    }
    private void OffAttack()
    {
        _isAttack = false;
    }

    protected virtual void Attack()
    {
        if (_targetTr != null)
        {
            Vector3 lookDir = _targetTr.position - transform.position;
            lookDir.y = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation,
                                 Quaternion.LookRotation(lookDir), _rotateSpeed * Time.deltaTime);

            if (!_isAttack)
            {
                transform.rotation = Quaternion.LookRotation(lookDir);
                _isAttack = true;
                _animator.SetTrigger("Attack");
            }
        }
    }

    protected void Hit()
    {
        _animator.SetTrigger("Damaged");
        _isDamaged = false;
    }

    protected void AttackOn()
    {
        _enemyAttack.EnableHitbox();
    }

    protected void AttackOff()
    {
        _enemyAttack.DisableHitbox();
    }

    protected void Dead()
    {
        _animator.SetTrigger("Dead");
        Invoke("Destroy", 2.0f);
    }

    protected void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void GetDamage(float damage)
    {
        if (_isDead) return;

        _health -= damage;

        if (_health <= 0)
        {
            _currentState = State.Dead;
            return;
        }

        _isDamaged = true;
        _currentState = State.Hit;
    }
    IEnumerator FSM()
    {
        while (!_isDead)
        {
            switch (_currentState)
            {
                case State.Idle:
                    _animator.SetBool("IsWalk", false);
                    _nmAgent.isStopped = true;
                    Idle();
                    yield return new WaitForSeconds(1.0f);
                    break;

                case State.Chase:
                    _animator.SetBool("IsWalk", true);
                    _nmAgent.isStopped = false;
                    Chase();
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Attack:
                    OffAttack();
                    _animator.SetBool("IsWalk", false);
                    _nmAgent.isStopped = true;
                    Attack();
                    yield return new WaitForSeconds(1.5f);
                    break;

                case State.Hit:
                    _animator.SetBool("IsWalk", false);
                    _nmAgent.isStopped = true;
                    Hit();
                    yield return new WaitForSeconds(0.5f);
                    break;

                case State.Dead:
                    Dead();
                    _isDead = true;
                    break;
            }
        }
    }

    protected virtual void UpdateState()
    {
        if (_targetTr != null && _targetDistance < _attackRange)
        {
            if(_health <= 0)
            {
                _currentState = State.Dead;
            }
            else
            {
                _currentState = State.Attack;
            }
        }
        else if (_targetTr != null && (_targetDistance >= _attackRange && _targetDistance <= _chaseRange))
        {
            if(_health <= 0)
            {
                _currentState = State.Dead;
            }
            else
            {
                _currentState = State.Chase;
            }
                
        }
        else
        {
            if(_health <= 0)
            {
                _currentState = State.Dead;
            }
            else
            {
                _currentState = State.Idle;
            }
        }

        if (_targetTr != null)
        {
            _targetDistance = Vector3.Distance(_targetTr.position, this.transform.position);
        }
    }


    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }
}
